using System.Diagnostics.CodeAnalysis;

namespace EmitToolbox;

public class DynamicProperty(DynamicType context, PropertyBuilder builder) : IAttributeMarker
{
    public DynamicType Context { get; } = context;
    
    public PropertyBuilder Builder { get; } = builder;
    
    public DynamicMethod? Setter { get; private set; }

    public DynamicMethod? Getter { get; private set; }

    [field: MaybeNull]
    public PropertyInfo BuildingProperty
    {
        get
        {
            if (field is not null)
                return field;
            if (!Context.IsBuilt)
                return Builder;
            field = Context.BuildingType.GetProperty(
                Builder.Name, 
                BindingFlags.Public | BindingFlags.NonPublic | 
                BindingFlags.Instance | BindingFlags.Static)!;
            return field;
        }
    }
    
    public void BindSetter(DynamicMethod setter)
    {
        Builder.SetSetMethod(setter.Builder);
        Setter = setter;
    }
    
    public void BindGetter(DynamicMethod getter)
    {
        Builder.SetGetMethod(getter.Builder);
        Getter = getter;
    }
    
    public IAttributeMarker MarkAttribute(CustomAttributeBuilder attribute)
    {
        Builder.SetCustomAttribute(attribute);
        return this;
    }
    
    public static implicit operator PropertyInfo(DynamicProperty property)
        => property.BuildingProperty;
}