namespace EmitToolbox.Framework.Elements;

public static class VariableElementDoubleExtensions
{
    public static void Assign(this VariableElement<double> target, double value)
    {
        target.Context.Code.Emit(OpCodes.Ldc_R8, value);
        target.EmitStoreValue();
    }

    public static void SelfAdd(this VariableElement<double> target, ValueElement<double> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        target.EmitStoreValue();
    }

    public static void SelfAdd(this VariableElement<double> target, double value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R8, value);
        target.Context.Code.Emit(OpCodes.Add);
        target.EmitStoreValue();
    }

    public static void SelfSubtract(this VariableElement<double> target, ValueElement<double> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        target.EmitStoreValue();
    }

    public static void SelfSubtract(this VariableElement<double> target, double value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R8, value);
        target.Context.Code.Emit(OpCodes.Sub);
        target.EmitStoreValue();
    }

    public static void SelfMultiply(this VariableElement<double> target, ValueElement<double> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        target.EmitStoreValue();
    }

    public static void SelfMultiply(this VariableElement<double> target, double value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R8, value);
        target.Context.Code.Emit(OpCodes.Mul);
        target.EmitStoreValue();
    }

    public static void SelfDivide(this VariableElement<double> target, ValueElement<double> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        target.EmitStoreValue();
    }

    public static void SelfDivide(this VariableElement<double> target, double value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R8, value);
        target.Context.Code.Emit(OpCodes.Div);
        target.EmitStoreValue();
    }

    public static void SelfModulus(this VariableElement<double> target, ValueElement<double> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        target.EmitStoreValue();
    }

    public static void SelfModulus(this VariableElement<double> target, double value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R8, value);
        target.Context.Code.Emit(OpCodes.Rem);
        target.EmitStoreValue();
    }
}
