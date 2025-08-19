namespace EmitToolbox.Framework.Symbols.Extensions;

public static class ValueSymbolInteger64Extensions
{
    public static VariableSymbol<long> Add(this ValueSymbol<long> target, ValueSymbol<long> value)
    {
        var result = target.Context.Variable<long>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<long> Add(this ValueSymbol<long> target, long value)
    {
        var result = target.Context.Variable<long>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<long> Subtract(this ValueSymbol<long> target, ValueSymbol<long> value)
    {
        var result = target.Context.Variable<long>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<long> Subtract(this ValueSymbol<long> target, long value)
    {
        var result = target.Context.Variable<long>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<long> Multiply(this ValueSymbol<long> target, ValueSymbol<long> value)
    {
        var result = target.Context.Variable<long>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<long> Multiply(this ValueSymbol<long> target, long value)
    {
        var result = target.Context.Variable<long>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<long> Divide(this ValueSymbol<long> target, ValueSymbol<long> value)
    {
        var result = target.Context.Variable<long>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<long> Divide(this ValueSymbol<long> target, long value)
    {
        var result = target.Context.Variable<long>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<long> Modulus(this ValueSymbol<long> target, ValueSymbol<long> value)
    {
        var result = target.Context.Variable<long>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<long> Modulus(this ValueSymbol<long> target, long value)
    {
        var result = target.Context.Variable<long>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<long> Negate(this ValueSymbol<long> target)
    {
        var result = target.Context.Variable<long>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Neg);
        result.EmitStoreFromValue();
        return result;
    }
    
    public static VariableSymbol<ulong> ToIntegerU64(this ValueSymbol<long> target)
    {
        var result = target.Context.Variable<ulong>();
        
        target.EmitLoadAsValue();
        result.EmitStoreFromValue();
        
        return result;
    }
}