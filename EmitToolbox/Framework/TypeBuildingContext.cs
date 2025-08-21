namespace EmitToolbox.Framework;

public partial class TypeBuildingContext
{
    private readonly TypeBuilder _typeBuilder;

    public Type BuildingType => _typeBuilder;

    internal TypeBuildingContext(TypeBuilder typeBuilder)
    {
        _typeBuilder = typeBuilder;
    }

    public void ImplementInterface(Type interfaceType)
    {
        if (!interfaceType.IsInterface)
            throw new ArgumentException(
                $"The provided type '{interfaceType.Name}' must be an interface.",
                nameof(interfaceType));
        _typeBuilder.AddInterfaceImplementation(interfaceType);
    }

    public void MarkAttribute(CustomAttributeBuilder attribute)
        => _typeBuilder.SetCustomAttribute(attribute);

    public FieldBuildingContext<TField> DefineField<TField>(
        string name, VisibilityLevel visibility = VisibilityLevel.Public)
    {
        var fieldBuilder = _typeBuilder.DefineField(name, typeof(TField), visibility switch
        {
            VisibilityLevel.Public => FieldAttributes.Public,
            VisibilityLevel.Private => FieldAttributes.Private,
            VisibilityLevel.Protected => FieldAttributes.Family,
            VisibilityLevel.Internal => FieldAttributes.Assembly,
            VisibilityLevel.ProtectedInternal => FieldAttributes.FamORAssem,
            _ => throw new ArgumentOutOfRangeException(nameof(visibility), visibility, null)
        });
        return new FieldBuildingContext<TField>(fieldBuilder);
    }

    public PropertyBuildingContext DefineProperty<TProperty>(
        string name, VisibilityLevel visibility = VisibilityLevel.Public,
        bool isReference = false)
    {
        var propertyBuilder = _typeBuilder.DefineProperty(name,
            PropertyAttributes.None, 
            isReference ? typeof(TProperty).MakeByRefType() : typeof(TProperty),
            Type.EmptyTypes);
        return new PropertyBuildingContext(this, propertyBuilder, 
            name, typeof(TProperty), isReference, visibility);
    }
}