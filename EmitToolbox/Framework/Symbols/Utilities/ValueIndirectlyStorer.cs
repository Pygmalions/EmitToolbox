namespace EmitToolbox.Framework.Symbols.Utilities;

internal static class ValueIndirectlyStorer
{
    public static void EmitStoreClassReference(ILGenerator code, Type type)
    {
        code.Emit(OpCodes.Stind_Ref, type);
    }

    public static void EmitStoreStructReference(ILGenerator code, Type type)
    {
        code.Emit(OpCodes.Stobj, type);
    }

    public static void EmitStoreNativeIntegerReference(ILGenerator code)
    {
        code.Emit(OpCodes.Stind_I);
    }

    public static void EmitStoreInteger8Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Stind_I1);
    }

    public static void EmitStoreIntegerU8Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Stind_I1);
    }

    public static void EmitStoreInteger16Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Stind_I2);
    }

    public static void EmitStoreIntegerU16Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Stind_I2);
    }

    public static void EmitStoreInteger32Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Stind_I4);
    }

    public static void EmitStoreIntegerU32Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Stind_I4);
    }

    public static void EmitStoreInteger64Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Stind_I8);
    }

    public static void EmitStoreIntegerU64Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Stind_I8);
    }

    public static void EmitStoreFloatReference(ILGenerator code)
    {
        code.Emit(OpCodes.Stind_R4);
    }

    public static void EmitStoreDoubleReference(ILGenerator code)
    {
        code.Emit(OpCodes.Stind_R8);
    }

    public static Action<ILGenerator> GetReferenceStorer(Type type)
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
            ? GetReferenceStorer(type.GetEnumUnderlyingType())
            : code => EmitStoreStructReference(code, type);
    }
}