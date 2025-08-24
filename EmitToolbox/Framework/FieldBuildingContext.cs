using System.Diagnostics.CodeAnalysis;
using EmitToolbox.Framework.Symbols;
using EmitToolbox.Framework.Symbols.Members;

namespace EmitToolbox.Framework;

public class FieldBuildingContext<TField>(TypeBuildingContext typeContext, FieldBuilder fieldBuilder)
{
    [field: MaybeNull]
    public FieldInfo BuildingField
    {
        get
        {
            if (typeContext.IsBuilt)
                field ??= typeContext.BuildingType.GetField(fieldBuilder.Name,
                    BindingFlags.Public | BindingFlags.NonPublic |
                    (fieldBuilder.IsStatic ?  BindingFlags.Static : BindingFlags.Instance))
                    ?? throw new InvalidOperationException("Failed to retrieve the built field.");
            return field ?? fieldBuilder;
        }
    }
    
    public bool IsStatic { get; } = fieldBuilder.IsStatic;
    
    public void MarkAttribute(CustomAttributeBuilder attribute)
    {
        fieldBuilder.SetCustomAttribute(attribute);
    }

    public FieldSymbol<TField> InstanceSymbol(MethodBuildingContext context)
    {
        if (!fieldBuilder.IsStatic && context.IsStatic)
            throw new InvalidOperationException("Cannot access instance field in static method.");
        
        return new FieldSymbol<TField>(
            context, new ThisSymbol(context, fieldBuilder.DeclaringType!),
            fieldBuilder
        );
    }
    
    public FieldSymbol<TField> InstanceSymbol(ValueSymbol instance)
    {
        return new FieldSymbol<TField>(instance.Context, instance, fieldBuilder);
    }
    
    public StaticFieldSymbol<TField> StaticSymbol(MethodBuildingContext context)
    {
        if (!fieldBuilder.IsStatic)
            throw new InvalidOperationException("This field is not static.");
        
        return new StaticFieldSymbol<TField>(context, fieldBuilder);
    }
}