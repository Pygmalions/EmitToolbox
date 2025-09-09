using EmitToolbox.Framework.Symbols;
using EmitToolbox.Framework.Symbols.Extensions;

namespace EmitToolbox.Framework;

public abstract partial class DynamicMethod(DynamicType typeContext, ILGenerator code)
{
    /// <summary>
    /// IL code of this method.
    /// </summary>
    public ILGenerator Code { get; } = code;

    /// <summary>
    /// Type that defines this method.
    /// </summary>
    public DynamicType TypeContext { get; } = typeContext;

    /// <summary>
    /// Whether this method is a static method.
    /// </summary>
    public abstract bool IsStatic { get; }

    /// <summary>
    /// Add a custom attribute to this method.
    /// </summary>
    /// <param name="attributeBuilder">Attribute builder of the custom attribute.</param>
    public abstract void MarkAttribute(CustomAttributeBuilder attributeBuilder);

    /// <summary>
    /// Define a local variable in this method.
    /// </summary>
    /// <param name="type">Type of the local variable.</param>
    /// <param name="isPinned">Whether this local variable is pinned.</param>
    /// <returns>Symbol of the local variable.</returns>
    public VariableSymbol Variable(Type type, bool isPinned = false)
        => new(this, Code.DeclareLocal(type, isPinned));

    /// <summary>
    /// Define a local variable in this method.
    /// </summary>
    /// <param name="modifier">Modifier of the value.</param>
    /// <param name="isPinned">Whether this local variable is pinned.</param>
    /// <typeparam name="TType">Type of the local variable.</typeparam>
    /// <returns>Symbol of the local variable.</returns>
    public VariableSymbol<TType> Variable<TType>(ValueModifier modifier = ValueModifier.None, bool isPinned = false)
        => new(this, modifier, isPinned);

    /// <summary>
    /// Define a symbol for the argument of this method.
    /// </summary>
    /// <param name="index">Zero-based index of the argument in the arguments list.</param>
    /// <param name="type">Type of the value.</param>
    /// <returns>Symbol of the argument.</returns>
    public abstract ArgumentSymbol Argument(int index, Type type);

    /// <summary>
    /// Define a symbol for the argument of this method.
    /// </summary>
    /// <param name="index">Zero-based index of the argument in the arguments list.</param>
    /// <param name="modifier">Modifier of the value.</param>
    /// <typeparam name="TValue">Type of the value.</typeparam>
    /// <returns>Symbol of the argument.</returns>
    public abstract ArgumentSymbol<TValue> Argument<TValue>(int index, ValueModifier modifier = ValueModifier.None);

    /// <summary>
    /// Define a symbol from an expression.
    /// </summary>
    /// <param name="expression">Expression function.</param>
    /// <typeparam name="TValue">Type of the return value of the expression.</typeparam>
    /// <returns>Symbol for the expression.</returns>
    public ExpressionSymbol<TValue> Expression<TValue>(Func<ISymbol<TValue>> expression)
        => new(this) { Expression = expression };

    /// <summary>
    /// Emit a no-operation instruction.
    /// </summary>
    public void Pass() => Code.Emit(OpCodes.Nop);

    /// <summary>
    /// Create a label. This label can be marked at a location and then be jumped to.
    /// </summary>
    public CodeLabel Label() => new(this);
    
    /// <summary>
    /// Instantiate a new instance by calling the specified constructor with the specified arguments.
    /// </summary>
    /// <param name="constructor">Constructor to invoke.</param>
    /// <param name="arguments">Arguments for the constructor.</param>
    /// <returns>Local variable of the instantiated instance.</returns>
    public VariableSymbol NewInstance(ConstructorInfo constructor, params Span<ISymbol> arguments)
    {
        var result = Variable(constructor.DeclaringType!);
        var parameters = constructor.GetParameters();
        if (parameters.Length != arguments.Length)
            throw new ArgumentException(
                "The number of arguments does not match the number of parameters.", nameof(arguments));
        foreach (var (index, parameter) in parameters.Index())
            arguments[index].EmitLoadAsParameter(parameter);
        Code.Emit(OpCodes.Newobj, constructor);
        return result;
    }
    
    /// <summary>
    /// Instantiate a new instance by calling the specified constructor with the specified arguments.
    /// </summary>
    /// <param name="constructor">Constructor to invoke.</param>
    /// <param name="arguments">Arguments for the constructor.</param>
    /// <returns>Local variable of the instantiated instance.</returns>
    public VariableSymbol<TType> NewInstance<TType>(ConstructorInfo constructor, params Span<ISymbol> arguments)
    {
        if (!constructor.DeclaringType!.IsAssignableTo(typeof(TType)))
            throw new ArgumentException(
                "The type of the constructor's declaring type cannot be assigned to the specified type.",
                nameof(constructor));
        var result = Variable<TType>();
        var parameters = constructor.GetParameters();
        if (parameters.Length != arguments.Length)
            throw new ArgumentException(
                "The number of arguments does not match the number of parameters.", nameof(arguments));
        foreach (var (index, parameter) in parameters.Index())
            arguments[index].EmitLoadAsParameter(parameter);
        Code.Emit(OpCodes.Newobj, constructor);
        return result;
    }

    public void If(ISymbol<bool> condition, Action? ifTrue = null, Action? ifFalse = null)
    {
        var labelElse = Code.DefineLabel();
        var labelEnd = Code.DefineLabel();

        condition.EmitLoadAsValue();
        Code.Emit(OpCodes.Brfalse, labelElse);

        ifTrue?.Invoke();
        Code.Emit(OpCodes.Nop);

        Code.Emit(OpCodes.Br, labelEnd);

        Code.MarkLabel(labelElse);

        ifFalse?.Invoke();
        Code.Emit(OpCodes.Nop);

        Code.MarkLabel(labelEnd);
    }

    public void While(ISymbol<bool> condition, Action body)
    {
        var labelStart = Code.DefineLabel();
        var labelEnd = Code.DefineLabel();

        Code.MarkLabel(labelStart);
        condition.EmitLoadAsValue();
        Code.Emit(OpCodes.Brfalse, labelEnd);

        body();
        Code.Emit(OpCodes.Nop);

        Code.Emit(OpCodes.Br, labelStart);
        Code.MarkLabel(labelEnd);
    }
}