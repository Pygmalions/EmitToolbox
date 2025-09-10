namespace EmitToolbox.Framework.Symbols.Extensions;

public static class AssignableSymbolObjectExtensions
{
    public static void Assign(this IAssignableSymbol<object> target, ISymbol value)
    {
        if (!value.ContentType.IsValueType)
            value.EmitLoadAsValue();
        else
        {
            value.EmitLoadAsValue();
            target.Context.Code.Emit(OpCodes.Box, value.ContentType);
        }
        
        target.EmitStoreFromValue();
    }
}