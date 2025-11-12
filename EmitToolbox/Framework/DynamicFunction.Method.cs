namespace EmitToolbox.Framework;

public abstract class DynamicMethod(MethodBuilder builder) : DynamicFunction(builder) 
{
    public MethodBuilder Builder { get; } = builder;
    
    public new MethodInfo BuildingMethod => (MethodInfo)base.BuildingMethod;
    
    public override DynamicFunction MarkAttribute(CustomAttributeBuilder attributeBuilder)
    {
        Builder.SetCustomAttribute(attributeBuilder);
        return this;
    }

    protected override MethodBase SearchBuiltMethod(Type type)
    {
        var flags = Builder.IsStatic ? BindingFlags.Static : BindingFlags.Instance;
        if (Builder.IsPublic)
            flags |= BindingFlags.Public;
        else
            flags |= BindingFlags.NonPublic;
        return type.GetMethod(Builder.Name, flags,
            Builder.GetParameters().Select(parameter => parameter.ParameterType).ToArray())!;
    }
}

public class DynamicMethod<TReturnDelegate>(MethodBuilder builder, TReturnDelegate delegateReturn) 
    : DynamicMethod(builder) where TReturnDelegate : Delegate
{
    public TReturnDelegate Return { get; } = delegateReturn;
}
