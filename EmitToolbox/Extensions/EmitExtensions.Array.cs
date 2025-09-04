namespace EmitToolbox.Extensions;

public static class EmitArrayExtensions
{
    public static void EmitLoadArrayElement(this ILGenerator code, Type elementType)
    {
        if (elementType.IsValueType)
            code.Emit(OpCodes.Ldelem, elementType);
        else
            code.Emit(OpCodes.Ldelem_Ref);
    }

    public static void EmitLoadArrayElement<TElement>(this ILGenerator code)
        => code.EmitLoadArrayElement(typeof(TElement));
    
    public static void EmitLoadArrayElementAddress(this ILGenerator code, Type elementType)
    {
        if (elementType.IsValueType)
            throw new InvalidOperationException("Cannot load address of value type array element.");
        code.Emit(OpCodes.Ldelema, elementType);
    }
    
    public static void EmitLoadArrayElementAddress<TElement>(this ILGenerator code) where TElement : class
        => code.EmitLoadArrayElementAddress(typeof(TElement));
    
    public static void EmitStoreArrayElement(this ILGenerator code, Type elementType)
    {
        if (elementType.IsValueType)
            code.Emit(OpCodes.Stelem, elementType);
        else
            code.Emit(OpCodes.Stelem_Ref);
    }
    
    public static void EmitStoreArrayElement<TElement>(this ILGenerator code)
        => code.EmitStoreArrayElement(typeof(TElement));
}