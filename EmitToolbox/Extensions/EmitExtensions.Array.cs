namespace EmitToolbox.Extensions;

public static class EmitArrayExtensions
{
    public static void LoadArrayElement(this ILGenerator code, Type elementType)
    {
        if (elementType.IsValueType)
            code.Emit(OpCodes.Ldelem, elementType);
        else
            code.Emit(OpCodes.Ldelem_Ref);
    }

    public static void LoadArrayElement<TElement>(this ILGenerator code)
        => code.LoadArrayElement(typeof(TElement));
    
    public static void LoadArrayElementAddress(this ILGenerator code, Type elementType)
    {
        if (elementType.IsValueType)
            throw new InvalidOperationException("Cannot load address of value type array element.");
        code.Emit(OpCodes.Ldelema, elementType);
    }
    
    public static void LoadArrayElementAddress<TElement>(this ILGenerator code) where TElement : class
        => code.LoadArrayElementAddress(typeof(TElement));
    
    public static void StoreArrayElement(this ILGenerator code, Type elementType)
    {
        if (elementType.IsValueType)
            code.Emit(OpCodes.Stelem, elementType);
        else
            code.Emit(OpCodes.Stelem_Ref);
    }
    
    public static void StoreArrayElement<TElement>(this ILGenerator code)
        => code.StoreArrayElement(typeof(TElement));
}