using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework;

public abstract partial class MethodBuildingContext(TypeBuildingContext context, ILGenerator code)
{
    public ILGenerator Code { get; } = code;

    public TypeBuildingContext TypeContext { get; } = context;
    
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
    public ArgumentSymbol<TValue> Argument<TValue>(int index, bool isReference = false) 
        => new(this, index, isReference);

    public ExpressionSymbol<TValue> Expression<TValue>(Func<ValueSymbol<TValue>> expression)
        => new(this) {Expression = expression};

    public void EmplacePadding() => Code.Emit(OpCodes.Nop);
    
    public Label DefineLabel() => Code.DefineLabel();
    
    public void MarkLabel(Label label) => Code.MarkLabel(label);

    public void GoToLabel(Label label) => Code.Emit(OpCodes.Br, label);
    
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