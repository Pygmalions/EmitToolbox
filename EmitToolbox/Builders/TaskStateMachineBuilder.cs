using System.Runtime.CompilerServices;
using EmitToolbox.Extensions;
using EmitToolbox.Symbols;

namespace EmitToolbox.Builders;

public class TaskStateMachineBuilder
{
    public Type ResultType { get; }

    public Type StateMachineType => _typeStateMachine.BuildingType;

    private readonly DynamicMethod _invoker;

    private readonly DynamicType _typeStateMachine;

    private readonly DynamicField _fieldBuilder;

    private readonly DynamicField _fieldState;

    private readonly LabelExtensions.CodeLabel _labelStepRedirector;

    private readonly List<LabelExtensions.CodeLabel> _steps = [];

    private readonly List<CapturedVariable> _capturedVariables = [];

    private readonly FieldSymbol _symbolFieldBuilder;

    private readonly FieldSymbol<int> _symbolFieldState;

    private readonly Dictionary<Type, FieldSymbol> _fieldsAwaiter = [];

    private int _fieldsVariableCount;

    public DynamicMethod<Action> Method { get; }

    public TaskStateMachineBuilder(
        DynamicMethod invoker, string name, Type? typeAsyncMethodBuilder = null)
    {
        if (invoker.ReturnType == typeof(Task) ||
            invoker.ReturnType.IsGenericType && invoker.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            ResultType = invoker.ReturnType;
        else
            throw new InvalidOperationException(
                "Cannot create a state machine for a method that does not return a 'Task' or 'Task<T>'.");

        _invoker = invoker;
        // This state machine cannot be defined as a nested type.
        // Nested type cannot be used in dynamic type unless the containing type is also built.
        // The state machine should be built before the invoker method.
        _typeStateMachine = invoker.DeclaringType.DeclaringAssembly.DefineStruct(
            invoker.BuildingMethod.Name + "_" + name);
        _typeStateMachine.ImplementInterface(typeof(IAsyncStateMachine));

        typeAsyncMethodBuilder ??=
            ResultType == typeof(Task)
                ? typeof(AsyncTaskMethodBuilder)
                : typeof(AsyncTaskMethodBuilder<>).MakeGenericType(ResultType.GetGenericArguments()[0]);

        _fieldBuilder = _typeStateMachine.FieldFactory.DefineInstance(
            "_builder", typeAsyncMethodBuilder);
        _fieldState = _typeStateMachine.FieldFactory.DefineInstance("_state", typeof(int));

        var baseType = typeof(IAsyncStateMachine);

        var methodSetStateMachine = _typeStateMachine.MethodFactory.Instance.OverrideAction(
            baseType.GetMethod(nameof(IAsyncStateMachine.SetStateMachine))!);
        methodSetStateMachine.Return();

        Method = _typeStateMachine.MethodFactory.Instance.OverrideAction(
            baseType.GetMethod(nameof(IAsyncStateMachine.MoveNext))!);
        _symbolFieldBuilder = _fieldBuilder.SymbolOf(Method, Method.This());
        _symbolFieldState = _fieldState.SymbolOf<int>(Method, Method.This());
        _labelStepRedirector = Method.DefineLabel();

        // Jump to the step redirector if the state is not 0.
        _labelStepRedirector.GotoIfFalse(_symbolFieldState.IsEqualTo(Method.Literal(0)));

        var labelInitialStep = Method.DefineLabel();
        _steps.Add(labelInitialStep);
        labelInitialStep.Mark();
    }

    private FieldSymbol GetAwaiterField(Type awaiterType)
    {
        if (_fieldsAwaiter.TryGetValue(awaiterType, out var field))
            return field;
        field = _typeStateMachine.FieldFactory
            .DefineInstance($"_awaiter_{_fieldsAwaiter.Count}", awaiterType, VisibilityLevel.Private)
            .SymbolOf(Method, Method.This());
        _fieldsAwaiter.Add(awaiterType, field);
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
        return _typeStateMachine.FieldFactory
            .DefineInstance($"_variable_{_fieldsVariableCount++}",
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
        if (invokerSymbol.Context != _invoker)
            throw new InvalidOperationException(
                $"Cannot capture symbol '{invokerSymbol}': it is not from the invoker method '{_invoker}'.");
        var field = _typeStateMachine.FieldFactory.DefineInstance(
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
    /// Thrown if the specified task is not a task-like object,
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
            _symbolFieldState.AssignContent(Method.Literal(_steps.Count));
            _symbolFieldBuilder.Invoke(_symbolFieldBuilder.ContentType
                    .GetMethod("AwaitUnsafeOnCompleted")!
                    .MakeGenericMethod(fieldAwaiter.ContentType, _typeStateMachine.Builder),
                [fieldAwaiter, Method.This()]);
            Method.Return();
        }

        var labelNextStep = Method.DefineLabel();
        _steps.Add(labelNextStep);
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
        if (_typeStateMachine.IsBuilt)
            throw new InvalidOperationException(
                "Failed to complete the state machine: it is already built.");

        if (result == null && ResultType != typeof(Task))
            throw new InvalidOperationException(
                "Failed to complete the state machine: result cannot be null for non-void state machine.");
        if (result != null && result.BasicType.IsAssignableTo(ResultType))
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
        Method.Return();

        // Define the step redirection table.
        _labelStepRedirector.Mark();

        _symbolFieldState.LoadAsValue();
        Method.Code.Emit(OpCodes.Switch, _steps.Select(label => label.Label).ToArray());

        // Complete the 'MoveNext' method.
        Method.Return();

        // Build the state machine.
        _typeStateMachine.Build();
    }

    /// <summary>
    /// Initialize and start the state machine in the invoker method
    /// and return the task of the async method builder.
    /// </summary>
    /// <returns>Symbol of the task.</returns>
    public ISymbol Invoke()
    {
        // Initialize the state machine.
        var variableStateMachine = _invoker.New(
            _typeStateMachine.BuildingType.GetConstructor(Type.EmptyTypes)!);
        var symbolFieldBuilder = _fieldBuilder.SymbolOf(_invoker, variableStateMachine);
        var symbolFieldState = _fieldState.SymbolOf<int>(_invoker, variableStateMachine);
        symbolFieldBuilder.AssignContent(
            _invoker.Invoke(
                symbolFieldBuilder.ContentType.GetMethod("Create",
                    BindingFlags.Static | BindingFlags.Public)!)!);
        symbolFieldState.AssignContent(_invoker.Literal(0));

        // Bind the captured variables.
        foreach (var capturedVariable in _capturedVariables)
        {
            capturedVariable.VariableField
                .SymbolOf(_invoker, variableStateMachine)
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
}

public static class TaskStateMachineBuilderExtensions
{
    extension(DynamicMethod self)
    {
        public TaskStateMachineBuilder DefineAsyncStateMachine(string name = "AsyncStateMachine")
            => new(self, name);
    }
}