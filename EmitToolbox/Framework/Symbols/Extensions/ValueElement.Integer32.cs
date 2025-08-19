namespace EmitToolbox.Framework.Symbols.Extensions;

public static class ValueElementInteger32Extensions
{
    public static VariableSymbol<int> Add(this ValueSymbol<int> target, ValueSymbol<int> value)
    {
        var result = target.Context.Variable<int>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<int> Add(this ValueSymbol<int> target, int value)
    {
        var result = target.Context.Variable<int>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<int> Subtract(this ValueSymbol<int> target, ValueSymbol<int> value)
    {
        var result = target.Context.Variable<int>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<int> Subtract(this ValueSymbol<int> target, int value)
    {
        var result = target.Context.Variable<int>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<int> Multiply(this ValueSymbol<int> target, ValueSymbol<int> value)
    {
        var result = target.Context.Variable<int>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<int> Multiply(this ValueSymbol<int> target, int value)
    {
        var result = target.Context.Variable<int>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<int> Divide(this ValueSymbol<int> target, ValueSymbol<int> value)
    {
        var result = target.Context.Variable<int>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<int> Divide(this ValueSymbol<int> target, int value)
    {
        var result = target.Context.Variable<int>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<int> Modulus(this ValueSymbol<int> target, ValueSymbol<int> value)
    {
        var result = target.Context.Variable<int>();
        
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<int> Modulus(this ValueSymbol<int> target, int value)
    {
        var result = target.Context.Variable<int>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<int> Negate(this ValueSymbol<int> target)
    {
        var result = target.Context.Variable<int>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Neg);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<uint> ToIntegerU32(this ValueSymbol<int> target)
    {
        var result = target.Context.Variable<uint>();
        
        target.EmitLoadAsValue();
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<long> ToInteger64(this ValueSymbol<short> target)
    {
        var result = target.Context.Variable<long>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Conv_I8);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<ulong> ToIntegerU64(this ValueSymbol<short> target)
    {
        var result = target.Context.Variable<ulong>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Conv_I8);
        result.EmitStoreFromValue();
        
        return result;
    }
}