namespace EmitToolbox.Framework.Symbols.Extensions;

public static class AssignableSymbolBooleanExtensions
{
    public static void Assign(this IAssignableSymbol<bool> target, bool value)
    {
        target.Context.Code.Emit(value ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
        target.EmitStoreFromValue();
    }
}