using System.Diagnostics.CodeAnalysis;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework;

public class DynamicConstructor(DynamicType typeContext, ConstructorBuilder constructorBuilder)
    : DynamicMethod(typeContext, constructorBuilder.GetILGenerator())
{
    public ConstructorBuilder ConstructorBuilder { get; } = constructorBuilder;
    
    [field: MaybeNull]
    public ConstructorInfo BuildingConstructor
    {
        get
        {
            if (field != null)
                return field;
            if (TypeContext.IsBuilt)
            {
                return field = TypeContext.BuildingType.GetConstructor(
                                   BindingFlags.Public | BindingFlags.NonPublic,
                                   ConstructorBuilder.GetParameters()
                                       .Select(parameter => parameter.ParameterType).ToArray())
                               ?? throw new InvalidOperationException("Failed to retrieve the built constructor.");
            }
            return ConstructorBuilder;
        }
    }

    public override bool IsStatic => false;

    [field: MaybeNull]
    public ArgumentSymbol This =>
        field ??= new ArgumentSymbol(this, 0, TypeContext.BuildingType);

    public override void MarkAttribute(CustomAttributeBuilder attributeBuilder)
    {
        ConstructorBuilder.SetCustomAttribute(attributeBuilder);
    }

    public override ArgumentSymbol Argument(int index, Type type)
        => new(this, index + 1, type);

    public override ArgumentSymbol<TValue> Argument<TValue>(int index, ValueModifier modifier = ValueModifier.None)
        => new(this, index + 1, modifier);
}