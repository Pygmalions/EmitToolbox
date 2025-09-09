namespace EmitToolbox.Framework.Symbols.Extensions;

public static class ValueSymbolIntegerU64Extensions
{
    public static VariableSymbol<ulong> Add(this ISymbol<ulong> target, ISymbol<ulong> value)
    {
        var result = target.Context.Variable<ulong>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<ulong> Add(this ISymbol<ulong> target, ulong value)
    {
        var result = target.Context.Variable<ulong>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<ulong> Subtract(this ISymbol<ulong> target, ISymbol<ulong> value)
    {
        var result = target.Context.Variable<ulong>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<ulong> Subtract(this ISymbol<ulong> target, ulong value)
    {
        var result = target.Context.Variable<ulong>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<ulong> Multiply(this ISymbol<ulong> target, ISymbol<ulong> value)
    {
        var result = target.Context.Variable<ulong>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<ulong> Multiply(this ISymbol<ulong> target, ulong value)
    {
        var result = target.Context.Variable<ulong>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<ulong> Divide(this ISymbol<ulong> target, ISymbol<ulong> value)
    {
        var result = target.Context.Variable<ulong>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<ulong> Divide(this ISymbol<ulong> target, ulong value)
    {
        var result = target.Context.Variable<ulong>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<ulong> Modulus(this ISymbol<ulong> target, ISymbol<ulong> value)
    {
        var result = target.Context.Variable<ulong>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<ulong> Modulus(this ISymbol<ulong> target, ulong value)
    {
        var result = target.Context.Variable<ulong>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<ulong> Negate(this ISymbol<ulong> target)
    {
        var result = target.Context.Variable<ulong>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Neg);
        result.EmitStoreFromValue();
        return result;
    }
    
    public static VariableSymbol<long> ToInteger64(this ISymbol<ulong> target)
    {
        var result = target.Context.Variable<long>();
        
        target.EmitLoadAsValue();
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsEqualTo(this ISymbol<ulong> target, ISymbol<ulong> value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsEqualTo(this ISymbol<ulong> target, ulong value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, (long)value);
        target.Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsGreaterThan(this ISymbol<ulong> target, ISymbol<ulong> value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Cgt_Un);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsGreaterThan(this ISymbol<ulong> target, ulong value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, (long)value);
        target.Context.Code.Emit(OpCodes.Cgt_Un);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsLessThan(this ISymbol<ulong> target, ISymbol<ulong> value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Clt_Un);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsLessThan(this ISymbol<ulong> target, ulong value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, (long)value);
        target.Context.Code.Emit(OpCodes.Clt_Un);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsGreaterEqualThan(this ISymbol<ulong> target, ISymbol<ulong> value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Clt_Un);
        target.Context.Code.Emit(OpCodes.Ldc_I4_0);
        target.Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsGreaterEqualThan(this ISymbol<ulong> target, ulong value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, (long)value);
        target.Context.Code.Emit(OpCodes.Clt_Un);
        target.Context.Code.Emit(OpCodes.Ldc_I4_0);
        target.Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsLessEqualThan(this ISymbol<ulong> target, ISymbol<ulong> value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Cgt_Un);
        target.Context.Code.Emit(OpCodes.Ldc_I4_0);
        target.Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> IsLessEqualThan(this ISymbol<ulong> target, ulong value)
    {
        var result = target.Context.Variable<bool>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, (long)value);
        target.Context.Code.Emit(OpCodes.Cgt_Un);
        target.Context.Code.Emit(OpCodes.Ldc_I4_0);
        target.Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();
        
        return result;
    }
}