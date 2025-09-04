namespace EmitToolbox.Framework;

public partial class TypeBuildingContext
{
    public TypeBuilder TypeBuilder { get; }

    public Type BuildingType
    {
        get => IsBuilt ? field : TypeBuilder;
        private set;
    } = null!;
    
    public bool IsBuilt { get; private set; }
    
    internal TypeBuildingContext(TypeBuilder typeBuilder)
    {
        TypeBuilder = typeBuilder;

        Actions = new ActionBuilder(this);
        Functors = new FunctorBuilder(this);
        Fields = new FieldBuilder(this);
        Properties = new PropertyBuilder(this);
    }

    public void Build()
    {
        if (IsBuilt)
            throw new InvalidOperationException(
                $"Type '{TypeBuilder.Name}' has already been built.");
        
        BuildingType = TypeBuilder.CreateType();
        IsBuilt = true;
    }
    
    public void ImplementInterface(Type interfaceType)
    {
        if (!interfaceType.IsInterface)
            throw new ArgumentException(
                $"The provided type '{interfaceType.Name}' must be an interface.",
                nameof(interfaceType));
        TypeBuilder.AddInterfaceImplementation(interfaceType);
    }

    public void MarkAttribute(CustomAttributeBuilder attribute)
        => TypeBuilder.SetCustomAttribute(attribute);
}