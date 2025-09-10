namespace EmitToolbox.Framework.Symbols.Extensions;

public static class ValueSymbolObjectExtensions
{
    public static VariableSymbol<object> Box<TValue>(this ISymbol<TValue> target) where TValue : struct
    {
        var result = target.Context.Variable<object>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Box, target.ContentType);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<TValue> Unbox<TValue>(this ISymbol<object> target) where TValue : struct
    {
        var result = target.Context.Variable<TValue>();
        
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Unbox_Any, typeof(TValue));
        result.EmitStoreFromValue();
        
        return result;
    }
}