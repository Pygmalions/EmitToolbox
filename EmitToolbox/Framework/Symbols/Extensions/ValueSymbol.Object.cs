namespace EmitToolbox.Framework.Symbols.Extensions;

public static class ValueSymbolBoxExtensions
{
    public static VariableSymbol<object> Box<TValue>(this ValueSymbol<TValue> target) where TValue : struct
    {
        var result = target.Context.Variable<object>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Box, target.ValueType);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<TValue> Unbox<TValue>(this ValueSymbol<object> target) where TValue : struct
    {
        var result = target.Context.Variable<TValue>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Unbox_Any, typeof(TValue));
        result.EmitStoreFromValue();
        
        return result;
    }
}