namespace EmitToolbox.Extensions;

public static class EmitExtensions
{
    /// <param name="code">Stream to emit IL code to.</param>
    extension(ILGenerator code)
    {
        public void Duplicate()
            => code.Emit(OpCodes.Dup);

        /// <summary>
        /// Pop the top of the stack and store it in a local variable, then load the address of that variable.
        /// </summary>
        /// <typeparam name="TType">Type of the variable.</typeparam>
        public LocalBuilder ToAddress<TType>()
            => ToAddress(code, typeof(TType));

        /// <summary>
        /// Pop the top of the stack and store it in a local variable, then load the address of that variable.
        /// </summary>
        /// <param name="type">Type of the variable.</param>
        public LocalBuilder ToAddress(Type type)
        {
            var variable = code.DeclareLocal(type);
            code.StoreLocal(variable);
            code.LoadLocalAddress(variable);
            return variable;
        }

        public void LoadLocal(LocalBuilder local)
            => code.Emit(OpCodes.Ldloc, local);

        public void LoadLocalAddress(LocalBuilder local)
            => code.Emit(OpCodes.Ldloca, local);

        public void StoreLocal(LocalBuilder local)
            => code.Emit(OpCodes.Stloc, local);

        public void NewObject(ConstructorInfo constructor)
            => code.Emit(OpCodes.Newobj, constructor);

        public LocalBuilder NewStruct(ConstructorInfo constructor)
        {
            var variable = code.DeclareLocal(constructor.DeclaringType!);
            code.Emit(OpCodes.Ldloca, variable);
            code.Emit(OpCodes.Call, constructor);
            code.Emit(OpCodes.Ldloc, variable);
            return variable;
        }

        public LocalBuilder NewStruct(Type type)
        {
            var variable = code.DeclareLocal(type);
            code.Emit(OpCodes.Ldloca, variable);
            code.Emit(OpCodes.Initobj, type);
            code.Emit(OpCodes.Ldloc, variable);
            return variable;
        }

        public void NewArray(Type type)
            => code.Emit(OpCodes.Newarr, type);

        public void MethodReturn()
            => code.Emit(OpCodes.Ret);

        public void Call(MethodInfo method)
            => code.Emit(OpCodes.Call, method);

        public void Call(ConstructorInfo constructor)
            => code.Emit(OpCodes.Call, constructor);

        public void CallVirtual(MethodInfo method)
            => code.Emit(OpCodes.Callvirt, method);

        public void IsInstanceOf(Type type)
            => code.Emit(OpCodes.Isinst, type);

        public void IsInstanceOf<TType>() where TType : class
            => code.Emit(OpCodes.Isinst, typeof(TType));
    }
}