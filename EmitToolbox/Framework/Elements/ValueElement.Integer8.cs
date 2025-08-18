namespace EmitToolbox.Framework.Elements;

public static class ValueElementInteger8Extensions
{
    public static VariableElement<byte> ToIntegerU8(this ValueElement<sbyte> target)
    {
        var result = target.Context.DefineVariable<byte>();
        
        target.EmitLoadAsValue();
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<short> ToInteger16(this ValueElement<sbyte> target)
    {
        var result = target.Context.DefineVariable<short>();
        
        target.EmitLoadAsValue();
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<ushort> ToIntegerU16(this ValueElement<sbyte> target)
    {
        var result = target.Context.DefineVariable<ushort>();
        
        target.EmitLoadAsValue();
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<int> ToInteger32(this ValueElement<sbyte> target)
    {
        var result = target.Context.DefineVariable<int>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Conv_I4);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<uint> ToIntegerU32(this ValueElement<sbyte> target)
    {
        var result = target.Context.DefineVariable<uint>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Conv_I4);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<long> ToInteger64(this ValueElement<sbyte> target)
    {
        var result = target.Context.DefineVariable<long>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Conv_I8);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<ulong> ToIntegerU64(this ValueElement<sbyte> target)
    {
        var result = target.Context.DefineVariable<ulong>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Conv_I8);
        result.EmitStoreValue();
        
        return result;
    }
}