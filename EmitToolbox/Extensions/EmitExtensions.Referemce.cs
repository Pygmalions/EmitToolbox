namespace EmitToolbox.Extensions;

public static class EmitReferenceExtensions
{
    extension(ILGenerator code)
    {
        public void LoadReference_IntPtr()
        {
            code.Emit(OpCodes.Ldind_I);
        }

        public void LoadReference_Int8()
        {
            code.Emit(OpCodes.Ldind_I1);
        }

        public void LoadReference_UInt8()
        {
            code.Emit(OpCodes.Ldind_U1);
        }

        public void LoadReference_Int16()
        {
            code.Emit(OpCodes.Ldind_I2);
        }

        public void LoadReference_UInt16()
        {
            code.Emit(OpCodes.Ldind_U2);
        }

        public void LoadReference_Int32()
        {
            code.Emit(OpCodes.Ldind_I4);
        }

        public void LoadReference_UInt32()
        {
            code.Emit(OpCodes.Ldind_U4);
        }

        public void LoadReference_Int64()
        {
            code.Emit(OpCodes.Ldind_I8);
        }

        public void LoadReference_Single()
        {
            code.Emit(OpCodes.Ldind_R4);
        }

        public void LoadReference_Double()
        {
            code.Emit(OpCodes.Ldind_R8);
        }

        public void LoadReference_Class(Type type)
        {
            if (!type.IsClass)
                throw new Exception("Instruction 'ldind.ref' can only be used to load reference of class types.");
            code.Emit(OpCodes.Ldind_Ref);
        }

        public void LoadReference_Class<TType>() where TType : class
        {
            LoadReference_Class(code, typeof(TType));
        }

        public void LoadReference_Struct(Type type)
        {
            if (!type.IsValueType)
                throw new Exception("Instruction 'ldobj' can only be used to load reference of value types.");
            code.Emit(OpCodes.Ldobj, type);
        }

        public void LoadReference_Struct<TType>() where TType : struct
        {
            LoadReference_Struct(code, typeof(TType));
        }

        public void LoadReference(Type type)
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
            if (type.IsValueType)
            {
                LoadReference_Struct(code, type);
                return;
            }
            LoadReference_Class(code, type);
        }

        public void LoadReference<TType>()
        {
            LoadReference(code, typeof(TType));
        }

        public void StoreReference_IntPtr()
        {
            code.Emit(OpCodes.Stind_I);
        }

        public void StoreReference_Int8()
        {
            code.Emit(OpCodes.Stind_I1);
        }

        public void StoreReference_Int16()
        {
            code.Emit(OpCodes.Stind_I2);
        }

        public void StoreReference_Int32()
        {
            code.Emit(OpCodes.Stind_I4);
        }

        public void StoreReference_Int64()
        {
            code.Emit(OpCodes.Stind_I8);
        }

        public void StoreReference_Single()
        {
            code.Emit(OpCodes.Stind_R4);
        }

        public void StoreReference_Double()
        {
            code.Emit(OpCodes.Stind_R8);
        }

        public void StoreReference_Class(Type type)
        {
            if (!type.IsClass)
                throw new Exception("Instruction 'stind.ref' can only be used to store reference of class types.");
            code.Emit(OpCodes.Stind_Ref);
        }

        public void StoreReference_Class<TType>() where TType : class
        {
            StoreReference_Class(code, typeof(TType));
        }

        public void StoreReference_Struct(Type type)
        {
            if (!type.IsClass)
                throw new Exception("Instruction 'stobj' can only be used to store reference of value types.");
            code.Emit(OpCodes.Stobj, type);
        }

        public void StoreReference_Struct<TType>() where TType : struct
        {
            StoreReference_Struct(code, typeof(TType));
        }

        public void StoreReference(Type type)
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
            if (type.IsValueType)
            {
                StoreReference_Struct(code, type);
                return;
            }
            StoreReference_Class(code, type);
        }

        public void StoreReference<TType>()
        {
            StoreReference(code, typeof(TType));
        }

        public void CopyValueObject(Type type)
        {
            if (!type.IsValueType)
                throw new Exception("Instruction 'cpobj' can only be used to copy value types.");
            code.Emit(OpCodes.Cpobj, type);
        }

        public void CopyValueObject<TType>() where TType : struct
        {
            CopyValueObject(code, typeof(TType));
        }
    }
}