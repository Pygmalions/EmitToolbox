namespace EmitToolbox.Framework.Symbols.Extensions;

public static class ValueSymbolStringExtensions
{
    public static VariableSymbol<bool> IsEqualTo(this ISymbol<string> target, ISymbol<string> other)
    {
        if (target.Context != other.Context)
            throw new InvalidOperationException("Cannot compare values from different contexts.");

        var result = target.Context.Variable<bool>();

        target.EmitLoadAsValue();
        other.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Call,
            typeof(string).GetMethod("Equals",
                BindingFlags.Static | BindingFlags.Public,
                [typeof(string), typeof(string)])!);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<bool> IsEqualTo(this ISymbol<string> target, string other)
    {
        var result = target.Context.Variable<bool>();

        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldstr, other);
        target.Context.Code.Emit(OpCodes.Call,
            typeof(string).GetMethod("Equals",
                BindingFlags.Static | BindingFlags.Public,
                [typeof(string), typeof(string)])!);
        result.EmitStoreFromValue();

        return result;
    }
}