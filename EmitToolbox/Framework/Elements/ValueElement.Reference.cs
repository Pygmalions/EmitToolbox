namespace EmitToolbox.Framework.Elements;

internal static class ValueElementReferenceLoader
{
    public static void EmitLoadClassReference(ILGenerator code, Type type)
    {
        code.Emit(OpCodes.Ldind_Ref, type);
    }

    public static void EmitStoreClassReference(ILGenerator code, Type type)
    {
        code.Emit(OpCodes.Stind_Ref, type);
    }

    public static void EmitLoadStructReference(ILGenerator code, Type type)
    {
        code.Emit(OpCodes.Ldobj, type);
    }

    public static void EmitStoreStructReference(ILGenerator code, Type type)
    {
        code.Emit(OpCodes.Stobj, type);
    }

    public static void EmitLoadNativeIntegerReference(ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_I);
    }

    public static void EmitStoreNativeIntegerReference(ILGenerator code)
    {
        code.Emit(OpCodes.Stind_I);
    }

    public static void EmitLoadInteger8Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_I1);
    }

    public static void EmitStoreInteger8Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Stind_I1);
    }

    public static void EmitLoadIntegerU8Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_U1);
    }

    public static void EmitStoreIntegerU8Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Stind_I1);
    }

    public static void EmitLoadInteger16Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_I2);
    }

    public static void EmitStoreInteger16Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Stind_I2);
    }

    public static void EmitLoadIntegerU16Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_U2);
    }

    public static void EmitStoreIntegerU16Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Stind_I2);
    }

    public static void EmitLoadInteger32Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_I4);
    }

    public static void EmitStoreInteger32Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Stind_I4);
    }

    public static void EmitLoadIntegerU32Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_U4);
    }

    public static void EmitStoreIntegerU32Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Stind_I4);
    }

    public static void EmitLoadInteger64Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_I8);
    }

    public static void EmitStoreInteger64Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Stind_I8);
    }

    public static void EmitLoadIntegerU64Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_I8);
    }

    public static void EmitStoreIntegerU64Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Stind_I8);
    }

    public static void EmitLoadFloatReference(ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_R4);
    }

    public static void EmitStoreFloatReference(ILGenerator code)
    {
        code.Emit(OpCodes.Stind_R4);
    }

    public static void EmitLoadDoubleReference(ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_R8);
    }

    public static void EmitStoreDoubleReference(ILGenerator code)
    {
        code.Emit(OpCodes.Stind_R8);
    }

    public static Action<ILGenerator> SelectReferenceLoader(Type type)
    {
        if (!type.IsValueType)
            return code => EmitLoadClassReference(code, type);

        if (type.IsPrimitive)
        {
            if (type == typeof(sbyte))
                return EmitLoadInteger8Reference;
            if (type == typeof(byte))
                return EmitLoadIntegerU8Reference;
            if (type == typeof(short))
                return EmitLoadInteger16Reference;
            if (type == typeof(ushort))
                return EmitLoadIntegerU16Reference;
            if (type == typeof(int))
                return EmitLoadInteger32Reference;
            if (type == typeof(uint))
                return EmitLoadIntegerU32Reference;
            if (type == typeof(long))
                return EmitLoadInteger64Reference;
            if (type == typeof(ulong))
                return EmitLoadIntegerU64Reference;
            if (type == typeof(float))
                return EmitLoadFloatReference;
            if (type == typeof(double))
                return EmitLoadDoubleReference;
            if (type == typeof(IntPtr) || type == typeof(UIntPtr))
                return EmitLoadNativeIntegerReference;
        }

        return type.IsEnum
            ? SelectReferenceLoader(type.GetEnumUnderlyingType())
            : code => EmitLoadStructReference(code, type);
    }

    public static Action<ILGenerator> SelectReferenceStorer(Type type)
    {
        if (!type.IsValueType)
            return code => EmitStoreClassReference(code, type);

        if (type.IsPrimitive)
        {
            if (type == typeof(sbyte))
                return EmitStoreInteger8Reference;
            if (type == typeof(byte))
                return EmitStoreIntegerU8Reference;
            if (type == typeof(short))
                return EmitStoreInteger16Reference;
            if (type == typeof(ushort))
                return EmitStoreIntegerU16Reference;
            if (type == typeof(int))
                return EmitStoreInteger32Reference;
            if (type == typeof(uint))
                return EmitStoreIntegerU32Reference;
            if (type == typeof(long))
                return EmitStoreInteger64Reference;
            if (type == typeof(ulong))
                return EmitStoreIntegerU64Reference;
            if (type == typeof(float))
                return EmitStoreFloatReference;
            if (type == typeof(double))
                return EmitStoreDoubleReference;
            if (type == typeof(IntPtr) || type == typeof(UIntPtr))
                return EmitStoreNativeIntegerReference;
        }

        return type.IsEnum
            ? SelectReferenceStorer(type.GetEnumUnderlyingType())
            : code => EmitStoreStructReference(code, type);
    }
}