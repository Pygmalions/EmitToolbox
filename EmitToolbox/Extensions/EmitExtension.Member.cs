namespace EmitToolbox.Extensions;

public static class EmitMemberExtension
{
    public static void LoadThis(this ILGenerator code)
        => code.Emit(OpCodes.Ldarg_0);

    /// <summary>
    /// Load 'this' when 'this' is passed by reference.
    /// This method will deference the pointer if 'this' is not a value type,
    /// otherwise it will load the 'reference of the reference of this'.
    /// </summary>
    public static void LoadByRefThis(this ILGenerator code, Type thisType)
    {
        code.Emit(OpCodes.Ldarg_0);
        if (!thisType.IsValueType)
            code.Emit(OpCodes.Ldind_Ref);
    }

    public static void LoadField(this ILGenerator code, FieldInfo field)
        => code.Emit(OpCodes.Ldfld, field);

    public static void LoadFieldAddress(this ILGenerator code, FieldInfo field)
        => code.Emit(OpCodes.Ldflda, field);

    public static void StoreField(this ILGenerator code, FieldInfo field)
        => code.Emit(OpCodes.Stfld, field);

    public static void LoadProperty(this ILGenerator code, PropertyInfo property)
    {
        var method = property.GetGetMethod()!;
        code.Emit(method.IsVirtual ? OpCodes.Callvirt : OpCodes.Call, method);
    }

    public static void StoreProperty(this ILGenerator code, PropertyInfo property)
    {
        var method = property.GetSetMethod()!;
        code.Emit(method.IsVirtual ? OpCodes.Callvirt : OpCodes.Call, method);
    }

    public static void LoadStaticField(this ILGenerator code, FieldInfo field)
        => code.Emit(OpCodes.Ldsfld, field);

    public static void LoadStaticFieldAddress(this ILGenerator code, FieldInfo field)
        => code.Emit(OpCodes.Ldsflda, field);

    public static void StoreStaticField(this ILGenerator code, FieldInfo field)
        => code.Emit(OpCodes.Stsfld, field);
}