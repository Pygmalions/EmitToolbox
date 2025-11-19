using EmitToolbox.Extensions;
using EmitToolbox.Utilities;

namespace EmitToolbox.Symbols.Literals;

public readonly struct LiteralTypeInfoSymbol(DynamicFunction context, Type value) : ILiteralSymbol<Type>
{
    public DynamicFunction Context => context;

    public Type Value => value;

    public void LoadContent()
    {
        Context.Code.Emit(OpCodes.Ldtoken, Value);
        Context.Code.Emit(OpCodes.Call,
            typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle))!);
    }
}

public readonly struct LiteralFieldInfoSymbol(DynamicFunction context, FieldInfo value) : ILiteralSymbol<FieldInfo>
{
    public DynamicFunction Context => context;

    public FieldInfo Value => value;

    public void LoadContent()
    {
        Context.Code.Emit(OpCodes.Ldtoken, Value);
        Context.Code.Emit(OpCodes.Call,
            typeof(FieldInfo).GetMethod(nameof(FieldInfo.GetFieldFromHandle), [typeof(RuntimeFieldHandle)])!);
    }
}

public readonly struct LiteralPropertyInfoSymbol(DynamicFunction context, PropertyInfo value)
    : ILiteralSymbol<PropertyInfo>
{
    public DynamicFunction Context => context;

    public PropertyInfo Value => value;

    public void LoadContent()
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

public readonly struct LiteralMethodInfoSymbol(DynamicFunction context, MethodInfo value) : ILiteralSymbol<MethodInfo>
{
    public DynamicFunction Context => context;

    public MethodInfo Value => value;

    public void LoadContent()
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

public readonly struct LiteralConstructorInfoSymbol(DynamicFunction context, ConstructorInfo value)
    : ILiteralSymbol<ConstructorInfo>
{
    public DynamicFunction Context => context;

    public ConstructorInfo Value => value;

    public void LoadContent()
    {
        if (Value.DeclaringType == null)
            throw new Exception("Cannot emit constructor info for a constructor with no declaring type.");

        var symbolType = Context.Value(Value.DeclaringType);
        var symbolFlags = Context.Value(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        var parameters = Value.GetParameters();

        var symbolParameters = Context.NewArray<Type>(Context.Value(parameters.Length));
        foreach (var (index, parameter) in parameters.Index())
        {
            symbolParameters
                .ElementAt(Context.Value(index))
                .AssignContent(Context.Value(parameter.ParameterType));
        }

        symbolType.Invoke(target => target.GetConstructor(Any<BindingFlags>.Value, Any<Type[]>.Value),
                [symbolFlags, symbolParameters])
            .LoadContent();
    }
}