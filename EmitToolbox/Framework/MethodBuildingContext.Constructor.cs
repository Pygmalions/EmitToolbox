namespace EmitToolbox.Framework;

public class ConstructorMethodBuildingContext(ConstructorBuilder constructorBuilder)
    : MethodBuildingContext(constructorBuilder.GetILGenerator())
{
    public ConstructorInfo BuildingConstructor { get; } = constructorBuilder;
    
    public override void MarkAttribute(CustomAttributeBuilder attributeBuilder)
    {
        constructorBuilder.SetCustomAttribute(attributeBuilder);
    }
}