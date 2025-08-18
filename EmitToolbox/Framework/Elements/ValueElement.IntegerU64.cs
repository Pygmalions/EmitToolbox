namespace EmitToolbox.Framework.Elements;

public static class ValueElementIntegerU64Extensions
{
    public static VariableElement<ulong> Add(this ValueElement<ulong> target, ValueElement<ulong> value)
    {
        var result = target.Context.DefineVariable<ulong>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<ulong> Add(this ValueElement<ulong> target, ulong value)
    {
        var result = target.Context.DefineVariable<ulong>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<ulong> Subtract(this ValueElement<ulong> target, ValueElement<ulong> value)
    {
        var result = target.Context.DefineVariable<ulong>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<ulong> Subtract(this ValueElement<ulong> target, ulong value)
    {
        var result = target.Context.DefineVariable<ulong>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<ulong> Multiply(this ValueElement<ulong> target, ValueElement<ulong> value)
    {
        var result = target.Context.DefineVariable<ulong>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<ulong> Multiply(this ValueElement<ulong> target, ulong value)
    {
        var result = target.Context.DefineVariable<ulong>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<ulong> Divide(this ValueElement<ulong> target, ValueElement<ulong> value)
    {
        var result = target.Context.DefineVariable<ulong>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<ulong> Divide(this ValueElement<ulong> target, ulong value)
    {
        var result = target.Context.DefineVariable<ulong>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<ulong> Modulus(this ValueElement<ulong> target, ValueElement<ulong> value)
    {
        var result = target.Context.DefineVariable<ulong>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<ulong> Modulus(this ValueElement<ulong> target, ulong value)
    {
        var result = target.Context.DefineVariable<ulong>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreValue();
        return result;
    }

    public static VariableElement<ulong> Negate(this ValueElement<ulong> target)
    {
        var result = target.Context.DefineVariable<ulong>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Neg);
        result.EmitStoreValue();
        return result;
    }
    
    public static VariableElement<long> ToInteger64(this ValueElement<ulong> target)
    {
        var result = target.Context.DefineVariable<long>();
        
        target.EmitLoadAsValue();
        result.EmitStoreValue();
        
        return result;
    }
}