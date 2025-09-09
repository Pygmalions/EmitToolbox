using System.Diagnostics.CodeAnalysis;
using EmitToolbox.Framework.Symbols.Members;

namespace EmitToolbox.Framework;

public abstract class DynamicProperty(DynamicType typeContext, PropertyBuilder propertyBuilder)
{
    public PropertyBuilder PropertyBuilder { get; } = propertyBuilder;

    public DynamicType TypeContext { get; } = typeContext;

    [field: MaybeNull]
    public PropertyInfo BuildingProperty
    {
        get
        {
            if (field != null)
                return field;
            if (TypeContext.IsBuilt)
            {
                return field = TypeContext.BuildingType.GetProperty(PropertyBuilder.Name,
                                   BindingFlags.Public | BindingFlags.NonPublic |
                                   BindingFlags.Instance | BindingFlags.Static)
                               ?? throw new InvalidOperationException("Failed to retrieve the built property.");
            }
            return PropertyBuilder;
        }
    }

    public void MarkAttribute(CustomAttributeBuilder attributeBuilder)
    {
        PropertyBuilder.SetCustomAttribute(attributeBuilder);
    }
}

public class InstanceDynamicProperty(
    DynamicType typeContext,
    PropertyBuilder propertyBuilder,
    VisibilityLevel visibility,
    MethodModifier modifier)
    : DynamicProperty(typeContext, propertyBuilder)
{
    public PropertySymbol SymbolOf(ISymbol instance)
        => new(instance.Context, PropertyBuilder, instance);

    public InstanceDynamicAction? Setter { get; private set; }

    public InstanceDynamicFunctor? Getter { get; private set; }

    public InstanceDynamicAction DefineSetter()
    {
        if (Setter != null)
            throw new InvalidOperationException("Setter is already defined for this property.");

        Setter = TypeContext.ActionBuilder.DefineInstance(
            "set_" + PropertyBuilder.Name,
            [
                new ParameterDefinition(PropertyBuilder.PropertyType, ParameterModifier.None, "value")
            ],
            visibility, modifier, hasSpecialName: true);
        PropertyBuilder.SetSetMethod(Setter.MethodBuilder);

        return Setter;
    }

    public InstanceDynamicFunctor DefineGetter()
    {
        if (Getter != null)
            throw new InvalidOperationException("Getter is already defined for this property.");

        Getter = TypeContext.FunctorBuilder.DefineInstance(
            "get_" + PropertyBuilder.Name, [],
            new ResultDefinition(PropertyBuilder.PropertyType),
            visibility, modifier, hasSpecialName: true);
        PropertyBuilder.SetGetMethod(Getter.MethodBuilder);
        return Getter;
    }
}

public class StaticDynamicProperty(
    DynamicType typeContext,
    PropertyBuilder propertyBuilder,
    VisibilityLevel visibility)
    : DynamicProperty(typeContext, propertyBuilder)
{
    public PropertySymbol Symbol(DynamicMethod context)
        => new(context, PropertyBuilder, null);

    public StaticDynamicAction? Setter { get; private set; }

    public StaticDynamicFunctor? Getter { get; private set; }

    public StaticDynamicAction DefineSetter()
    {
        if (Setter != null)
            throw new InvalidOperationException("Setter is already defined for this property.");

        Setter = TypeContext.ActionBuilder.DefineStatic(
            "set_" + PropertyBuilder.Name,
            [
                new ParameterDefinition(PropertyBuilder.PropertyType, ParameterModifier.None, "value")
            ],
            visibility, hasSpecialName: true);
        PropertyBuilder.SetSetMethod(Setter.MethodBuilder);

        return Setter;
    }

    public StaticDynamicFunctor DefineGetter()
    {
        if (Getter != null)
            throw new InvalidOperationException("Getter is already defined for this property.");

        Getter = TypeContext.FunctorBuilder.DefineStatic(
            "get_" + PropertyBuilder.Name, [],
            new ResultDefinition(PropertyBuilder.PropertyType),
            visibility, hasSpecialName: true);
        PropertyBuilder.SetGetMethod(Getter.MethodBuilder);
        return Getter;
    }
}