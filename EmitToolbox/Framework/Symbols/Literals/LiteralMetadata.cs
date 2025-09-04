using EmitToolbox.Extensions;

namespace EmitToolbox.Framework.Symbols.Literals;

public class LiteralTypeInfo(MethodBuildingContext context, Type value) 
    : LiteralValueSymbol<Type>(context, value)
{
    public override void EmitDirectlyLoadValue()
    {
        Context.Code.Emit(OpCodes.Ldtoken, Value);
        Context.Code.Emit(OpCodes.Call,
            typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle))!);
    }
}

public class LiteralFieldInfo(MethodBuildingContext context, FieldInfo value) 
    : LiteralValueSymbol<FieldInfo>(context, value)
{
    public override void EmitDirectlyLoadValue()
    {
        Context.Code.Emit(OpCodes.Ldtoken, Value);
        Context.Code.Emit(OpCodes.Call,
            typeof(FieldInfo).GetMethod(nameof(FieldInfo.GetFieldFromHandle), [typeof(RuntimeFieldHandle)])!);
    }
}

public class LiteralPropertyInfo(MethodBuildingContext context, PropertyInfo value) 
    : LiteralValueSymbol<PropertyInfo>(context, value)
{
    public override void EmitDirectlyLoadValue()
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

public class LiteralMethodInfo(MethodBuildingContext context, MethodInfo value)
    : LiteralValueSymbol<MethodInfo>(context, value)
{
    public override void EmitDirectlyLoadValue()
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

public class LiteralConstructorInfo(MethodBuildingContext context, ConstructorInfo value)
    : LiteralValueSymbol<ConstructorInfo>(context, value)
{
    public override void EmitDirectlyLoadValue()
    {
        if (Value.DeclaringType == null)
            throw new Exception("Cannot emit constructor info for a constructor with no declaring type.");
        
        Context.Code.EmitTypeInfo(Value.DeclaringType);
        Context.Code.LoadLiteral(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        var parameters = Value.GetParameters();

        Context.Code.LoadLiteral(parameters.Length);
        Context.Code.NewArray(typeof(Type)); 
        
        foreach (var (index, parameter) in parameters.Index())
        {
            Context.Code.Duplicate();
            Context.Code.LoadLiteral(index);
            Context.Code.EmitTypeInfo(parameter.ParameterType);
            Context.Code.Emit(OpCodes.Stelem_Ref);
        }
        
        Context.Code.Call(typeof(Type).GetMethod(nameof(Type.GetConstructor), 
            [typeof(BindingFlags), typeof(Type[])])!);
    }
}

public static class LiteralMetadataExtensions
{
    public static LiteralTypeInfo Value(this MethodBuildingContext context, Type type)
        => new(context, type);
    
    public static LiteralFieldInfo Value(this MethodBuildingContext context, FieldInfo field)
        => new(context, field);
    
    public static LiteralPropertyInfo Value(this MethodBuildingContext context, PropertyInfo property)
        => new(context, property);
    
    public static LiteralMethodInfo Value(this MethodBuildingContext context, MethodInfo method)
        => new(context, method);
    
    public static LiteralConstructorInfo Value(this MethodBuildingContext context, ConstructorInfo constructor)
        => new(context, constructor);
}