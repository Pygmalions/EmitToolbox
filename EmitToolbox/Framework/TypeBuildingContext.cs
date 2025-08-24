namespace EmitToolbox.Framework;

public partial class TypeBuildingContext
{
    private readonly TypeBuilder _typeBuilder;

    public Type BuildingType
    {
        get => IsBuilt ? field : _typeBuilder;
        private set;
    } = null!;
    
    public bool IsBuilt { get; private set; }
    
    internal TypeBuildingContext(TypeBuilder typeBuilder)
    {
        _typeBuilder = typeBuilder;
    }

    public void Build()
    {
        if (IsBuilt)
            throw new InvalidOperationException(
                $"Type '{_typeBuilder.Name}' has already been built.");
        
        BuildingType = _typeBuilder.CreateType();
        IsBuilt = true;
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

    public FieldBuildingContext<TField> DefineField<TField>(string name,
        VisibilityLevel visibility = VisibilityLevel.Public, bool isStatic = false)
    {
        var attributes =  visibility switch
        {
            VisibilityLevel.Public => FieldAttributes.Public,
            VisibilityLevel.Private => FieldAttributes.Private,
            VisibilityLevel.Protected => FieldAttributes.Family,
            VisibilityLevel.Internal => FieldAttributes.Assembly,
            VisibilityLevel.ProtectedInternal => FieldAttributes.FamORAssem,
            _ => throw new ArgumentOutOfRangeException(nameof(visibility), visibility, null)
        };
        if (isStatic)
            attributes |= FieldAttributes.Static;
        var fieldBuilder = _typeBuilder.DefineField(name, typeof(TField), attributes);
        
        return new FieldBuildingContext<TField>(this, fieldBuilder);
    }

    public PropertyBuildingContext<TProperty> DefineProperty<TProperty>(
        string name, VisibilityLevel visibility = VisibilityLevel.Public,
        bool isReference = false, MethodModifier modifier = MethodModifier.None)
    {
        var propertyBuilder = _typeBuilder.DefineProperty(name,
            PropertyAttributes.None, 
            isReference ? typeof(TProperty).MakeByRefType() : typeof(TProperty),
            Type.EmptyTypes);
        return new PropertyBuildingContext<TProperty>(this, propertyBuilder, 
            name, typeof(TProperty), isReference, visibility, modifier);
    }
}