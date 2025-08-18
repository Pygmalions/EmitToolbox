namespace EmitToolbox.Framework.Elements;

public static class ValueElementDoubleExtensions
{
    public static VariableElement<double> Add(this ValueElement<double> target, ValueElement<double> value)
    {
        var result = target.Context.DefineVariable<double>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<double> Add(this ValueElement<double> target, double value)
    {
        var result = target.Context.DefineVariable<double>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R8, value);
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<double> Subtract(this ValueElement<double> target, ValueElement<double> value)
    {
        var result = target.Context.DefineVariable<double>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<double> Subtract(this ValueElement<double> target, double value)
    {
        var result = target.Context.DefineVariable<double>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R8, value);
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<double> Multiply(this ValueElement<double> target, ValueElement<double> value)
    {
        var result = target.Context.DefineVariable<double>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<double> Multiply(this ValueElement<double> target, double value)
    {
        var result = target.Context.DefineVariable<double>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R8, value);
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<double> Divide(this ValueElement<double> target, ValueElement<double> value)
    {
        var result = target.Context.DefineVariable<double>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<double> Divide(this ValueElement<double> target, double value)
    {
        var result = target.Context.DefineVariable<double>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R8, value);
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<double> Modulus(this ValueElement<double> target, ValueElement<double> value)
    {
        var result = target.Context.DefineVariable<double>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<double> Modulus(this ValueElement<double> target, double value)
    {
        var result = target.Context.DefineVariable<double>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R8, value);
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<double> Negate(this ValueElement<double> target)
    {
        var result = target.Context.DefineVariable<double>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Neg);
        result.EmitStoreValue();
        return result;
    }
}