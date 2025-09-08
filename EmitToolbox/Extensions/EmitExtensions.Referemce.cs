namespace EmitToolbox.Extensions;

public static class EmitReferenceExtensions
{
    public static void LoadReference_IntPtr(this ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_I);
    }
    
    public static void LoadReference_Int8(this ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_I1);
    }
    
    public static void LoadReference_UInt8(this ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_U1);
    }
    
    public static void LoadReference_Int16(this ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_I2);
    }
    
    public static void LoadReference_UInt16(this ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_U2);
    }
    
    public static void LoadReference_Int32(this ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_I4);
    }
    
    public static void LoadReference_UInt32(this ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_U4);
    }
    
    public static void LoadReference_Int64(this ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_I8);
    }
    
    public static void LoadReference_Single(this ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_R4);
    }
    
    public static void LoadReference_Double(this ILGenerator code)
    {
        code.Emit(OpCodes.Ldind_R8);
    }
    
    public static void LoadReference(this ILGenerator code, Type type)
    {
        if (type.IsPrimitive)
        {
            if (type == typeof(sbyte))
            {
                LoadReference_Int8(code);
                return;
            }
            if (type == typeof(byte))
            {
                LoadReference_UInt8(code);
                return;
            }
            if (type == typeof(short))
            {
                LoadReference_Int16(code);
                return;
            }
            if (type == typeof(ushort))
            {
                LoadReference_UInt16(code);
                return;
            }
            if (type == typeof(int))
            {
                LoadReference_Int32(code);
                return;
            }
            if (type == typeof(uint))
            {
                LoadReference_UInt32(code);
                return;
            }
            if (type == typeof(long))
            {
                LoadReference_Int64(code);
                return;
            }
            if (type == typeof(float))
            {
                LoadReference_Single(code);
                return;
            }
            if (type == typeof(double))
            {
                LoadReference_Double(code);
                return;
            }
        }
        code.Emit(OpCodes.Ldind_Ref);
    }
    
    public static void LoadReference<TType>(this ILGenerator code)
    {
        LoadReference(code, typeof(TType));
    }
    
    public static void StoreReference_IntPtr(this ILGenerator code)
    {
        code.Emit(OpCodes.Stind_I);
    }
    
    public static void StoreReference_Int8(this ILGenerator code)
    {
        code.Emit(OpCodes.Stind_I1);
    }
    
    public static void StoreReference_Int16(this ILGenerator code)
    {
        code.Emit(OpCodes.Stind_I2);
    }
    
    public static void StoreReference_Int32(this ILGenerator code)
    {
        code.Emit(OpCodes.Stind_I4);
    }
    
    public static void StoreReference_Int64(this ILGenerator code)
    {
        code.Emit(OpCodes.Stind_I8);
    }
    
    public static void StoreReference_Single(this ILGenerator code)
    {
        code.Emit(OpCodes.Stind_R4);
    }
    
    public static void StoreReference_Double(this ILGenerator code)
    {
        code.Emit(OpCodes.Stind_R8);
    }
    
    public static void StoreReference(this ILGenerator code, Type type)
    {
        if (type.IsPrimitive)
        {
            if (type == typeof(sbyte) || type == typeof(byte))
            {
                StoreReference_Int8(code);
                return;
            }
            if (type == typeof(short) || type == typeof(ushort))
            {
                StoreReference_Int16(code);
                return;
            }
            if (type == typeof(int) || type == typeof(uint))
            {
                StoreReference_Int32(code);
                return;
            }
            if (type == typeof(long) || type == typeof(ulong))
            {
                StoreReference_Int64(code);
                return;
            }
            if (type == typeof(float))
            {
                StoreReference_Single(code);
                return;
            }
            if (type == typeof(double))
            {
                StoreReference_Double(code);
                return;
            }
        }
        code.Emit(OpCodes.Stind_Ref);
    }
    
    public static void StoreReference<TType>(this ILGenerator code)
    {
        StoreReference(code, typeof(TType));
    }
}