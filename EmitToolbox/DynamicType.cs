namespace EmitToolbox;

public class DynamicType
{
    public DynamicAssembly DeclaringAssembly { get; }

    public TypeBuilder Builder
    {
        get => field ?? throw new InvalidOperationException(
            "Cannot access the type builder: this type has been built.");
        private set;
    }

    public MethodFactory MethodFactory { get; }
    
    public FieldFactory FieldFactory { get; }
    
    public PropertyFactory PropertyFactory { get; }
    
    /// <summary>
    /// Refer to the type builder when <see cref="IsBuilt"/> is false;
    /// and refer to the built type when <see cref="IsBuilt"/> is true.
    /// </summary>
    public Type BuildingType { get; private set; }

    /// <summary>
    /// True if this type has been built; otherwise, false.
    /// </summary>
    public bool IsBuilt { get; private set; } = false;

    public DynamicType(DynamicAssembly assembly, TypeBuilder builder)
    {
        DeclaringAssembly = assembly;
        Builder = builder;
        BuildingType = builder;
        
        MethodFactory = new MethodFactory(this);
        FieldFactory = new FieldFactory(this);
        PropertyFactory = new PropertyFactory(this);
    }

    /// <summary>
    /// Add the specified interface into the interface map of this type.
    /// </summary>
    /// <param name="interfaceType">Interface type for this type to implement.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when the specified type is not an interface.
    /// </exception>
    /// <returns>This dynamic type.</returns>
    public DynamicType ImplementInterface(Type interfaceType)
    {
        if (!interfaceType.IsInterface)
            throw new ArgumentException("Specified type is not an interface.", nameof(interfaceType));
        Builder.AddInterfaceImplementation(interfaceType);
        return this;
    }
    
    /// <summary>
    /// Build the type.
    /// </summary>
    public void Build()
    {
        if (IsBuilt)
            throw new InvalidOperationException("The type is already built.");
        IsBuilt = true;
        var type = Builder.CreateType();
        BuildingType = type;
        Builder = null!;
    }
    
    public static implicit operator Type(DynamicType type)
        => type.BuildingType;
}