namespace EmitToolbox.Framework.Symbols.Extensions;

public static class ValueSymbolIntegerU32Extensions
{
    public static VariableSymbol<uint> Add(this ISymbol<uint> target, ISymbol<uint> value)
    {
        var result = target.Context.Variable<uint>();

        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<uint> Add(this ISymbol<uint> target, uint value)
    {
        var result = target.Context.Variable<uint>();

        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Add_Ovf_Un);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<uint> Subtract(this ISymbol<uint> target, ISymbol<uint> value)
    {
        var result = target.Context.Variable<uint>();

        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<uint> Subtract(this ISymbol<uint> target, uint value)
    {
        var result = target.Context.Variable<uint>();

        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<uint> Multiply(this ISymbol<uint> target, ISymbol<uint> value)
    {
        var result = target.Context.Variable<uint>();

        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<uint> Multiply(this ISymbol<uint> target, uint value)
    {
        var result = target.Context.Variable<uint>();

        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<uint> Divide(this ISymbol<uint> target, ISymbol<uint> value)
    {
        var result = target.Context.Variable<uint>();

        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<uint> Divide(this ISymbol<uint> target, uint value)
    {
        var result = target.Context.Variable<uint>();

        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<uint> Modulus(this ISymbol<uint> target, ISymbol<uint> value)
    {
        var result = target.Context.Variable<uint>();

        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<uint> Modulus(this ISymbol<uint> target, uint value)
    {
        var result = target.Context.Variable<uint>();

        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<uint> Negate(this ISymbol<uint> target)
    {
        var result = target.Context.Variable<uint>();

        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Neg);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<int> ToInteger32(this ISymbol<uint> target)
    {
        var result = target.Context.Variable<int>();

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

    public static VariableSymbol<bool> IsEqualTo(this ISymbol<uint> target, ISymbol<uint> value)
    {
        var result = target.Context.Variable<bool>();

        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<bool> IsEqualTo(this ISymbol<uint> target, uint value)
    {
        var result = target.Context.Variable<bool>();

        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, (int)value);
        target.Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<bool> IsGreaterThan(this ISymbol<uint> target, ISymbol<uint> value)
    {
        var result = target.Context.Variable<bool>();

        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Cgt);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<bool> IsGreaterThan(this ISymbol<uint> target, uint value)
    {
        var result = target.Context.Variable<bool>();

        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, (int)value);
        target.Context.Code.Emit(OpCodes.Cgt);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<bool> IsLessThan(this ISymbol<uint> target, ISymbol<uint> value)
    {
        var result = target.Context.Variable<bool>();

        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Clt);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<bool> IsLessThan(this ISymbol<uint> target, uint value)
    {
        var result = target.Context.Variable<bool>();

        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, (int)value);
        target.Context.Code.Emit(OpCodes.Clt);
        result.EmitStoreFromValue();

        return result;
    }

    public static VariableSymbol<bool> IsGreaterEqualThan(this ISymbol<uint> target, ISymbol<uint> value)
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

    public static VariableSymbol<bool> IsGreaterEqualThan(this ISymbol<uint> target, uint value)
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

    public static VariableSymbol<bool> IsLessEqualThan(this ISymbol<uint> target, ISymbol<uint> value)
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

    public static VariableSymbol<bool> IsLessEqualThan(this ISymbol<uint> target, uint value)
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