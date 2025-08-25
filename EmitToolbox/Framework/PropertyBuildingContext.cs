using System.Diagnostics.CodeAnalysis;
using EmitToolbox.Framework.Symbols;
using EmitToolbox.Framework.Symbols.Members;

namespace EmitToolbox.Framework;

public abstract class PropertyBuildingContext(
    TypeBuildingContext typeContext, PropertyBuilder propertyBuilder)
{
    internal PropertyBuilder PropertyBuilder { get; } = propertyBuilder;
    
    public TypeBuildingContext TypeContext { get; } = typeContext;

    [field: MaybeNull]
    public PropertyInfo BuildingProperty
    {
        get
        {
            if (TypeContext.IsBuilt)
                field ??= TypeContext.BuildingType.GetProperty(PropertyBuilder.Name,
                              BindingFlags.Public | BindingFlags.NonPublic |
                              BindingFlags.Instance | BindingFlags.Static)
                          ?? throw new InvalidOperationException("Failed to retrieve the built property.");
            return field ?? PropertyBuilder;
        }
    }

    public void MarkAttribute(CustomAttributeBuilder attributeBuilder)
    {
        PropertyBuilder.SetCustomAttribute(attributeBuilder);
    }
}

public class InstancePropertyBuildingContext<TProperty>(
    TypeBuildingContext typeContext,
    PropertyBuilder propertyBuilder,
    bool isReference,
    VisibilityLevel visibility,
    MethodModifier modifier)
    : PropertyBuildingContext(typeContext, propertyBuilder)
{
    public PropertySymbol<TProperty> Symbol(ValueSymbol instance)
    {
        if (!instance.ValueType.IsAssignableTo(PropertyBuilder.DeclaringType))
            throw new ArgumentException(
                $"Instance type '{instance.ValueType}' is not assignable to the " +
                $"declaring type '{PropertyBuilder.DeclaringType}' of the property.", nameof(instance));
        return new PropertySymbol<TProperty>(instance.Context, instance, PropertyBuilder);
    }

    public InstanceActionBuildingContext? Setter { get; private set; }

    public InstanceFunctorBuildingContext? Getter { get; private set; }

    public InstanceActionBuildingContext DefineSetter()
    {
        if (Setter != null)
            throw new InvalidOperationException("Setter is already defined for this property.");

        Setter = TypeContext.Actions.Instance(
            "set_" + PropertyBuilder.Name,
            [
                new ParameterDefinition(typeof(TProperty),
                    isReference ? ParameterModifier.Ref : ParameterModifier.None,
                    "value")
            ],
            visibility, modifier, hasSpecialName: true);
        PropertyBuilder.SetSetMethod(Setter.MethodBuilder);

        return Setter;
    }

    public InstanceFunctorBuildingContext DefineGetter()
    {
        if (Getter != null)
            throw new InvalidOperationException("Getter is already defined for this property.");

        Getter = TypeContext.Functors.Instance(
            "get_" + PropertyBuilder.Name, [],
            new ResultDefinition(!isReference ? typeof(TProperty) : typeof(TProperty).MakeByRefType()),
            visibility, modifier, hasSpecialName: true);
        PropertyBuilder.SetGetMethod(Getter.MethodBuilder);
        return Getter;
    }
}

public class StaticPropertyBuildingContext<TProperty>(
    TypeBuildingContext typeContext,
    PropertyBuilder propertyBuilder,
    bool isReference,
    VisibilityLevel visibility)
    : PropertyBuildingContext(typeContext, propertyBuilder)
{
    public StaticPropertySymbol<TProperty> Symbol(MethodBuildingContext context)
        => new(context, PropertyBuilder);

    public StaticActionBuildingContext? Setter { get; private set; }

    public StaticFunctorBuildingContext? Getter { get; private set; }

    public StaticActionBuildingContext DefineSetter()
    {
        if (Setter != null)
            throw new InvalidOperationException("Setter is already defined for this property.");

        Setter = TypeContext.Actions.Static(
            "set_" + PropertyBuilder.Name,
            [
                new ParameterDefinition(typeof(TProperty),
                    isReference ? ParameterModifier.Ref : ParameterModifier.None,
                    "value")
            ],
            visibility, hasSpecialName: true);
        PropertyBuilder.SetSetMethod(Setter.MethodBuilder);

        return Setter;
    }

    public StaticFunctorBuildingContext DefineGetter()
    {
        if (Getter != null)
            throw new InvalidOperationException("Getter is already defined for this property.");

        Getter = TypeContext.Functors.Static(
            "get_" + PropertyBuilder.Name, [],
            new ResultDefinition(!isReference ? typeof(TProperty) : typeof(TProperty).MakeByRefType()),
            visibility, hasSpecialName: true);
        PropertyBuilder.SetGetMethod(Getter.MethodBuilder);
        return Getter;
    }
}