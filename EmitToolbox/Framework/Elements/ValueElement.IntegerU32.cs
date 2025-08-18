namespace EmitToolbox.Framework.Elements;

public static class ValueElementIntegerU32Extensions
{
    public static VariableElement<uint> Add(this ValueElement<uint> target, ValueElement<uint> value)
    {
        var result = target.Context.DefineVariable<uint>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<uint> Add(this ValueElement<uint> target, uint value)
    {
        var result = target.Context.DefineVariable<uint>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Add_Ovf_Un);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<uint> Subtract(this ValueElement<uint> target, ValueElement<uint> value)
    {
        var result = target.Context.DefineVariable<uint>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<uint> Subtract(this ValueElement<uint> target, uint value)
    {
        var result = target.Context.DefineVariable<uint>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<uint> Multiply(this ValueElement<uint> target, ValueElement<uint> value)
    {
        var result = target.Context.DefineVariable<uint>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<uint> Multiply(this ValueElement<uint> target, uint value)
    {
        var result = target.Context.DefineVariable<uint>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<uint> Divide(this ValueElement<uint> target, ValueElement<uint> value)
    {
        var result = target.Context.DefineVariable<uint>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<uint> Divide(this ValueElement<uint> target, uint value)
    {
        var result = target.Context.DefineVariable<uint>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<uint> Modulus(this ValueElement<uint> target, ValueElement<uint> value)
    {
        var result = target.Context.DefineVariable<uint>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<uint> Modulus(this ValueElement<uint> target, uint value)
    {
        var result = target.Context.DefineVariable<uint>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<uint> Negate(this ValueElement<uint> target)
    {
        var result = target.Context.DefineVariable<uint>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Neg);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<int> ToInteger32(this ValueElement<uint> target)
    {
        var result = target.Context.DefineVariable<int>();
        
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