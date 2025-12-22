using System.Diagnostics.CodeAnalysis;
using EmitToolbox.Symbols;

namespace EmitToolbox;

public class DynamicField(DynamicType context, FieldBuilder builder) : IAttributeMarker
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

    public IAttributeMarker MarkAttribute(CustomAttributeBuilder attribute)
    {
        Builder.SetCustomAttribute(attribute);
        return this;
    }

    public FieldSymbol SymbolOf(DynamicFunction context, ISymbol? instance = null)
        => new(context, BuildingField, instance);

    public FieldSymbol<TField> SymbolOf<TField>(DynamicFunction context, ISymbol? instance = null)
        => new(context, BuildingField, instance);

    /// <summary>
    /// Get a field symbol for the specified instance.
    /// </summary>
    /// <param name="instance">Instance whose field is accessed.</param>
    /// <returns>Field symbol of the specified instance.</returns>
    public FieldSymbol SymbolOf(ISymbol instance)
        => SymbolOf(instance.Context, instance);

    /// <summary>
    /// Get a field symbol for the specified instance.
    /// </summary>
    /// <param name="instance">Instance whose field is accessed.</param>
    /// <returns>Field symbol of the specified instance.</returns>
    public FieldSymbol<TField> SymbolOf<TField>(ISymbol instance)
        => SymbolOf<TField>(instance.Context, instance);

    public static implicit operator FieldInfo(DynamicField field)
        => field.BuildingField;
}