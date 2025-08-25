using System.Diagnostics.CodeAnalysis;
using EmitToolbox.Framework.Symbols;

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

    public override bool IsStatic => false;

    [field: MaybeNull]
    public ThisSymbol This =>
        field ??= new ThisSymbol(this, TypeContext.BuildingType);

    public override void MarkAttribute(CustomAttributeBuilder attributeBuilder)
    {
        constructorBuilder.SetCustomAttribute(attributeBuilder);
    }

    public override ArgumentSymbol<TValue> Argument<TValue>(int index, bool isReference = false)
        => new(this, index + 1, isReference);
}