namespace EmitToolbox.Framework.Symbols.Extensions;

public static class VariableSymbolStringExtensions
{
    public static void Assign(this VariableSymbol<string> target, string value)
    {
        target.Context.Code.Emit(OpCodes.Ldstr, value);
        target.EmitStoreFromValue();
    }
}