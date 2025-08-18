namespace EmitToolbox.Framework.Elements;

public static class ValueElementBoxExtensions
{
    public static VariableElement<object> Box<TValue>(this ValueElement<TValue> target) where TValue : struct
    {
        var result = target.Context.DefineVariable<object>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Box, target.ValueType);
        result.EmitStoreValue();
        
        return result;
    }
    
    public static VariableElement<TValue> Unbox<TValue>(this ValueElement<object> target) where TValue : struct
    {
        var result = target.Context.DefineVariable<TValue>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Unbox_Any, target.ValueType);
        result.EmitStoreValue();
        
        return result;
    }
}