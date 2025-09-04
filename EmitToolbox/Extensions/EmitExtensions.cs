namespace EmitToolbox.Extensions;

public static class EmitExtensions
{
    public static void Duplicate(this ILGenerator code)
        => code.Emit(OpCodes.Dup);

    /// <summary>
    /// Pop the top of the stack and store it in a local variable, then load the address of that variable.
    /// </summary>
    /// <typeparam name="TType">Type of the variable.</typeparam>
    public static LocalBuilder ToAddress<TType>(this ILGenerator code)
        => ToAddress(code, typeof(TType));

    /// <summary>
    /// Pop the top of the stack and store it in a local variable, then load the address of that variable.
    /// </summary>
    /// <param name="code">Stream to emit IL code to.</param>
    /// <param name="type">Type of the variable.</param>
    public static LocalBuilder ToAddress(this ILGenerator code, Type type)
    {
        var variable = code.DeclareLocal(type);
        code.StoreLocal(variable);
        code.LoadLocalAddress(variable);
        return variable;
    }
    
    public static void LoadLocal(this ILGenerator code, LocalBuilder local)
        => code.Emit(OpCodes.Ldloc, local);

    public static void LoadLocalAddress(this ILGenerator code, LocalBuilder local)
        => code.Emit(OpCodes.Ldloca, local);

    public static void StoreLocal(this ILGenerator code, LocalBuilder local)
        => code.Emit(OpCodes.Stloc, local);

    public static void NewObject(this ILGenerator code, ConstructorInfo constructor)
        => code.Emit(OpCodes.Newobj, constructor);

    public static LocalBuilder NewStruct(this ILGenerator code, ConstructorInfo constructor)
    {
        var variable = code.DeclareLocal(constructor.DeclaringType!);
        code.Emit(OpCodes.Ldloca, variable);
        code.Emit(OpCodes.Call, constructor);
        code.Emit(OpCodes.Ldloc, variable);
        return variable;
    }
    
    public static LocalBuilder NewStruct(this ILGenerator code, Type type)
    {
        var variable = code.DeclareLocal(type);
        code.Emit(OpCodes.Ldloca, variable);
        code.Emit(OpCodes.Initobj, type);
        code.Emit(OpCodes.Ldloc, variable);
        return variable;
    }
    
    public static void NewArray(this ILGenerator code, Type type)
        => code.Emit(OpCodes.Newarr, type);

    public static void MethodReturn(this ILGenerator code)
        => code.Emit(OpCodes.Ret);

    public static void Call(this ILGenerator code, MethodInfo method)
        => code.Emit(OpCodes.Call, method);

    public static void Call(this ILGenerator code, ConstructorInfo constructor)
        => code.Emit(OpCodes.Call, constructor);

    public static void CallVirtual(this ILGenerator code, MethodInfo method)
        => code.Emit(OpCodes.Callvirt, method);
    
    public static void IsInstanceOf(this ILGenerator code, Type type)
        => code.Emit(OpCodes.Isinst, type);
    
    public static void IsInstanceOf<TType>(this ILGenerator code) where TType : class
        => code.Emit(OpCodes.Isinst, typeof(TType));
}