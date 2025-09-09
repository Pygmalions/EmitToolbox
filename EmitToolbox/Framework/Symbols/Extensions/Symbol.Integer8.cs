namespace EmitToolbox.Framework.Symbols.Extensions;

public static class ValueSymbolInteger8Extensions
{
    public static VariableSymbol<byte> ToIntegerU8(this ISymbol<sbyte> target)
    {
        var result = target.Context.Variable<byte>();
        
        target.EmitLoadAsValue();
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<short> ToInteger16(this ISymbol<sbyte> target)
    {
        var result = target.Context.Variable<short>();
        
        target.EmitLoadAsValue();
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<ushort> ToIntegerU16(this ISymbol<sbyte> target)
    {
        var result = target.Context.Variable<ushort>();
        
        target.EmitLoadAsValue();
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<int> ToInteger32(this ISymbol<sbyte> target)
    {
        var result = target.Context.Variable<int>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Conv_I4);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<uint> ToIntegerU32(this ISymbol<sbyte> target)
    {
        var result = target.Context.Variable<uint>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Conv_I4);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<long> ToInteger64(this ISymbol<sbyte> target)
    {
        var result = target.Context.Variable<long>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Conv_I8);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<ulong> ToIntegerU64(this ISymbol<sbyte> target)
    {
        var result = target.Context.Variable<ulong>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Conv_I8);
        result.EmitStoreFromValue();
        
        return result;
    }
}