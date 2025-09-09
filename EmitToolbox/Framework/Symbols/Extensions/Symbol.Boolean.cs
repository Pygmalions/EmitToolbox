namespace EmitToolbox.Framework.Symbols.Extensions;

public static class ValueSymbolBooleanExtensions
{
    public static VariableSymbol<bool> Not(this ISymbol<bool> value)
    {
        var method = value.Context;
        var result = method.Variable<bool>();
        
        value.EmitLoadAsValue();
        method.Code.Emit(OpCodes.Ldc_I4_0);
        method.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();
        
        return result;
    }
    
    public static VariableSymbol<bool> And(this ISymbol<bool> value, ISymbol<bool> other)
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
    
    public static VariableSymbol<bool> Or(this ISymbol<bool> value, ISymbol<bool> other)
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
    
    public static VariableSymbol<bool> Negate(this ISymbol<bool> value)
    {
        var method = value.Context;
        var result = method.Variable<bool>();
        
        value.EmitLoadAsValue();
        value.Context.Code.Emit(OpCodes.Ldc_I4_0);
        value.Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();
        
        return result;
    }
}