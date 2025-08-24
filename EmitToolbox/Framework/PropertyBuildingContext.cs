using System.Diagnostics.CodeAnalysis;
using EmitToolbox.Framework.Symbols;
using EmitToolbox.Framework.Symbols.Members;

namespace EmitToolbox.Framework;

public class PropertyBuildingContext<TProperty>(
    TypeBuildingContext typeContext,
    PropertyBuilder propertyBuilder,
    string propertyName,
    Type propertyType,
    bool isReference,
    VisibilityLevel visibility,
    MethodModifier modifier)
{
    [field: MaybeNull]
    public PropertyInfo BuildingProperty
    {
        get
        {
            if (typeContext.IsBuilt)
                field ??= typeContext.BuildingType.GetProperty(propertyBuilder.Name,
                              BindingFlags.Public | BindingFlags.NonPublic |
                              (Modifier != MethodModifier.Static ? BindingFlags.Instance : BindingFlags.Static))
                          ?? throw new InvalidOperationException("Failed to retrieve the built property.");
            return field ?? propertyBuilder;
        }
    }

    public MethodModifier Modifier { get; } = modifier;

    public ActionMethodBuildingContext? Setter { get; private set; }

    public FunctorMethodBuildingContext? Getter { get; private set; }

    public void MarkAttribute(CustomAttributeBuilder attributeBuilder)
    {
        propertyBuilder.SetCustomAttribute(attributeBuilder);
    }

    public ActionMethodBuildingContext DefineSetter()
    {
        if (Setter != null)
            throw new InvalidOperationException("Setter is already defined for this property.");

        Setter = typeContext.DefineAction(
            "set_" + propertyName,
            [
                new ParameterDefinition(propertyType,
                    isReference ? ParameterModifier.Ref : ParameterModifier.None,
                    "value")
            ],
            visibility, Modifier, hasSpecialName: true);
        propertyBuilder.SetSetMethod(Setter.MethodBuilder);

        return Setter;
    }

    public FunctorMethodBuildingContext DefineGetter()
    {
        if (Getter != null)
            throw new InvalidOperationException("Getter is already defined for this property.");

        Getter = typeContext.DefineFunctor(
            "get_" + propertyName, [],
            new ResultDefinition(isReference ? propertyType.MakeByRefType() : propertyType),
            visibility, Modifier, hasSpecialName: true);
        propertyBuilder.SetGetMethod(Getter.MethodBuilder);
        return Getter;
    }

    public PropertySymbol<TProperty> InstanceSymbol(MethodBuildingContext context)
    {
        if (Modifier != MethodModifier.Static && context.IsStatic)
            throw new InvalidOperationException("Cannot access instance property in static method.");

        return new PropertySymbol<TProperty>(
            context, new ThisSymbol(context, propertyBuilder.DeclaringType!),
            propertyBuilder
        );
    }

    public PropertySymbol<TProperty> InstanceSymbol(ValueSymbol instance)
    {
        return new PropertySymbol<TProperty>(instance.Context, instance, propertyBuilder);
    }

    public StaticPropertySymbol<TProperty> StaticSymbol(MethodBuildingContext context)
    {
        if (Modifier != MethodModifier.Static)
            throw new InvalidOperationException("This property is not static.");

        return new StaticPropertySymbol<TProperty>(context, propertyBuilder);
    }
}