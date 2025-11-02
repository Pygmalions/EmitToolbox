namespace EmitToolbox.Framework;

public partial class DynamicType
{
    public TypeBuilder TypeBuilder { get; }

    /// <summary>
    /// The type being built by this dynamic type.
    /// If <see cref="IsBuilt"/> is false, then this will return the underlying <see cref="TypeBuilder"/>.
    /// </summary>
    public Type BuildingType
    {
        get => IsBuilt ? field : TypeBuilder;
        private set;
    } = null!;
    
    /// <summary>
    /// Whether this type has been built.
    /// </summary>
    public bool IsBuilt { get; private set; }
    
    internal DynamicType(TypeBuilder typeBuilder)
    {
        TypeBuilder = typeBuilder;
        
        FieldBuilder = new DynamicFieldBuilder(this);
        PropertyBuilder = new DynamicPropertyBuilder(this);
        ActionBuilder = new DynamicActionBuilder(this);
        FunctorBuilder = new DynamicFunctorBuilder(this);
        
        _fieldCapturedObjects = typeBuilder.DefineField("__CapturedObjects", _listCapturedObjects.GetType(),
            FieldAttributes.Static | FieldAttributes.Private);
    }

    /// <summary>
    /// Build this dynamic type.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this type has already been built.
    /// </exception>
    public void Build()
    {
        if (IsBuilt)
            throw new InvalidOperationException(
                $"Type '{TypeBuilder.Name}' has already been built.");
        
        BuildingType = TypeBuilder.CreateType();
        
        _fieldCapturedObjects = BuildingType.GetField("__CapturedObjects",
            BindingFlags.Static | BindingFlags.NonPublic)!;
        _fieldCapturedObjects.SetValue(null, _listCapturedObjects);
        
        IsBuilt = true;
    }
    
    /// <summary>
    /// Implement the specified interface.
    /// </summary>
    /// <param name="interfaceType">Type of the interface to implement.</param>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="interfaceType"/> is not an interface.
    /// </exception>
    public void ImplementInterface(Type interfaceType)
    {
        if (!interfaceType.IsInterface)
            throw new ArgumentException(
                $"The provided type '{interfaceType.Name}' must be an interface.",
                nameof(interfaceType));
        TypeBuilder.AddInterfaceImplementation(interfaceType);
    }
    
    /// <summary>
    /// Implement the specified interface.
    /// </summary>
    /// <typeparam name="TInterface">Type of the interface to implement.</typeparam>
    /// <exception cref="ArgumentException">
    /// Thrown if <typeparamref name="TInterface"/> is not an interface.
    /// </exception>
    public void ImplementInterface<TInterface>() where TInterface : class
        => ImplementInterface(typeof(TInterface));

    /// <summary>
    /// Add an attribute to this type.
    /// </summary>
    /// <param name="attribute">Attribute to add to this type.</param>
    public void MarkAttribute(CustomAttributeBuilder attribute)
        => TypeBuilder.SetCustomAttribute(attribute);
}