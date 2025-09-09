namespace EmitToolbox.Framework.Symbols.Extensions;

public static class NullableSymbolExtensions
{
    public static void AssignNull<TValue>(this IAssignableSymbol<TValue?> symbol) where TValue : struct
    {
        symbol.Context.Code.Emit(OpCodes.Newobj, typeof(TValue?).GetConstructor(Type.EmptyTypes)!);
        symbol.EmitStoreContent();
    }

    public static void AssignNull<TValue>(this IAddressableSymbol<TValue?> symbol) where TValue : struct
    {
        symbol.EmitLoadAddress();
        symbol.Context.Code.Emit(OpCodes.Initobj, typeof(TValue?));
    }

#nullable disable
    public static void AssignNull<TValue>(this IAssignableSymbol<TValue> symbol) where TValue : class
#nullable restore
    {
        symbol.Context.Code.Emit(OpCodes.Ldnull);
        symbol.EmitStoreContent();
    }
    
    public static void AssignNull(this IAssignableSymbol symbol)
    {
        if (symbol.ValueType.IsClass)
        {
            symbol.Context.Code.Emit(OpCodes.Ldnull);
            symbol.EmitStoreContent();
            return;
        }

        if (!symbol.ValueType.IsGenericType || symbol.ValueType.GetGenericTypeDefinition() != typeof(Nullable<>))
            throw new InvalidOperationException("Cannot assign null to non-nullable value type symbols.");
        if (symbol is IAddressableSymbol addressable)
        {
            addressable.EmitLoadAddress();
            addressable.Context.Code.Emit(OpCodes.Initobj, symbol.ValueType);
            return;
        }

        symbol.Context.Code.Emit(OpCodes.Newobj, symbol.ValueType.GetConstructor(Type.EmptyTypes)!);
        symbol.EmitStoreContent();
    }
}