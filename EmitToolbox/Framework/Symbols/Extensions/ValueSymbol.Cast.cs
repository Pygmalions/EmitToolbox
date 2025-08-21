namespace EmitToolbox.Framework.Symbols.Extensions;

public static class ValueSymbolCastExtension
{
    public static VariableSymbol<TTo> Cast<TFrom, TTo>(this ValueSymbol<TFrom> target)
        where TFrom : class
        where TTo : class, TFrom
    {
        var result = target.Context.Variable<TTo>();

        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Castclass, typeof(TTo));
        result.EmitStoreFromValue();

        return result;
    }
}