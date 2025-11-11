using System.Diagnostics.CodeAnalysis;

namespace EmitToolbox.Framework;

public class DynamicProperty(DynamicType context, PropertyBuilder builder) : IAttributeMarker<DynamicProperty>
{
    public DynamicType Context { get; } = context;
    
    public PropertyBuilder Builder { get; } = builder;
    
    public DynamicFunction<MethodBuilder, MethodInfo>? Setter { get; private set; }

    public DynamicFunction<MethodBuilder, MethodInfo>? Getter { get; private set; }

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
    
    public void BindSetter(DynamicFunction<MethodBuilder, MethodInfo> setter)
    {
        Builder.SetSetMethod(setter.Builder);
        Setter = setter;
    }
    
    public void BindGetter(DynamicFunction<MethodBuilder, MethodInfo> getter)
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