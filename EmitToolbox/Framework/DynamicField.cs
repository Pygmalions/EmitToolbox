using System.Diagnostics.CodeAnalysis;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework;

public class DynamicField(DynamicType context, FieldBuilder builder) : IAttributeMarker<DynamicField>
{
    public DynamicType Context { get; } = context;
    
    public FieldBuilder Builder { get; } = builder;

    [field: MaybeNull]
    public FieldInfo BuildingField
    {
        get
        {
            if (field is not null)
                return field;
            if (!Context.IsBuilt)
                return Builder;
            var flags = (Builder.IsStatic ? BindingFlags.Static : BindingFlags.Instance) |
                        (Builder.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic);
            field = Context.BuildingType.GetField(Builder.Name, flags)!;
            return field;
        }
    }

    public DynamicField MarkAttribute(CustomAttributeBuilder attribute)
    {
        Builder.SetCustomAttribute(attribute);
        return this;
    }
    
    public FieldSymbol SymbolOf(DynamicMethod context, ISymbol? instance = null)
        => new(context, BuildingField, instance);
    
    public FieldSymbol<TField> SymbolOf<TField>(DynamicMethod context, ISymbol? instance = null)
        => new(context, BuildingField, instance);
    
    public static implicit operator FieldInfo(DynamicField field)
        => field.BuildingField;
}