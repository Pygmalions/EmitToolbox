namespace EmitToolbox.Framework.Symbols.Extensions;

public static class ValueSymbolIntegerU32Extensions
{
    public static VariableSymbol<uint> Add(this ValueSymbol<uint> target, ValueSymbol<uint> value)
    {
        var result = target.Context.Variable<uint>();

        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<uint> Add(this ValueSymbol<uint> target, uint value)
    {
        var result = target.Context.Variable<uint>();

        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Add_Ovf_Un);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<uint> Subtract(this ValueSymbol<uint> target, ValueSymbol<uint> value)
    {
        var result = target.Context.Variable<uint>();

        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<uint> Subtract(this ValueSymbol<uint> target, uint value)
    {
        var result = target.Context.Variable<uint>();

        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<uint> Multiply(this ValueSymbol<uint> target, ValueSymbol<uint> value)
    {
        var result = target.Context.Variable<uint>();

        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<uint> Multiply(this ValueSymbol<uint> target, uint value)
    {
        var result = target.Context.Variable<uint>();

        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<uint> Divide(this ValueSymbol<uint> target, ValueSymbol<uint> value)
    {
        var result = target.Context.Variable<uint>();

        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<uint> Divide(this ValueSymbol<uint> target, uint value)
    {
        var result = target.Context.Variable<uint>();

        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<uint> Modulus(this ValueSymbol<uint> target, ValueSymbol<uint> value)
    {
        var result = target.Context.Variable<uint>();

        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<uint> Modulus(this ValueSymbol<uint> target, uint value)
    {
        var result = target.Context.Variable<uint>();

        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<uint> Negate(this ValueSymbol<uint> target)
    {
        var result = target.Context.Variable<uint>();

        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Neg);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<int> ToInteger32(this ValueSymbol<uint> target)
    {
        var result = target.Context.Variable<int>();

        target.EmitLoadAsValue();
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<long> ToInteger64(this ValueSymbol<short> target)
    {
        var result = target.Context.Variable<long>();

        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Conv_I8);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<ulong> ToIntegerU64(this ValueSymbol<short> target)
    {
        var result = target.Context.Variable<ulong>();

        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Conv_I8);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<bool> IsEqualTo(this ValueSymbol<uint> target, ValueSymbol<uint> value)
    {
        var result = target.Context.Variable<bool>();

        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<bool> IsEqualTo(this ValueSymbol<uint> target, uint value)
    {
        var result = target.Context.Variable<bool>();

        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, (int)value);
        target.Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<bool> IsGreaterThan(this ValueSymbol<uint> target, ValueSymbol<uint> value)
    {
        var result = target.Context.Variable<bool>();

        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Cgt);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<bool> IsGreaterThan(this ValueSymbol<uint> target, uint value)
    {
        var result = target.Context.Variable<bool>();

        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, (int)value);
        target.Context.Code.Emit(OpCodes.Cgt);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<bool> IsLessThan(this ValueSymbol<uint> target, ValueSymbol<uint> value)
    {
        var result = target.Context.Variable<bool>();

        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Clt);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<bool> IsLessThan(this ValueSymbol<uint> target, uint value)
    {
        var result = target.Context.Variable<bool>();

        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, (int)value);
        target.Context.Code.Emit(OpCodes.Clt);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<bool> IsGreaterEqualThan(this ValueSymbol<uint> target, ValueSymbol<uint> value)
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

    public static VariableSymbol<bool> IsGreaterEqualThan(this ValueSymbol<uint> target, uint value)
    {
        var result = target.Context.Variable<bool>();

        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, (int)value);
        target.Context.Code.Emit(OpCodes.Clt);
        target.Context.Code.Emit(OpCodes.Ldc_I4_0);
        target.Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<bool> IsLessEqualThan(this ValueSymbol<uint> target, ValueSymbol<uint> value)
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

    public static VariableSymbol<bool> IsLessEqualThan(this ValueSymbol<uint> target, uint value)
    {
        var result = target.Context.Variable<bool>();

        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, (int)value);
        target.Context.Code.Emit(OpCodes.Cgt);
        target.Context.Code.Emit(OpCodes.Ldc_I4_0);
        target.Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();

        return result;
    }
}