namespace EmitToolbox.Framework;

public abstract class DynamicMethod<TBuilder, TMetadata>(
    TBuilder builder, 
    Func<Type, MethodBase> delegateSearchMethod, 
    Action<CustomAttributeBuilder> delegateMarkAttribute) : DynamicMethod(builder)
    where TBuilder : TMetadata
    where TMetadata : MethodBase
{
    public TBuilder Builder { get; } = builder;

    public new TMetadata BuildingMethod => (TMetadata)base.BuildingMethod;

    protected override MethodBase SearchBuiltMethod(Type type)
        => delegateSearchMethod(type);

    public override DynamicMethod MarkAttribute(CustomAttributeBuilder attributeBuilder)
    {
        delegateMarkAttribute(attributeBuilder);
        return this;
    }
}

public class DynamicMethod<TMethodBuilder, TMethodMetadata, TReturnDelegate>
    : DynamicMethod<TMethodBuilder, TMethodMetadata>
    where TReturnDelegate : Delegate
    where TMethodBuilder : TMethodMetadata
    where TMethodMetadata : MethodBase
{
    public TReturnDelegate Return { get; }

    internal DynamicMethod(
        TMethodBuilder builder,
        Func<Type, MethodBase> delegateSearchMethod,
        Action<CustomAttributeBuilder> delegateMarkAttribute,
        TReturnDelegate delegateReturnResult)
        : base(builder, delegateSearchMethod, delegateMarkAttribute)
    {
        Return = delegateReturnResult;
    }
}