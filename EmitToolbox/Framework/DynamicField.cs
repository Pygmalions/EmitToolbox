using System.Diagnostics.CodeAnalysis;
using EmitToolbox.Framework.Symbols.Members;

namespace EmitToolbox.Framework;

public abstract class DynamicField(DynamicType typeContext, FieldBuilder fieldBuilder)
{
    public FieldBuilder FieldBuilder { get; } = fieldBuilder;

    public DynamicType TypeContext { get; } = typeContext;

    [field: MaybeNull]
    public FieldInfo BuildingField
    {
        get
        {
            if (field != null)
                return field;
            if (TypeContext.IsBuilt)
            {
                return field = TypeContext.BuildingType.GetField(FieldBuilder.Name,
                                   BindingFlags.Public | BindingFlags.NonPublic |
                                   BindingFlags.Instance | BindingFlags.Static)
                               ?? throw new InvalidOperationException("Failed to retrieve the built property.");
            }
            return FieldBuilder;
        }
    }

    public bool IsStatic { get; } = fieldBuilder.IsStatic;

    public void MarkAttribute(CustomAttributeBuilder attribute)
    {
        FieldBuilder.SetCustomAttribute(attribute);
    }
}

public class StaticDynamicField(DynamicType typeContext, FieldBuilder fieldBuilder)
    : DynamicField(typeContext, fieldBuilder)
{
    public FieldSymbol SymbolOf(DynamicMethod methodContext)
        => new(methodContext, BuildingField, null);
}

public class InstanceDynamicField(DynamicType typeContext, FieldBuilder fieldBuilder)
    : DynamicField(typeContext, fieldBuilder)
{
    public FieldSymbol SymbolOf(ISymbol instance)
        => new(instance.Context, BuildingField, instance);
}