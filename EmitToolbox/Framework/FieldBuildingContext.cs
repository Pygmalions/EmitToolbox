using EmitToolbox.Framework.Symbols;
using EmitToolbox.Framework.Symbols.Members;

namespace EmitToolbox.Framework;

public class FieldBuildingContext<TField>(FieldBuilder fieldBuilder)
{
    public FieldInfo BuildingField => fieldBuilder;
    
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