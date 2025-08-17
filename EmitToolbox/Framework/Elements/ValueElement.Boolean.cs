namespace EmitToolbox.Framework.Elements;

public static class ValueElementBooleanExtensions
{
    public static VariableElement<bool> Not(this ValueElement<bool> value)
    {
        var method = value.Context;
        var result = method.DefineVariable<bool>();
        
        value.EmitLoadAsValue();
        method.Code.Emit(OpCodes.Ldc_I4_0);
        method.Code.Emit(OpCodes.Ceq);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<bool> And(this ValueElement<bool> value, ValueElement<bool> other)
    {
        var method = value.Context;
        var result = method.DefineVariable<bool>();
        
        value.EmitLoadAsValue();
        other.EmitLoadAsValue();
        method.Code.Emit(OpCodes.Add);
        method.Code.Emit(OpCodes.Ldc_I4_2);
        method.Code.Emit(OpCodes.Ceq);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<bool> Or(this ValueElement<bool> value, ValueElement<bool> other)
    {
        var method = value.Context;
        var result = method.DefineVariable<bool>();
        
        value.EmitLoadAsValue();
        other.EmitLoadAsValue();
        method.Code.Emit(OpCodes.Add);
        method.Code.Emit(OpCodes.Ldc_I4_0);
        method.Code.Emit(OpCodes.Cgt);
        result.EmitStoreValue();
        
        return result;
    }
}