using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework;

public partial class MethodBuildingContext(ILGenerator code)
{
    public ILGenerator Code { get; } = code;

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
}