namespace EmitToolbox.Framework.Symbols.Extensions;

public static class ValueElementFloatExtensions
{
    public static VariableSymbol<float> Add(this ValueSymbol<float> target, ValueSymbol<float> value)
    {
        var result = target.Context.Variable<float>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<float> Add(this ValueSymbol<float> target, float value)
    {
        var result = target.Context.Variable<float>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<float> Subtract(this ValueSymbol<float> target, ValueSymbol<float> value)
    {
        var result = target.Context.Variable<float>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<float> Subtract(this ValueSymbol<float> target, float value)
    {
        var result = target.Context.Variable<float>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<float> Multiply(this ValueSymbol<float> target, ValueSymbol<float> value)
    {
        var result = target.Context.Variable<float>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<float> Multiply(this ValueSymbol<float> target, float value)
    {
        var result = target.Context.Variable<float>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<float> Divide(this ValueSymbol<float> target, ValueSymbol<float> value)
    {
        var result = target.Context.Variable<float>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<float> Divide(this ValueSymbol<float> target, float value)
    {
        var result = target.Context.Variable<float>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<float> Modulus(this ValueSymbol<float> target, ValueSymbol<float> value)
    {
        var result = target.Context.Variable<float>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<float> Modulus(this ValueSymbol<float> target, float value)
    {
        var result = target.Context.Variable<float>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<float> Negate(this ValueSymbol<float> target)
    {
        var result = target.Context.Variable<float>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Neg);
        result.EmitStoreFromValue();
        return result;
    }
}