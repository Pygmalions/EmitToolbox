namespace EmitToolbox.Framework.Elements;

public static class VariableElementFloatExtensions
{
    public static void Assign(this VariableElement<float> target, float value)
    {
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.EmitStoreValue();
    }

    public static void SelfAdd(this VariableElement<float> target, ValueElement<float> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        target.EmitStoreValue();
    }

    public static void SelfAdd(this VariableElement<float> target, float value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.Context.Code.Emit(OpCodes.Add);
        target.EmitStoreValue();
    }

    public static void SelfSubtract(this VariableElement<float> target, ValueElement<float> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        target.EmitStoreValue();
    }

    public static void SelfSubtract(this VariableElement<float> target, float value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.Context.Code.Emit(OpCodes.Sub);
        target.EmitStoreValue();
    }

    public static void SelfMultiply(this VariableElement<float> target, ValueElement<float> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        target.EmitStoreValue();
    }

    public static void SelfMultiply(this VariableElement<float> target, float value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.Context.Code.Emit(OpCodes.Mul);
        target.EmitStoreValue();
    }

    public static void SelfDivide(this VariableElement<float> target, ValueElement<float> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        target.EmitStoreValue();
    }

    public static void SelfDivide(this VariableElement<float> target, float value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.Context.Code.Emit(OpCodes.Div);
        target.EmitStoreValue();
    }

    public static void SelfModulus(this VariableElement<float> target, ValueElement<float> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        target.EmitStoreValue();
    }

    public static void SelfModulus(this VariableElement<float> target, float value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.Context.Code.Emit(OpCodes.Rem);
        target.EmitStoreValue();
    }
}
