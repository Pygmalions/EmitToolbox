namespace EmitToolbox.Framework.Elements;

public static class ValueElementFloatExtensions
{
    public static VariableElement<float> Add(this ValueElement<float> target, ValueElement<float> value)
    {
        var result = target.Context.DefineVariable<float>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<float> Add(this ValueElement<float> target, float value)
    {
        var result = target.Context.DefineVariable<float>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<float> Subtract(this ValueElement<float> target, ValueElement<float> value)
    {
        var result = target.Context.DefineVariable<float>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<float> Subtract(this ValueElement<float> target, float value)
    {
        var result = target.Context.DefineVariable<float>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<float> Multiply(this ValueElement<float> target, ValueElement<float> value)
    {
        var result = target.Context.DefineVariable<float>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<float> Multiply(this ValueElement<float> target, float value)
    {
        var result = target.Context.DefineVariable<float>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<float> Divide(this ValueElement<float> target, ValueElement<float> value)
    {
        var result = target.Context.DefineVariable<float>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<float> Divide(this ValueElement<float> target, float value)
    {
        var result = target.Context.DefineVariable<float>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<float> Modulus(this ValueElement<float> target, ValueElement<float> value)
    {
        var result = target.Context.DefineVariable<float>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<float> Modulus(this ValueElement<float> target, float value)
    {
        var result = target.Context.DefineVariable<float>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<float> Negate(this ValueElement<float> target)
    {
        var result = target.Context.DefineVariable<float>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Neg);
        result.EmitStoreValue();
        return result;
    }
}