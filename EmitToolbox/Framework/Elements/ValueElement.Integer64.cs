namespace EmitToolbox.Framework.Elements;

public static class ValueElementInteger64Extensions
{
    public static VariableElement<long> Add(this ValueElement<long> target, ValueElement<long> value)
    {
        var result = target.Context.DefineVariable<long>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<long> Add(this ValueElement<long> target, long value)
    {
        var result = target.Context.DefineVariable<long>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<long> Subtract(this ValueElement<long> target, ValueElement<long> value)
    {
        var result = target.Context.DefineVariable<long>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<long> Subtract(this ValueElement<long> target, long value)
    {
        var result = target.Context.DefineVariable<long>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<long> Multiply(this ValueElement<long> target, ValueElement<long> value)
    {
        var result = target.Context.DefineVariable<long>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<long> Multiply(this ValueElement<long> target, long value)
    {
        var result = target.Context.DefineVariable<long>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<long> Divide(this ValueElement<long> target, ValueElement<long> value)
    {
        var result = target.Context.DefineVariable<long>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<long> Divide(this ValueElement<long> target, long value)
    {
        var result = target.Context.DefineVariable<long>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<long> Modulus(this ValueElement<long> target, ValueElement<long> value)
    {
        var result = target.Context.DefineVariable<long>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<long> Modulus(this ValueElement<long> target, long value)
    {
        var result = target.Context.DefineVariable<long>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<long> Negate(this ValueElement<long> target)
    {
        var result = target.Context.DefineVariable<long>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Neg);
        result.EmitStoreValue();
        return result;
    }
    
    public static VariableElement<ulong> ToIntegerU64(this ValueElement<long> target)
    {
        var result = target.Context.DefineVariable<ulong>();
        
        target.EmitLoadAsValue();
        result.EmitStoreValue();
        
        return result;
    }
}