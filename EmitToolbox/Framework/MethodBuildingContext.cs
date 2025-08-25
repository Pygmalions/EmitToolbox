using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework;

public abstract partial class MethodBuildingContext(TypeBuildingContext context, ILGenerator code)
{
    public ILGenerator Code { get; } = code;

    public TypeBuildingContext TypeContext { get; } = context;

    /// <summary>
    /// Whether this method is static.
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
    /// <param name="isReference">Whether this variable holds a reference.</param>
    /// <typeparam name="TValue">Type of the value.</typeparam>
    /// <returns>Symbol of the local variable.</returns>
    public LocalVariableSymbol<TValue> Variable<TValue>(bool isReference = false)
        => new(this, isReference);

    /// <summary>
    /// Define a symbol for the argument of this method.
    /// </summary>
    /// <param name="index">Zero-based index of the arguments.</param>
    /// <param name="isReference">Whether this variable holds a reference.</param>
    /// <typeparam name="TValue">Type of the value.</typeparam>
    /// <returns>Symbol of the argument.</returns>
    public abstract ArgumentSymbol<TValue> Argument<TValue>(int index, bool isReference = false);

    /// <summary>
    /// Create a symbol for an expression.
    /// </summary>
    /// <param name="expression">Expression function.</param>
    /// <typeparam name="TValue">Type of the return value of the expression.</typeparam>
    /// <returns>Symbol for the expression.</returns>
    public ExpressionSymbol<TValue> Expression<TValue>(Func<ValueSymbol<TValue>> expression)
        => new(this) { Expression = expression };

    /// <summary>
    /// Emit a no-operation instruction.
    /// </summary>
    public void Pass() => Code.Emit(OpCodes.Nop);

    /// <summary>
    /// Create a label. This label can be marked at a location and then be jumped to.
    /// </summary>
    public CodeLabel Label() => new(this);

    public void If(ValueSymbol<bool> condition, Action? ifTrue = null, Action? ifFalse = null)
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

    public void While(ValueSymbol<bool> condition, Action body)
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