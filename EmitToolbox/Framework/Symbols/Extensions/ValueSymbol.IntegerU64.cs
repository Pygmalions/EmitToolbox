namespace EmitToolbox.Framework.Symbols.Extensions;

public static class ValueSymbolIntegerU64Extensions
{
    public static VariableSymbol<ulong> Add(this ValueSymbol<ulong> target, ValueSymbol<ulong> value)
    {
        var result = target.Context.Variable<ulong>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<ulong> Add(this ValueSymbol<ulong> target, ulong value)
    {
        var result = target.Context.Variable<ulong>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<ulong> Subtract(this ValueSymbol<ulong> target, ValueSymbol<ulong> value)
    {
        var result = target.Context.Variable<ulong>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<ulong> Subtract(this ValueSymbol<ulong> target, ulong value)
    {
        var result = target.Context.Variable<ulong>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<ulong> Multiply(this ValueSymbol<ulong> target, ValueSymbol<ulong> value)
    {
        var result = target.Context.Variable<ulong>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<ulong> Multiply(this ValueSymbol<ulong> target, ulong value)
    {
        var result = target.Context.Variable<ulong>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<ulong> Divide(this ValueSymbol<ulong> target, ValueSymbol<ulong> value)
    {
        var result = target.Context.Variable<ulong>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<ulong> Divide(this ValueSymbol<ulong> target, ulong value)
    {
        var result = target.Context.Variable<ulong>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<ulong> Modulus(this ValueSymbol<ulong> target, ValueSymbol<ulong> value)
    {
        var result = target.Context.Variable<ulong>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<ulong> Modulus(this ValueSymbol<ulong> target, ulong value)
    {
        var result = target.Context.Variable<ulong>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<ulong> Negate(this ValueSymbol<ulong> target)
    {
        var result = target.Context.Variable<ulong>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Neg);
        result.EmitStoreFromValue();
        return result;
    }
    
    public static VariableSymbol<long> ToInteger64(this ValueSymbol<ulong> target)
    {
        var result = target.Context.Variable<long>();
        
        target.EmitLoadAsValue();
        result.EmitStoreFromValue();
        
        return result;
    }
}