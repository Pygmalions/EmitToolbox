using System.Diagnostics.CodeAnalysis;

namespace EmitToolbox.Framework;

public class ConstructorMethodBuildingContext(TypeBuildingContext typeContext, ConstructorBuilder constructorBuilder)
    : MethodBuildingContext(typeContext, constructorBuilder.GetILGenerator())
{
    [field: MaybeNull]
    public ConstructorInfo BuildingConstructor
    {
        get
        {
            if (TypeContext.IsBuilt)
                field ??= TypeContext.BuildingType.GetConstructor(
                    constructorBuilder.GetParameters()
                        .Select(parameter => parameter.ParameterType).ToArray())!;
            return field ?? constructorBuilder;
        }
    }
    
    public override void MarkAttribute(CustomAttributeBuilder attributeBuilder)
    {
        constructorBuilder.SetCustomAttribute(attributeBuilder);
    }
}