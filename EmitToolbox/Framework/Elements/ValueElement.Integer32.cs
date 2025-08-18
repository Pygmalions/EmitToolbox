namespace EmitToolbox.Framework.Elements;

public static class ValueElementInteger32Extensions
{
    public static VariableElement<int> Add(this ValueElement<int> target, ValueElement<int> value)
    {
        var result = target.Context.DefineVariable<int>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<int> Add(this ValueElement<int> target, int value)
    {
        var result = target.Context.DefineVariable<int>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<int> Subtract(this ValueElement<int> target, ValueElement<int> value)
    {
        var result = target.Context.DefineVariable<int>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<int> Subtract(this ValueElement<int> target, int value)
    {
        var result = target.Context.DefineVariable<int>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<int> Multiply(this ValueElement<int> target, ValueElement<int> value)
    {
        var result = target.Context.DefineVariable<int>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<int> Multiply(this ValueElement<int> target, int value)
    {
        var result = target.Context.DefineVariable<int>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<int> Divide(this ValueElement<int> target, ValueElement<int> value)
    {
        var result = target.Context.DefineVariable<int>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<int> Divide(this ValueElement<int> target, int value)
    {
        var result = target.Context.DefineVariable<int>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<int> Modulus(this ValueElement<int> target, ValueElement<int> value)
    {
        var result = target.Context.DefineVariable<int>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<int> Modulus(this ValueElement<int> target, int value)
    {
        var result = target.Context.DefineVariable<int>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<int> Negate(this ValueElement<int> target)
    {
        var result = target.Context.DefineVariable<int>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Neg);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<uint> ToIntegerU32(this ValueElement<int> target)
    {
        var result = target.Context.DefineVariable<uint>();
        
        target.EmitLoadAsValue();
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<long> ToInteger64(this ValueElement<short> target)
    {
        var result = target.Context.DefineVariable<long>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Conv_I8);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<ulong> ToIntegerU64(this ValueElement<short> target)
    {
        var result = target.Context.DefineVariable<ulong>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Conv_I8);
        result.EmitStoreValue();
        
        return result;
    }
}