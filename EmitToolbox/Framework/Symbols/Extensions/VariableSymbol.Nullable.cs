namespace EmitToolbox.Framework.Symbols.Extensions;

public static class VariableSymbolNullableExtensions
{
    public static void AssignNull<TValue>(this VariableSymbol<TValue?> target)
        where TValue : struct
    {
        target.EmitLoadAsAddress();
        target.Context.Code.Emit(OpCodes.Initobj, typeof(TValue?));
    }

    public static void AssignNull<TValue>(this VariableSymbol<TValue?> target)
        where TValue : class
    {
        target.Context.Code.Emit(OpCodes.Ldnull);
        target.EmitStoreFromValue();
    }
}