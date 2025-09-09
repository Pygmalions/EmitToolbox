namespace EmitToolbox.Framework.Symbols.Extensions;

public static class AssignableSymbolObjectExtensions
{
    public static void Assign(this IAssignableSymbol<object> target, ISymbol value)
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