namespace EmitToolbox.Extensions;

public static class EmitArrayExtensions
{
    extension(ILGenerator code)
    {
        public void LoadArrayElement(Type elementType)
        {
            if (elementType.IsValueType)
                code.Emit(OpCodes.Ldelem, elementType);
            else
                code.Emit(OpCodes.Ldelem_Ref);
        }

        public void LoadArrayElement_Struct(Type elementType)
        {
            if (!elementType.IsValueType)
                throw new InvalidOperationException("Element type must be a value type.");
            code.Emit(OpCodes.Ldelem, elementType);
        }

        public void LoadArrayElement_Struct<TElement>() where TElement : struct
            => code.LoadArrayElement_Struct(typeof(TElement));

        public void LoadArrayElement_Class()
        {
            code.Emit(OpCodes.Ldelem_Ref);
        }

        public void LoadArrayElement<TElement>()
            => code.LoadArrayElement(typeof(TElement));

        public void LoadArrayElementAddress(Type elementType)
        {
            if (elementType.IsValueType)
                throw new InvalidOperationException("Cannot load address of value type array element.");
            code.Emit(OpCodes.Ldelema, elementType);
        }

        public void LoadArrayElementAddress<TElement>() where TElement : class
            => code.LoadArrayElementAddress(typeof(TElement));

        public void StoreArrayElement(Type elementType)
        {
            if (elementType.IsValueType)
                code.Emit(OpCodes.Stelem, elementType);
            else
                code.Emit(OpCodes.Stelem_Ref);
        }

        public void StoreArrayElement_Struct(Type elementType)
        {
            if (!elementType.IsValueType)
                throw new InvalidOperationException("Element type must be a value type.");
            code.Emit(OpCodes.Stelem, elementType);
        }

        public void StoreArrayElement_Struct<TElement>() where TElement : struct
            => code.StoreArrayElement_Struct(typeof(TElement));

        public void StoreArrayElement_Class()
            => code.Emit(OpCodes.Stelem_Ref);

        public void StoreArrayElement<TElement>()
            => code.StoreArrayElement(typeof(TElement));
    }
}