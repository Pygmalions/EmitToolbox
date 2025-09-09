namespace EmitToolbox.Framework.Symbols.Extensions;

public static class ValueSymbolInteger64Extensions
{
    public static VariableSymbol<long> Add(this ISymbol<long> target, ISymbol<long> value)
    {
        var result = target.Context.Variable<long>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<long> Add(this ISymbol<long> target, long value)
    {
        var result = target.Context.Variable<long>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<long> Subtract(this ISymbol<long> target, ISymbol<long> value)
    {
        var result = target.Context.Variable<long>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<long> Subtract(this ISymbol<long> target, long value)
    {
        var result = target.Context.Variable<long>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<long> Multiply(this ISymbol<long> target, ISymbol<long> value)
    {
        var result = target.Context.Variable<long>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<long> Multiply(this ISymbol<long> target, long value)
    {
        var result = target.Context.Variable<long>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<long> Divide(this ISymbol<long> target, ISymbol<long> value)
    {
        var result = target.Context.Variable<long>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<long> Divide(this ISymbol<long> target, long value)
    {
        var result = target.Context.Variable<long>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<long> Modulus(this ISymbol<long> target, ISymbol<long> value)
    {
        var result = target.Context.Variable<long>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<long> Modulus(this ISymbol<long> target, long value)
    {
        var result = target.Context.Variable<long>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<long> Negate(this ISymbol<long> target)
    {
        var result = target.Context.Variable<long>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Neg);
        result.EmitStoreFromValue();
        return result;
    }
    
    public static VariableSymbol<ulong> ToIntegerU64(this ISymbol<long> target)
    {
        var result = target.Context.Variable<ulong>();
        
        target.EmitLoadAsValue();
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsEqualTo(this ISymbol<long> target, ISymbol<long> value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsEqualTo(this ISymbol<long> target, long value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsGreaterThan(this ISymbol<long> target, ISymbol<long> value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Cgt);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsGreaterThan(this ISymbol<long> target, long value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Cgt);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsLessThan(this ISymbol<long> target, ISymbol<long> value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Clt);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsLessThan(this ISymbol<long> target, long value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Clt);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsGreaterEqualThan(this ISymbol<long> target, ISymbol<long> value)
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
    
    public static VariableSymbol<bool> IsGreaterEqualThan(this ISymbol<long> target, long value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Clt);
        target.Context.Code.Emit(OpCodes.Ldc_I4_0);
        target.Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsLessEqualThan(this ISymbol<long> target, ISymbol<long> value)
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
    
    public static VariableSymbol<bool> IsLessEqualThan(this ISymbol<long> target, long value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Cgt);
        target.Context.Code.Emit(OpCodes.Ldc_I4_0);
        target.Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();
        
        return result;
    }
}