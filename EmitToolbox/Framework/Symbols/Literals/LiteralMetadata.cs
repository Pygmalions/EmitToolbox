using EmitToolbox.Extensions;
using EmitToolbox.Framework.Utilities;

namespace EmitToolbox.Framework.Symbols.Literals;

public readonly struct LiteralTypeInfoSymbol(DynamicMethod context, Type value) : ILiteralSymbol<Type>
{
    public DynamicMethod Context => context;
    
    public Type Value => value;
    
    public void EmitContent()
    {
        Context.Code.Emit(OpCodes.Ldtoken, Value);
        Context.Code.Emit(OpCodes.Call,
            typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle))!);
    }
}

public readonly struct LiteralFieldInfoSymbol(DynamicMethod context, FieldInfo value) : ILiteralSymbol<FieldInfo>
{
    public DynamicMethod Context => context;
    
    public FieldInfo Value => value;
    
    public void EmitContent()
    {
        Context.Code.Emit(OpCodes.Ldtoken, Value);
        Context.Code.Emit(OpCodes.Call,
            typeof(FieldInfo).GetMethod(nameof(FieldInfo.GetFieldFromHandle), [typeof(RuntimeFieldHandle)])!);
    }
}

public readonly struct LiteralPropertyInfoSymbol(DynamicMethod context, PropertyInfo value) : ILiteralSymbol<PropertyInfo>
{
    public DynamicMethod Context => context;
    
    public PropertyInfo Value => value;
    
    public void EmitContent()
    {
        Context.Code.Emit(OpCodes.Ldtoken, Value.DeclaringType!);
        Context.Code.Emit(OpCodes.Call, typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle))!);
        Context.Code.Emit(OpCodes.Ldstr, Value.Name);
        Context.Code.Emit(OpCodes.Ldc_I4_S,
            (int)(BindingFlags.Public | BindingFlags.NonPublic |
                  (value.IsStatic ? BindingFlags.Static : BindingFlags.Instance)));
        Context.Code.Emit(OpCodes.Call,
            typeof(Type).GetMethod(nameof(Type.GetProperty), [typeof(string), typeof(BindingFlags)])!);
    }
}

public readonly struct LiteralMethodInfoSymbol(DynamicMethod context, MethodInfo value) : ILiteralSymbol<MethodInfo>
{
    public DynamicMethod Context => context;
    
    public MethodInfo Value => value;
    
    public void EmitContent()
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

public readonly struct LiteralConstructorInfoSymbol(DynamicMethod context, ConstructorInfo value) : ILiteralSymbol<ConstructorInfo>
{
    public DynamicMethod Context => context;
    
    public ConstructorInfo Value => value;
    
    public void EmitContent()
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