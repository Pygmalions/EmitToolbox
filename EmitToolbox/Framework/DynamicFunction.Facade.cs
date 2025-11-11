namespace EmitToolbox.Framework;

public abstract class DynamicFunction<TBuilder, TMetadata>(
    TBuilder builder, 
    Func<Type, MethodBase> delegateSearchMethod, 
    Action<CustomAttributeBuilder> delegateMarkAttribute) : DynamicFunction(builder)
    where TBuilder : TMetadata
    where TMetadata : MethodBase
{
    public TBuilder Builder { get; } = builder;

    public new TMetadata BuildingMethod => (TMetadata)base.BuildingMethod;

    protected override MethodBase SearchBuiltMethod(Type type)
        => delegateSearchMethod(type);

    public override DynamicFunction MarkAttribute(CustomAttributeBuilder attributeBuilder)
    {
        delegateMarkAttribute(attributeBuilder);
        return this;
    }
}

public class DynamicFunction<TMethodBuilder, TMethodMetadata, TReturnDelegate>
    : DynamicFunction<TMethodBuilder, TMethodMetadata>
    where TReturnDelegate : Delegate
    where TMethodBuilder : TMethodMetadata
    where TMethodMetadata : MethodBase
{
    public TReturnDelegate Return { get; }

    internal DynamicFunction(
        TMethodBuilder builder,
        Func<Type, MethodBase> delegateSearchMethod,
        Action<CustomAttributeBuilder> delegateMarkAttribute,
        TReturnDelegate delegateReturnResult)
        : base(builder, delegateSearchMethod, delegateMarkAttribute)
    {
        Return = delegateReturnResult;
    }
}