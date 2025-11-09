using System.Diagnostics.CodeAnalysis;

namespace EmitToolbox.Framework;

public class DynamicProperty(DynamicType context, PropertyBuilder builder) : IAttributeMarker<DynamicProperty>
{
    public DynamicType Context { get; } = context;
    
    public PropertyBuilder Builder { get; } = builder;
    
    public DynamicMethod<MethodBuilder, MethodInfo>? Setter { get; private set; }

    public DynamicMethod<MethodBuilder, MethodInfo>? Getter { get; private set; }

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
    
    public void BindSetter(DynamicMethod<MethodBuilder, MethodInfo> setter)
    {
        Builder.SetSetMethod(setter.Builder);
        Setter = setter;
    }
    
    public void BindGetter(DynamicMethod<MethodBuilder, MethodInfo> getter)
    {
        Builder.SetGetMethod(getter.Builder);
        Getter = getter;
    }
    
    public DynamicProperty MarkAttribute(CustomAttributeBuilder attribute)
    {
        Builder.SetCustomAttribute(attribute);
        return this;
    }
    
    public static implicit operator PropertyInfo(DynamicProperty property)
        => property.BuildingProperty;
}