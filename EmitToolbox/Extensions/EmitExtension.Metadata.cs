namespace EmitToolbox.Extensions;

public static class EmitMetadataExtension
{
    public static void EmitTypeInfo(this ILGenerator code, Type type)
    {
        code.Emit(OpCodes.Ldtoken, type);
        code.Emit(OpCodes.Call,
            typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle))!);
    }

    public static void EmitFieldInfo(this ILGenerator code, FieldInfo field)
    {
        code.Emit(OpCodes.Ldtoken, field);
        code.Emit(OpCodes.Call,
            typeof(FieldInfo).GetMethod(nameof(FieldInfo.GetFieldFromHandle), [typeof(RuntimeFieldHandle)])!);
    }

    public static void EmitPropertyInfo(this ILGenerator code, PropertyInfo property)
    {
        code.Emit(OpCodes.Ldtoken, property.DeclaringType!);
        code.Emit(OpCodes.Call, typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle))!);
        code.Emit(OpCodes.Ldstr, property.Name);
        code.Emit(OpCodes.Ldc_I4_S,
            (int)(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
        code.Emit(OpCodes.Call,
            typeof(Type).GetMethod(nameof(Type.GetProperty), [typeof(string), typeof(BindingFlags)])!);
    }

    public static void EmitMethodInfo(this ILGenerator code, MethodInfo method)
    {
        code.Emit(OpCodes.Ldtoken, method);

        if (method.DeclaringType == null)
        {
            code.Emit(OpCodes.Call,
                typeof(MethodBase).GetMethod(nameof(MethodBase.GetMethodFromHandle),
                    [typeof(RuntimeMethodHandle)])!);
            return;
        }

        code.Emit(OpCodes.Ldtoken, method.DeclaringType!);
        code.Emit(OpCodes.Call,
            typeof(MethodBase).GetMethod(nameof(MethodBase.GetMethodFromHandle),
                [typeof(RuntimeMethodHandle), typeof(RuntimeTypeHandle)])!);
    }

    public static void EmitConstructorInfo(this ILGenerator code, ConstructorInfo constructor)
    {
        if (constructor.DeclaringType == null)
            throw new Exception("Cannot emit constructor info for a constructor with no declaring type.");
        
        code.EmitTypeInfo(constructor.DeclaringType);
        code.LoadLiteral(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        var parameters = constructor.GetParameters();

        code.LoadLiteral(parameters.Length);
        code.NewArray(typeof(Type)); 
        
        foreach (var (index, parameter) in parameters.Index())
        {
            code.Duplicate();
            code.LoadLiteral(index);
            code.EmitTypeInfo(parameter.ParameterType);
            code.Emit(OpCodes.Stelem_Ref);
        }
        
        code.Call(typeof(Type).GetMethod(nameof(Type.GetConstructor), 
            [typeof(BindingFlags), typeof(Type[])])!);
    }
}