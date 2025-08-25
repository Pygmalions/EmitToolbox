using System.Diagnostics.CodeAnalysis;
using EmitToolbox.Framework.Symbols;
using EmitToolbox.Framework.Symbols.Members;

namespace EmitToolbox.Framework;

public abstract class FieldBuildingContext(TypeBuildingContext typeContext, FieldBuilder fieldBuilder)
{
    internal FieldBuilder FieldBuilder { get; } = fieldBuilder;
    
    public TypeBuildingContext TypeContext { get; } = typeContext;

    [field: MaybeNull]
    public FieldInfo BuildingField
    {
        get
        {
            if (field != null)
                return field;
            if (TypeContext.IsBuilt)
                field ??= TypeContext.BuildingType.GetField(FieldBuilder.Name,
                              BindingFlags.Public | BindingFlags.NonPublic |
                              BindingFlags.Instance | BindingFlags.Static)
                          ?? throw new InvalidOperationException("Failed to retrieve the built property.");
            return field ?? FieldBuilder;
        }
    }

    public bool IsStatic { get; } = fieldBuilder.IsStatic;

    public void MarkAttribute(CustomAttributeBuilder attribute)
    {
        FieldBuilder.SetCustomAttribute(attribute);
    }
}

public class InstanceFieldBuildingContext<TField>(TypeBuildingContext typeContext, FieldBuilder fieldBuilder) 
    : FieldBuildingContext(typeContext, fieldBuilder)
{
    public FieldSymbol<TField> Symbol(ValueSymbol instance)
    {
        if (!instance.ValueType.IsAssignableTo(FieldBuilder.DeclaringType))
            throw new ArgumentException(
                $"Instance type '{instance.ValueType}' is not assignable to the " +
                $"declaring type '{FieldBuilder.DeclaringType}' of the field.", nameof(instance));
        return new FieldSymbol<TField>(instance.Context, instance, FieldBuilder);
    }
}

public class StaticFieldBuildingContext<TField>(TypeBuildingContext typeContext, FieldBuilder fieldBuilder) 
    : FieldBuildingContext(typeContext, fieldBuilder)
{
    public StaticFieldSymbol<TField> Symbol(MethodBuildingContext context)
        => new(context, FieldBuilder);
}