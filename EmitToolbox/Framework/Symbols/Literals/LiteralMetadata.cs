using EmitToolbox.Extensions;

namespace EmitToolbox.Framework.Symbols.Literals;

public class LiteralTypeInfo(DynamicMethod context, Type value) : LiteralSymbol<Type>(context, value)
{
    public override void EmitLoadContent()
    {
        Context.Code.Emit(OpCodes.Ldtoken, Value);
        Context.Code.Emit(OpCodes.Call,
            typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle))!);
    }
}

public class LiteralFieldInfo(DynamicMethod context, FieldInfo value) : LiteralSymbol<FieldInfo>(context, value)
{
    public override void EmitLoadContent()
    {
        Context.Code.Emit(OpCodes.Ldtoken, Value);
        Context.Code.Emit(OpCodes.Call,
            typeof(FieldInfo).GetMethod(nameof(FieldInfo.GetFieldFromHandle), [typeof(RuntimeFieldHandle)])!);
    }
}

public class LiteralPropertyInfo(DynamicMethod context, PropertyInfo value) : LiteralSymbol<PropertyInfo>(context, value)
{
    public override void EmitLoadContent()
    {
        Context.Code.Emit(OpCodes.Ldtoken, Value.DeclaringType!);
        Context.Code.Emit(OpCodes.Call, typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle))!);
        Context.Code.Emit(OpCodes.Ldstr, Value.Name);
        Context.Code.Emit(OpCodes.Ldc_I4_S,
            (int)(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
        Context.Code.Emit(OpCodes.Call,
            typeof(Type).GetMethod(nameof(Type.GetProperty), [typeof(string), typeof(BindingFlags)])!);
    }
}

public class LiteralMethodInfo(DynamicMethod context, MethodInfo value) : LiteralSymbol<MethodInfo>(context, value)
{
    public override void EmitLoadContent()
    {
        Context.Code.Emit(OpCodes.Ldtoken, Value);

        if (Value.DeclaringType == null)
        {
            Context.Code.Emit(OpCodes.Call,
                typeof(MethodBase).GetMethod(nameof(MethodBase.GetMethodFromHandle),
                    [typeof(RuntimeMethodHandle)])!);
            return;
        }

        Context.Code.Emit(OpCodes.Ldtoken, Value.DeclaringType!);
        Context.Code.Emit(OpCodes.Call,
            typeof(MethodBase).GetMethod(nameof(MethodBase.GetMethodFromHandle),
                [typeof(RuntimeMethodHandle), typeof(RuntimeTypeHandle)])!);
    }
}

public class LiteralConstructorInfo(DynamicMethod context, ConstructorInfo value) : LiteralSymbol<ConstructorInfo>(context, value)
{
    public override void EmitLoadContent()
    {
        if (Value.DeclaringType == null)
            throw new Exception("Cannot emit constructor info for a constructor with no declaring type.");
        
        Context.Code.LoadTypeInfo(Value.DeclaringType);
        Context.Code.LoadLiteral(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        var parameters = Value.GetParameters();

        Context.Code.LoadLiteral(parameters.Length);
        Context.Code.NewArray(typeof(Type)); 
        
        foreach (var (index, parameter) in parameters.Index())
        {
            Context.Code.Duplicate();
            Context.Code.LoadLiteral(index);
            Context.Code.LoadTypeInfo(parameter.ParameterType);
            Context.Code.Emit(OpCodes.Stelem_Ref);
        }
        
        Context.Code.Call(typeof(Type).GetMethod(nameof(Type.GetConstructor), 
            [typeof(BindingFlags), typeof(Type[])])!);
    }
}

public static class LiteralMetadataExtensions
{
    public static LiteralTypeInfo Value(this DynamicMethod context, Type type)
        => new(context, type);
    
    public static LiteralFieldInfo Value(this DynamicMethod context, FieldInfo field)
        => new(context, field);
    
    public static LiteralPropertyInfo Value(this DynamicMethod context, PropertyInfo property)
        => new(context, property);
    
    public static LiteralMethodInfo Value(this DynamicMethod context, MethodInfo method)
        => new(context, method);
    
    public static LiteralConstructorInfo Value(this DynamicMethod context, ConstructorInfo constructor)
        => new(context, constructor);
}