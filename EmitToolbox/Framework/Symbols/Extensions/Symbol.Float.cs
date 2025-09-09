namespace EmitToolbox.Framework.Symbols.Extensions;

public static class ValueSymbolFloatExtensions
{
    public static VariableSymbol<float> Add(this ISymbol<float> target, ISymbol<float> value)
    {
        var result = target.Context.Variable<float>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<float> Add(this ISymbol<float> target, float value)
    {
        var result = target.Context.Variable<float>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<float> Subtract(this ISymbol<float> target, ISymbol<float> value)
    {
        var result = target.Context.Variable<float>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<float> Subtract(this ISymbol<float> target, float value)
    {
        var result = target.Context.Variable<float>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<float> Multiply(this ISymbol<float> target, ISymbol<float> value)
    {
        var result = target.Context.Variable<float>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<float> Multiply(this ISymbol<float> target, float value)
    {
        var result = target.Context.Variable<float>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<float> Divide(this ISymbol<float> target, ISymbol<float> value)
    {
        var result = target.Context.Variable<float>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<float> Divide(this ISymbol<float> target, float value)
    {
        var result = target.Context.Variable<float>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<float> Modulus(this ISymbol<float> target, ISymbol<float> value)
    {
        var result = target.Context.Variable<float>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<float> Modulus(this ISymbol<float> target, float value)
    {
        var result = target.Context.Variable<float>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<float> Negate(this ISymbol<float> target)
    {
        var result = target.Context.Variable<float>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Neg);
        result.EmitStoreFromValue();
        return result;
    }
}