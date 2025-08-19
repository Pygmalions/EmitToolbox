namespace EmitToolbox.Framework.Symbols.Extensions;

public static class ValueElementBooleanExtensions
{
    public static VariableSymbol<bool> Not(this ValueSymbol<bool> value)
    {
        var method = value.Context;
        var result = method.Variable<bool>();
        
        value.EmitLoadAsValue();
        method.Code.Emit(OpCodes.Ldc_I4_0);
        method.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> And(this ValueSymbol<bool> value, ValueSymbol<bool> other)
    {
        var method = value.Context;
        var result = method.Variable<bool>();
        
        value.EmitLoadAsValue();
        other.EmitLoadAsValue();
        method.Code.Emit(OpCodes.Add);
        method.Code.Emit(OpCodes.Ldc_I4_2);
        method.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> Or(this ValueSymbol<bool> value, ValueSymbol<bool> other)
    {
        var method = value.Context;
        var result = method.Variable<bool>();
        
        value.EmitLoadAsValue();
        other.EmitLoadAsValue();
        method.Code.Emit(OpCodes.Add);
        method.Code.Emit(OpCodes.Ldc_I4_0);
        method.Code.Emit(OpCodes.Cgt);
        result.EmitStoreFromValue();
        
        return result;
    }
}