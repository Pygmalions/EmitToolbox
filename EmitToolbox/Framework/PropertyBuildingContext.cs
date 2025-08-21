namespace EmitToolbox.Framework;

public class PropertyBuildingContext(
    TypeBuildingContext typeContext, PropertyBuilder propertyBuilder,
    string propertyName, Type propertyType, bool isReference, VisibilityLevel visibility)
{
    public PropertyInfo BuildingProperty => propertyBuilder;
    
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

        Setter = typeContext.DefineInstanceAction(
            "set_" + propertyName,
            [new ParameterDefinition(propertyType, 
                isReference ? ParameterModifier.Ref : ParameterModifier.None,
                "value")],
            visibility);

        return Setter;
    }
    
    public FunctorMethodBuildingContext DefineGetter()
    {
        if (Getter != null)
            throw new InvalidOperationException("Getter is already defined for this property.");
        
        Getter = typeContext.DefineInstanceFunctor(
            "get_" + propertyName, [],
            new ResultDefinition(isReference ? propertyType.MakeByRefType() : propertyType),
            visibility);

        return Getter;
    }
}