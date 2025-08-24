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
                field ??= typeContext.BuildingType.GetField(fieldBuilder.Name)!;
            return field ?? fieldBuilder;
        }
    }
    
    public void MarkAttribute(CustomAttributeBuilder attribute)
    {
        fieldBuilder.SetCustomAttribute(attribute);
    }

    public FieldSymbol<TField> Symbol(MethodBuildingContext context)
    {
        return new FieldSymbol<TField>(
            context, new ThisSymbol(context, fieldBuilder.DeclaringType!),
            fieldBuilder
        );
    }
}