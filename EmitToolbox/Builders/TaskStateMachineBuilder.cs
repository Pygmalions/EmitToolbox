using System.Runtime.CompilerServices;
using EmitToolbox.Extensions;
using EmitToolbox.Symbols;

namespace EmitToolbox.Builders;

public class TaskStateMachineBuilder
{
    /// <summary>
    /// Task type of this state machine.
    /// </summary>
    public Type TaskType { get; }

    /// <summary>
    /// Type of the state machine.
    /// </summary>
    public Type StateMachineType => _contextStateMachine.TypeBuilder.BuildingType;

    /// <summary>
    /// Dynamic method that initializes and starts this state machine.
    /// </summary>
    private readonly DynamicMethod _callerMethod;

    /// <summary>
    /// Context of building the state machine type.
    /// </summary>
    private readonly StateMachineContext _contextStateMachine;

    /// <summary>
    /// Label for the step redirection table.
    /// </summary>
    private readonly CodeLabel _labelRedirecting;

    /// <summary>
    /// Label for returning from the state machine 'MoveNext' method.
    /// It is outsides the try-catch block.
    /// </summary>
    private readonly CodeLabel _labelReturning;

    /// <summary>
    /// Labels for each step of the state machine.
    /// </summary>
    private readonly List<CodeLabel> _stepLabels = [];

    /// <summary>
    /// Field symbol of the async method builder.
    /// </summary>
    private readonly FieldSymbol _symbolFieldBuilder;

    /// <summary>
    /// Field symbol of the state, which is the current step of the state machine.
    /// </summary>
    private readonly FieldSymbol<int> _symbolFieldState;

    /// <summary>
    /// Fields for retaining awaiters, keyed by their types.
    /// </summary>
    private readonly Dictionary<Type, FieldSymbol> _awaiterFields = [];

    /// <summary>
    /// List of captured variables from the caller method.
    /// </summary>
    private readonly List<CapturedVariable> _capturedVariables = [];

    private int _variableFieldsCount;

    public DynamicMethod<Action> Method { get; }

    public TaskStateMachineBuilder(DynamicMethod caller, string? name = null, Type? typeAsyncMethodBuilder = null)
    {
        if (caller.ReturnType == typeof(Task) ||
            caller.ReturnType.IsGenericType && caller.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            TaskType = caller.ReturnType;
        else
            throw new InvalidOperationException(
                "Cannot create a state machine for a method that does not return a 'Task' or 'Task<T>'.");

        _callerMethod = caller;

        // This state machine cannot be defined as a nested type.
        // Nested type cannot be built and used in dynamic type unless the enclosing type is also built.
        // See https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.typebuilder.createtype for details.
        var builderStateMachine = caller.DeclaringType.DeclaringAssembly.DefineStruct(
            name ?? $"{caller.BuildingMethod.Name}_{caller.BuildingMethod.MetadataToken}_AsyncStateMachine");
        builderStateMachine.ImplementInterface(typeof(IAsyncStateMachine));

        typeAsyncMethodBuilder ??= TaskType == typeof(Task)
            ? typeof(AsyncTaskMethodBuilder)
            : typeof(AsyncTaskMethodBuilder<>).MakeGenericType(TaskType.GetGenericArguments()[0]);

        _contextStateMachine = new StateMachineContext
        {
            TypeBuilder = builderStateMachine,
            FieldStepNumber = builderStateMachine.FieldFactory
                .DefineInstance("_state", typeof(int)),
            FieldAsyncBuilder = builderStateMachine.FieldFactory
                .DefineInstance("_builder", typeAsyncMethodBuilder)
        };

        // Define a default constructor.
        builderStateMachine.MethodFactory.Constructor.DefineDefaultConstructor();
        
        var interfaceType = typeof(IAsyncStateMachine);

        // Define an empty 'SetStateMachine' method.
        var methodSetStateMachine = _contextStateMachine.TypeBuilder.MethodFactory
            .Instance.OverrideAction(interfaceType.GetMethod(nameof(IAsyncStateMachine.SetStateMachine))!);
        methodSetStateMachine.Return();

        Method = _contextStateMachine.TypeBuilder.MethodFactory.Instance.OverrideAction(
            interfaceType.GetMethod(nameof(IAsyncStateMachine.MoveNext))!);
        _symbolFieldBuilder = _contextStateMachine.FieldAsyncBuilder.SymbolOf(Method, Method.This());
        _symbolFieldState = _contextStateMachine.FieldStepNumber.SymbolOf<int>(Method, Method.This());
        _labelRedirecting = Method.DefineLabel();
        _labelReturning = Method.DefineLabel();

        // Note: instruction 'switch' cannot be used across protected regions;
        // therefore, the jumping table and the initial check are implemented in the try block.
        Method.Code.BeginExceptionBlock();

        // Jump to the step redirector if the state is not 0.
        _labelRedirecting.GotoIfFalse(_symbolFieldState.IsEqualTo(Method.Literal(0)));

        var labelInitialStep = Method.DefineLabel();
        _stepLabels.Add(labelInitialStep);
        labelInitialStep.Mark();
    }

    private FieldSymbol GetAwaiterField(Type awaiterType)
    {
        if (_awaiterFields.TryGetValue(awaiterType, out var field))
            return field;
        field = _contextStateMachine.TypeBuilder.FieldFactory
            .DefineInstance($"_awaiter_{_awaiterFields.Count}", awaiterType, VisibilityLevel.Private)
            .SymbolOf(Method, Method.This());
        _awaiterFields.Add(awaiterType, field);
        return field;
    }

    /// <summary>
    /// Create a new field of the specified type to hold the value.
    /// Note that the async method is executed for multiple times with different step index;
    /// therefore, all variables should be considered temporary that will be lost across async scopes.
    /// </summary>
    /// <param name="contentType">Type of this variable.</param>
    /// <returns>Field symbol to use in the state machine.</returns>
    public FieldSymbol NewField(Type contentType)
    {
        return _contextStateMachine.TypeBuilder.FieldFactory
            .DefineInstance($"_variable_{_variableFieldsCount++}",
                contentType, VisibilityLevel.Private)
            .SymbolOf(Method, Method.This());
    }

    /// <summary>
    /// Create a new field of the specified type to hold the specified value.
    /// Note that the async method is executed for multiple times with different step index;
    /// therefore, all variables should be considered temporary that will be lost across async scopes.
    /// </summary>
    /// <param name="symbol">Symbol whose value will be stored in a field.</param>
    /// <returns>Field symbol to use in the state machine.</returns>
    public FieldSymbol Retain(ISymbol symbol)
    {
        var field = NewField(symbol.ContentType);
        field.AssignContent(symbol);
        return field;
    }

    /// <inheritdoc cref="Retain"/>
    public FieldSymbol<TContent> Retain<TContent>(ISymbol<TContent> symbol)
        => Retain((ISymbol)symbol).AsSymbol<TContent>();

    /// <summary>
    /// Capture a symbol from the invoker method so that it can be used in the state machine.
    /// </summary>
    /// <param name="invokerSymbol">Symbol of the invoker context to capture into the state machine.</param>
    /// <returns>Symbol can be used in the state machine.</returns>
    public FieldSymbol Capture(ISymbol invokerSymbol)
    {
        if (invokerSymbol.Context != _callerMethod)
            throw new InvalidOperationException(
                $"Cannot capture symbol '{invokerSymbol}': it is not from the invoker method '{_callerMethod}'.");
        var field = _contextStateMachine.TypeBuilder.FieldFactory.DefineInstance(
            $"_capture_{_capturedVariables.Count}",
            invokerSymbol.ContentType);
        _capturedVariables.Add(new CapturedVariable
        {
            VariableField = field,
            InvokerSymbol = invokerSymbol
        });
        return field.SymbolOf(Method, Method.This());
    }

    /// <inheritdoc cref="Capture(ISymbol)"/>
    public FieldSymbol<TContent> Capture<TContent>(ISymbol<TContent> invokerSymbol)
        => Capture((ISymbol)invokerSymbol).AsSymbol<TContent>();

    /// <summary>
    /// Await for a task-like object and return the result if it has one.
    /// A task-like object is the object that has a 'GetAwaiter' method that returns a valid awaiter.
    /// A valid awaiter satisfies all the following requirements:
    /// <br/> - Implements the interface 'ICriticalNotifyCompletion'.
    /// <br/> - Has a bool 'IsCompleted' property.
    /// <br/> - Has a 'GetResult' method.
    /// </summary>
    /// <remarks>
    /// Any values not stored in fields will be lost across async scopes.
    /// Use <see cref="Retain"/> to retain values across async scopes.
    /// </remarks>
    /// <param name="task">Symbol of a task-like object.</param>
    /// <returns>Symbol of the task result if it has one.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the specified task is not a task-like object.
    /// </exception>
    public FieldSymbol? AwaitResult(ISymbol task)
    {
        var symbolAwaiter = task.Invoke(
            task.ContentType.GetMethod("GetAwaiter")
            ?? throw new InvalidOperationException(
                $"Cannot await '{task}': it does not have a 'GetAwaiter' method."));
        if (symbolAwaiter is null ||
            !symbolAwaiter.ContentType.IsAssignableTo(typeof(ICriticalNotifyCompletion)) ||
            symbolAwaiter.ContentType.GetMethod("GetResult",
                    BindingFlags.Public | BindingFlags.Instance, Type.EmptyTypes)
                is not { } methodGetResult)
            throw new InvalidOperationException(
                $"Cannot await '{task}': its 'GetAwaiter' method does not return an valid awaiter.");

        var fieldAwaiter = GetAwaiterField(symbolAwaiter.ContentType);
        fieldAwaiter.AssignContent(symbolAwaiter);

        using (Method.IfNot(fieldAwaiter.GetPropertyValue<bool>(
                   fieldAwaiter.ContentType.GetProperty("IsCompleted")!)))
        {
            _symbolFieldState.AssignContent(Method.Literal(_stepLabels.Count));
            _symbolFieldBuilder.Invoke(_symbolFieldBuilder.ContentType
                    .GetMethod("AwaitUnsafeOnCompleted")!
                    .MakeGenericMethod(fieldAwaiter.ContentType, _contextStateMachine.TypeBuilder.BuildingType),
                [fieldAwaiter, Method.This()]);
            _labelReturning.GotoFromProtectedRegion();
        }

        var labelNextStep = Method.DefineLabel();
        _stepLabels.Add(labelNextStep);
        labelNextStep.Mark();

        // The control maybe jumped to here when the awaiting task completes;
        // therefore, it is necessary to acquire the awaiter from the fields.
        var symbolResult = fieldAwaiter.Invoke(methodGetResult);
        fieldAwaiter.AssignNullOrDefault();
        if (symbolResult is null)
            return null;
        var fieldResult = NewField(methodGetResult.ReturnType);
        fieldResult.AssignValue(symbolResult);

        return fieldResult;
    }

    /// <summary>
    /// Mark the end of this step and awaits the given task.
    /// </summary>
    /// <remarks>
    /// Any values not stored in fields will be lost across async scopes.
    /// Use <see cref="Retain"/> to retain values across async scopes.
    /// </remarks>
    /// <param name="task">Task to await.</param>
    public void AwaitResult(ISymbol<Task> task)
        => AwaitResult((ISymbol)task);

    /// <inheritdoc cref="AwaitResult(ISymbol{Task})"/>
    public void AwaitResult(ISymbol<ValueTask> task)
        => AwaitResult((ISymbol)task);

    /// <summary>
    /// Mark the end of this step and awaits the result of the given task.
    /// </summary>
    /// <remarks>
    /// Any values not stored in fields will be lost across async scopes.
    /// Use <see cref="Retain"/> to retain values across async scopes.
    /// </remarks>
    /// <param name="task">Result task to await.</param>
    /// <returns>Symbol of the result.</returns>
    public FieldSymbol<TResult> AwaitResult<TResult>(ISymbol<Task<TResult>> task)
        => AwaitResult((ISymbol)task)!.AsSymbol<TResult>();

    /// <inheritdoc cref="AwaitResult{TResult}(ISymbol{Task{TResult}})"/>
    public FieldSymbol<TResult> AwaitResult<TResult>(ISymbol<ValueTask<TResult>> task)
        => AwaitResult((ISymbol)task)!.AsSymbol<TResult>();

    /// <summary>
    /// Initialize and start the state machine and return the task of the async method builder.
    /// </summary>
    /// <param name="result">
    /// Result to set into the async method builder.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the state machine is already built.
    /// </exception>
    /// <returns>Task to return from the invoker.</returns>
    public void Complete(ISymbol? result)
    {
        if (_contextStateMachine.TypeBuilder.IsBuilt)
            throw new InvalidOperationException(
                "Failed to complete the state machine: it is already built.");

        if (result == null && TaskType != typeof(Task))
            throw new InvalidOperationException(
                "Failed to complete the state machine: result cannot be null for non-void state machine.");
        if (result != null && result.BasicType.IsAssignableTo(TaskType))
            throw new InvalidOperationException(
                "Failed to complete the state machine: " +
                "result is not assignable to the return type of the state machine.");

        // Set the state to -2: completed.
        _symbolFieldState.AssignContent(Method.Literal(-2));

        var methodSetResult =
            _symbolFieldBuilder.ContentType.GetMethod("SetResult")
            ?? throw new Exception(
                $"Failed to complete the dynamic state machine: " +
                $"async method builder '{_symbolFieldBuilder.ContentType}' +" +
                $"does not have a 'SetResult' method.");
        if (result == null)
            _symbolFieldBuilder.Invoke(methodSetResult);
        else
            _symbolFieldBuilder.Invoke(methodSetResult, [result]);
        _labelReturning.GotoFromProtectedRegion();

        // Define the step redirection table.
        _labelRedirecting.Mark();

        if (_stepLabels.Count > 0)
        {
            _symbolFieldState.LoadAsValue();
            // Note: 'switch' instruction cannot be used to jump into or out of protected regions.
            Method.Code.Emit(OpCodes.Switch, _stepLabels.Select(label => label.Label).ToArray());
        }

        _labelReturning.GotoFromProtectedRegion();

        Method.Code.BeginCatchBlock(typeof(Exception));

        var variableException = Method.Variable(typeof(Exception));
        variableException.StoreContent();

        _symbolFieldState.AssignContent(Method.Literal(-2));
        _symbolFieldBuilder.Invoke(
            _symbolFieldBuilder.ContentType.GetMethod("SetException", [typeof(Exception)])!,
            [variableException]);
        _labelReturning.GotoFromProtectedRegion();

        Method.Code.EndExceptionBlock();

        _labelReturning.Mark();

        // Complete the 'MoveNext' method.
        Method.Return();

        // Build the state machine.
        _contextStateMachine.TypeBuilder.Build();
    }

    /// <summary>
    /// Initialize and start the state machine in the invoker method
    /// and return the task of the async method builder.
    /// </summary>
    /// <returns>Symbol of the task.</returns>
    public ISymbol Invoke()
    {
        // Initialize the state machine.
        var variableStateMachine = _callerMethod.New(
            _contextStateMachine.TypeBuilder.BuildingType.GetConstructor(Type.EmptyTypes)!);
        var symbolFieldBuilder = _contextStateMachine.FieldAsyncBuilder
            .SymbolOf(_callerMethod, variableStateMachine);
        var symbolFieldState = _contextStateMachine.FieldStepNumber
            .SymbolOf<int>(_callerMethod, variableStateMachine);
        symbolFieldBuilder.AssignContent(
            _callerMethod.Invoke(
                symbolFieldBuilder.ContentType.GetMethod("Create",
                    BindingFlags.Static | BindingFlags.Public)!)!);
        symbolFieldState.AssignContent(_callerMethod.Literal(0));

        // Bind the captured variables.
        foreach (var capturedVariable in _capturedVariables)
        {
            capturedVariable.VariableField
                .SymbolOf(_callerMethod, variableStateMachine)
                .AssignContent(capturedVariable.InvokerSymbol);
        }

        // Start the state machine.
        symbolFieldBuilder.Invoke(
            symbolFieldBuilder.BasicType.GetMethod("Start")!
                .MakeGenericMethod(variableStateMachine.BasicType),
            [variableStateMachine]);

        // Return the task of the async method builder.
        return symbolFieldBuilder
            .GetPropertyValue(symbolFieldBuilder.BasicType.GetProperty("Task")!);
    }

    private readonly struct CapturedVariable
    {
        public required DynamicField VariableField { get; init; }

        public required ISymbol InvokerSymbol { get; init; }
    }

    private readonly struct StateMachineContext
    {
        public required DynamicType TypeBuilder { get; init; }

        public required DynamicField FieldStepNumber { get; init; }

        public required DynamicField FieldAsyncBuilder { get; init; }
    }
}

public static class TaskStateMachineBuilderExtensions
{
    extension(DynamicMethod self)
    {
        public TaskStateMachineBuilder DefineAsyncStateMachine(
            string? name = null, Type? typeAsyncMethodBuilder = null)
            => new(self, name);
    }
}