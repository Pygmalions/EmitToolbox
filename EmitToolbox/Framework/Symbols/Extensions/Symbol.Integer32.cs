namespace EmitToolbox.Framework.Symbols.Extensions;

public static class ValueSymbolInteger32Extensions
{
    public static VariableSymbol<int> Add(this ISymbol<int> target, ISymbol<int> value)
    {
        var result = target.Context.Variable<int>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<int> Add(this ISymbol<int> target, int value)
    {
        var result = target.Context.Variable<int>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<int> Subtract(this ISymbol<int> target, ISymbol<int> value)
    {
        var result = target.Context.Variable<int>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<int> Subtract(this ISymbol<int> target, int value)
    {
        var result = target.Context.Variable<int>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<int> Multiply(this ISymbol<int> target, ISymbol<int> value)
    {
        var result = target.Context.Variable<int>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<int> Multiply(this ISymbol<int> target, int value)
    {
        var result = target.Context.Variable<int>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<int> Divide(this ISymbol<int> target, ISymbol<int> value)
    {
        var result = target.Context.Variable<int>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<int> Divide(this ISymbol<int> target, int value)
    {
        var result = target.Context.Variable<int>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<int> Modulus(this ISymbol<int> target, ISymbol<int> value)
    {
        var result = target.Context.Variable<int>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<int> Modulus(this ISymbol<int> target, int value)
    {
        var result = target.Context.Variable<int>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<int> Negate(this ISymbol<int> target)
    {
        var result = target.Context.Variable<int>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Neg);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<uint> ToIntegerU32(this ISymbol<int> target)
    {
        var result = target.Context.Variable<uint>();
        
        target.EmitLoadAsValue();
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<long> ToInteger64(this ISymbol<short> target)
    {
        var result = target.Context.Variable<long>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Conv_I8);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<ulong> ToIntegerU64(this ISymbol<short> target)
    {
        var result = target.Context.Variable<ulong>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Conv_I8);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsEqualTo(this ISymbol<int> target, ISymbol<int> value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsEqualTo(this ISymbol<int> target, int value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsGreaterThan(this ISymbol<int> target, ISymbol<int> value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Cgt);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsGreaterThan(this ISymbol<int> target, int value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Cgt);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsLessThan(this ISymbol<int> target, ISymbol<int> value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Clt);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsLessThan(this ISymbol<int> target, int value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Clt);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsGreaterEqualThan(this ISymbol<int> target, ISymbol<int> value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Clt);
        target.Context.Code.Emit(OpCodes.Ldc_I4_0);
        target.Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsGreaterEqualThan(this ISymbol<int> target, int value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Clt);
        target.Context.Code.Emit(OpCodes.Ldc_I4_0);
        target.Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsLessEqualThan(this ISymbol<int> target, ISymbol<int> value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Cgt);
        target.Context.Code.Emit(OpCodes.Ldc_I4_0);
        target.Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsLessEqualThan(this ISymbol<int> target, int value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Cgt);
        target.Context.Code.Emit(OpCodes.Ldc_I4_0);
        target.Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();
        
        return result;
    }
}