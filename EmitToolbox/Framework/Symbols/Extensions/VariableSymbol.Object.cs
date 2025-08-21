namespace EmitToolbox.Framework.Symbols.Extensions;

public static class VariableSymbolObjectExtensions
{
    public static void Assign(this VariableSymbol<object> target, ValueSymbol value)
    {
        if (!value.ValueType.IsValueType)
            value.EmitLoadAsValue();
        else
        {
            value.EmitLoadAsValue();
            target.Context.Code.Emit(OpCodes.Box, value.ValueType);
        }
        
        target.EmitStoreFromValue();
    }
}