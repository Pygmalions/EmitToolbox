namespace EmitToolbox.Framework.Symbols.Extensions;

public static class AssignableSymbolStringExtensions
{
    public static void Assign(this IAssignableSymbol<string> target, string value)
    {
        target.Context.Code.Emit(OpCodes.Ldstr, value);
        target.EmitStoreFromValue();
    }
}