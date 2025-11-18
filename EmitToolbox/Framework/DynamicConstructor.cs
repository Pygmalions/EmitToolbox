namespace EmitToolbox.Framework;

public class DynamicConstructor(ConstructorBuilder builder) : DynamicFunction(builder)
{
    public ConstructorBuilder Builder { get; } = builder;
    
    public new ConstructorInfo BuildingMethod => (ConstructorInfo)base.BuildingMethod;

    public void Return()
    {
        Code.Emit(OpCodes.Ret);
    }
    
    public override DynamicFunction MarkAttribute(CustomAttributeBuilder attributeBuilder)
    {
        Builder.SetCustomAttribute(attributeBuilder);
        return this;
    }

    protected override MethodBase SearchBuiltMethod(Type type)
    {
        var flags = (Builder.IsStatic ? BindingFlags.Static : BindingFlags.Instance) |
                    (Builder.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic);
        return type.GetConstructor(flags,
            Builder.GetParameters().Select(parameter => parameter.ParameterType).ToArray())!;
    }
}