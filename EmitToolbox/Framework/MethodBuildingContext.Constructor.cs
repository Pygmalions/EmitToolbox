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
                              BindingFlags.Public | BindingFlags.NonPublic,
                              constructorBuilder.GetParameters()
                                  .Select(parameter => parameter.ParameterType).ToArray())
                          ?? throw new InvalidOperationException("Failed to retrieve the built constructor.");
            return field ?? constructorBuilder;
        }
    }

    public override bool IsStatic { get; } = false;

    public override void MarkAttribute(CustomAttributeBuilder attributeBuilder)
    {
        constructorBuilder.SetCustomAttribute(attributeBuilder);
    }
}