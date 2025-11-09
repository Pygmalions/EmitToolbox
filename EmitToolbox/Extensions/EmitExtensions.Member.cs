namespace EmitToolbox.Extensions;

public static class EmitMemberExtensions
{
    extension(ILGenerator code)
    {
        public void LoadThis()
            => code.Emit(OpCodes.Ldarg_0);

        /// <summary>
        /// Load 'this' when 'this' is passed by reference.
        /// This method will deference the pointer if 'this' is not a value type,
        /// otherwise it will load the 'reference of the reference of this'.
        /// </summary>
        public void LoadByRefThis(Type thisType)
        {
            code.Emit(OpCodes.Ldarg_0);
            if (!thisType.IsValueType)
                code.Emit(OpCodes.Ldind_Ref);
        }

        public void LoadField(FieldInfo field)
            => code.Emit(OpCodes.Ldfld, field);

        public void LoadFieldAddress(FieldInfo field)
            => code.Emit(OpCodes.Ldflda, field);

        public void StoreField(FieldInfo field)
            => code.Emit(OpCodes.Stfld, field);

        public void LoadProperty(PropertyInfo property)
        {
            var method = property.GetGetMethod()!;
            code.Emit(method.IsVirtual ? OpCodes.Callvirt : OpCodes.Call, method);
        }

        public void StoreProperty(PropertyInfo property)
        {
            var method = property.GetSetMethod()!;
            code.Emit(method.IsVirtual ? OpCodes.Callvirt : OpCodes.Call, method);
        }

        public void LoadStaticField(FieldInfo field)
            => code.Emit(OpCodes.Ldsfld, field);

        public void LoadStaticFieldAddress(FieldInfo field)
            => code.Emit(OpCodes.Ldsflda, field);

        public void StoreStaticField(FieldInfo field)
            => code.Emit(OpCodes.Stsfld, field);
    }
}