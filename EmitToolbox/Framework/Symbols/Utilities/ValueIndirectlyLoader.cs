namespace EmitToolbox.Framework.Symbols.Utilities;

internal static class ValueIndirectlyLoader
{
    public static void EmitLoadClassReference(ILGenerator code, Type type)
    {
        code.Emit(OpCodes.Ldind_Ref, type);
    }

    public static void EmitLoadStructReference(ILGenerator code, Type type)
    {
        code.Emit(OpCodes.Ldobj, type);
    }

    public static void EmitLoadNativeIntegerReference(ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_I);
    }

    public static void EmitLoadInteger8Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_I1);
    }

    public static void EmitLoadIntegerU8Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_U1);
    }
    
    public static void EmitLoadInteger16Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_I2);
    }

    public static void EmitLoadIntegerU16Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_U2);
    }

    public static void EmitLoadInteger32Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_I4);
    }

    public static void EmitLoadIntegerU32Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_U4);
    }
    
    public static void EmitLoadInteger64Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_I8);
    }
    
    public static void EmitLoadIntegerU64Reference(ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_I8);
    }
    
    public static void EmitLoadFloatReference(ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_R4);
    }
    
    public static void EmitLoadDoubleReference(ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_R8);
    }
    
    public static Action<ILGenerator> GetReferenceLoader(Type type)
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
            ? GetReferenceLoader(type.GetEnumUnderlyingType())
            : code => EmitLoadStructReference(code, type);
    }
}