using System.Runtime.CompilerServices;
using EmitToolbox.Extensions;
using EmitToolbox.Symbols;

namespace EmitToolbox.Builders;

public class AsyncStateMachineBuilder
{
    /// <summary>
    /// Task type of this state machine.
    /// </summary>
    public Type TaskType { get; }

    /// <summary>
    /// Result type of this state machine.
    /// </summary>
    public Type ResultType { get; }

    /// <summary>
    /// Type of the async method builder used by this builder.
    /// </summary>
    public Type AsyncMethodBuilderType { get; }

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
    /// It is outside the try-catch block.
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

    /// <summary>
    /// Create a new state machine builder for the specified method.
    /// </summary>
    /// <param name="caller">
    /// Method which will initialize and start this state machine.
    /// It is bound for the later calls to the <see cref="Capture"/> method.
    /// </param>
    /// <param name="resultType">
    /// Result type of this state machine.
    /// It can be void if this state machine does not return a value.
    /// If null, it will be automatically determined from the async method builder type if provided,
    /// or from the return type of the caller method.
    /// </param>
    /// <param name="builderType">
    /// Async method builder type to use.
    /// If null, it will be automatically determined from the return type of the caller method,
    /// or <see cref="AsyncTaskMethodBuilder"/> by default.
    /// </param>
    /// <param name="name">Name of this state machine type.</param>
    public AsyncStateMachineBuilder(
        DynamicMethod caller,
        Type? resultType = null,
        Type? builderType = null,
        string? name = null)
    {
        _callerMethod = caller;

        (ResultType, TaskType, AsyncMethodBuilderType) =
            ResolveStateMachineTypes(caller.BuildingMethod.ReturnType, resultType, builderType);

        // This state machine cannot be defined as a nested type.
        // Nested type cannot be built and used in dynamic type unless the enclosing type is also built.
        // See https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.typebuilder.createtype for details.
        var builderStateMachine = caller.DeclaringType.DeclaringAssembly.DefineStruct(
            name ?? $"{caller.BuildingMethod.Name}_{caller.BuildingMethod.MetadataToken}_AsyncStateMachine");
        builderStateMachine.ImplementInterface(typeof(IAsyncStateMachine));

        _contextStateMachine = new StateMachineContext
        {
            TypeBuilder = builderStateMachine,
            FieldStepNumber = builderStateMachine.FieldFactory
                .DefineInstance("_state", typeof(int)),
            FieldAsyncBuilder = builderStateMachine.FieldFactory
                .DefineInstance("_builder", AsyncMethodBuilderType)
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

    private static (Type ResultType, Type TaskType, Type BuilderType) ResolveStateMachineTypes(
        Type callerReturnType, Type? resultTypeOverride, Type? builderTypeOverride)
    {
        // If the async method builder type is provided, determine the result type based on its 'SetResult' method.
        if (builderTypeOverride != null)
        {
            if (builderTypeOverride.GetMethod(nameof(AsyncTaskMethodBuilder.SetResult))
                is not { } methodSetResult)
                throw new ArgumentException(
                    "Invalid async method builder type: it does not have a 'SetResult' method.");
            var taskType = builderTypeOverride.GetProperty("Task")?.PropertyType
                           ?? throw new ArgumentException(
                               "Invalid async method builder type: it does not have a 'Task' property.");
            if (resultTypeOverride != null)
                return (resultTypeOverride, taskType, builderTypeOverride);
            var parameters = methodSetResult.GetParameters();
            var resultType = parameters.Length == 0 ? typeof(void) : parameters[0].ParameterType;
            return (resultType, taskType, builderTypeOverride);
        }

        if (resultTypeOverride != null)
        {
            if (resultTypeOverride == typeof(void))
                return (resultTypeOverride, typeof(Task), typeof(AsyncTaskMethodBuilder));
            return (resultTypeOverride,
                typeof(Task<>).MakeGenericType(resultTypeOverride),
                typeof(AsyncValueTaskMethodBuilder<>).MakeGenericType(resultTypeOverride));
        }

        if (callerReturnType == typeof(void) || callerReturnType == typeof(Task))
            return (typeof(void), typeof(Task), typeof(AsyncTaskMethodBuilder));

        if (callerReturnType == typeof(ValueTask))
            return (typeof(void), typeof(ValueTask), typeof(AsyncValueTaskMethodBuilder));

        if (callerReturnType.IsGenericType)
        {
            var returnTypeDefinition = callerReturnType.GetGenericTypeDefinition();
            if (returnTypeDefinition == typeof(Task<>))
            {
                var resultType = callerReturnType.GetGenericArguments()[0];
                builderTypeOverride ??= typeof(AsyncTaskMethodBuilder<>).MakeGenericType(resultType);
                return (resultType, callerReturnType, builderTypeOverride);
            }

            if (returnTypeDefinition == typeof(ValueTask<>))
            {
                var resultType = callerReturnType.GetGenericArguments()[0];
                builderTypeOverride = typeof(AsyncValueTaskMethodBuilder<>)
                    .MakeGenericType(resultType);
                return (resultType, callerReturnType, builderTypeOverride);
            }
        }

        // Return type is not a task-like object, use 'Task<>' by default.
        builderTypeOverride ??= typeof(AsyncTaskMethodBuilder<>).MakeGenericType(callerReturnType);
        var taskWrapperType = typeof(Task<>).MakeGenericType(callerReturnType);
        return (callerReturnType, taskWrapperType, builderTypeOverride);
    }

    private FieldSymbol GetOrAddAwaiterField(Type awaiterType)
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
    /// Define a new field on the state machine type.
    /// </summary>
    /// <param name="type">Type of this field.</param>
    /// <param name="name">Optional name of this field.</param>
    /// <returns>Field symbol to use in the state machine.</returns>
    public FieldSymbol DefineStateMachineField(Type type, string? name = null)
    {
        name ??= $"_variable_{_variableFieldsCount++}";
        return _contextStateMachine.TypeBuilder.FieldFactory
            .DefineInstance(name, type, VisibilityLevel.Private)
            .SymbolOf(Method, Method.This());
    }

    /// <summary>
    /// Create a new field of the specified type to hold the specified value.
    /// Note that the async method is executed for multiple times with different step index;
    /// therefore, all variables should be considered temporary that will be lost across async scopes.
    /// </summary>
    /// <param name="symbol">Symbol whose value will be stored in a field.</param>
    /// <returns>Field symbol to use in the state machine.</returns>
    public FieldSymbol Hoist(ISymbol symbol)
    {
        var field = DefineStateMachineField(symbol.ContentType);
        field.AssignContent(symbol);
        return field;
    }

    /// <inheritdoc cref="Hoist"/>
    public FieldSymbol<TContent> Hoist<TContent>(ISymbol<TContent> symbol)
        => Hoist((ISymbol)symbol).AsSymbol<TContent>();

    /// <summary>
    /// Capture a symbol from the caller method so that it can be used in the state machine.
    /// </summary>
    /// <param name="symbol">Symbol from the caller context to capture into the state machine.</param>
    /// <returns>Symbol can be used in the state machine.</returns>
    public FieldSymbol Capture(ISymbol symbol)
    {
        if (symbol.Context != _callerMethod)
            throw new InvalidOperationException(
                $"Cannot capture symbol '{symbol}': it is not from the caller method '{_callerMethod}'.");
        var field = _contextStateMachine.TypeBuilder.FieldFactory.DefineInstance(
            $"_capture_{_capturedVariables.Count}",
            symbol.ContentType);
        _capturedVariables.Add(new CapturedVariable
        {
            VariableField = field,
            CallerSymbol = symbol
        });
        return field.SymbolOf(Method, Method.This());
    }

    /// <inheritdoc cref="Capture"/>
    public FieldSymbol<TContent> Capture<TContent>(ISymbol<TContent> symbol)
        => Capture((ISymbol)symbol).AsSymbol<TContent>();

    /// <summary>
    /// Emit code to await for a task-like object and return the result if it has one.
    /// A task-like object is the object that has a 'GetAwaiter' method that returns a valid awaiter.
    /// A valid awaiter satisfies all the following requirements:
    /// <br/> - Implements the interface 'ICriticalNotifyCompletion'.
    /// <br/> - Has a bool 'IsCompleted' property.
    /// <br/> - Has a 'GetResult' method.
    /// </summary>
    /// <remarks>
    /// Any values not stored in fields will be lost across async scopes.
    /// Use <see cref="Hoist"/> to retain values across async scopes.
    /// </remarks>
    /// <param name="task">Symbol of a task-like object.</param>
    /// <returns>Symbol of the task result if it has one.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the specified task is not a task-like object.
    /// </exception>
    public FieldSymbol? Await(ISymbol task)
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

        var fieldAwaiter = GetOrAddAwaiterField(symbolAwaiter.ContentType);
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
        var fieldResult = DefineStateMachineField(methodGetResult.ReturnType);
        fieldResult.AssignValue(symbolResult);

        return fieldResult;
    }

    /// <inheritdoc cref="Await(EmitToolbox.Symbols.ISymbol)"/>
    public void Await(ISymbol<Task> task)
        => Await((ISymbol)task);

    /// <inheritdoc cref="Await(EmitToolbox.Symbols.ISymbol)"/>
    public void Await(ISymbol<ValueTask> task)
        => Await((ISymbol)task);

    /// <inheritdoc cref="Await(EmitToolbox.Symbols.ISymbol)"/>
    public FieldSymbol<TResult> Await<TResult>(ISymbol<Task<TResult>> task)
        => Await((ISymbol)task)!.AsSymbol<TResult>();

    /// <inheritdoc cref="Await(EmitToolbox.Symbols.ISymbol)"/>
    public FieldSymbol<TResult> Await<TResult>(ISymbol<ValueTask<TResult>> task)
        => Await((ISymbol)task)!.AsSymbol<TResult>();

    /// <summary>
    /// Set the result in the async method builder
    /// and then finish the building of this state machine.
    /// </summary>
    /// <param name="result">
    /// The result to set into the async method builder. This can be null if the state machine has a void return type.
    /// For non-void return types, the result cannot be null and must be assignable to the result type.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the state machine is already built, the result is null for a non-void return type,
    /// or the result is not assignable to the expected result type of the state machine.
    /// </exception>
    public void Finish(ISymbol? result)
    {
        if (_contextStateMachine.TypeBuilder.IsBuilt)
            throw new InvalidOperationException(
                "Failed to complete the state machine: it is already built.");

        if (result == null && ResultType != typeof(void))
            throw new InvalidOperationException(
                "Failed to complete the state machine: result cannot be null for non-void return type.");
        if (result != null && !result.BasicType.IsAssignableTo(ResultType))
            throw new InvalidOperationException(
                "Failed to complete the state machine: " +
                "result is not assignable to the result type of the state machine.");

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
    /// Initialize and start the state machine in the caller method,
    /// then return the task of the async method builder in use.
    /// </summary>
    /// <returns>
    /// Task symbol of this state machine, of type <see cref="TaskType"/>.
    /// </returns>
    public ISymbol Execute()
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
                .AssignContent(capturedVariable.CallerSymbol);
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

        public required ISymbol CallerSymbol { get; init; }
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
        /// <summary>
        /// Create a new state machine builder for this method to initialize and start.
        /// </summary>
        /// <param name="resultType">
        /// Result type of this state machine. It can be void if this state machine does not return a value.
        /// </param>
        /// <param name="builderType">
        /// Async method builder type to use.
        /// For example, <see cref="AsyncTaskMethodBuilder"/> and <see cref="AsyncValueTaskMethodBuilder"/>.
        /// </param>
        /// <param name="name">Name of this state machine type.</param>
        public AsyncStateMachineBuilder DefineAsyncStateMachine(
            Type? resultType = null,
            Type? builderType = null,
            string? name = null)
            => new(self, resultType, builderType, name);
    }
}