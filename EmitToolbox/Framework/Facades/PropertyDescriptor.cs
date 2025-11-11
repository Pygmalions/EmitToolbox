namespace EmitToolbox.Framework.Facades;

/// <summary>
/// Facade for <see cref="PropertyInfo"/> and <see cref="DynamicProperty"/>.
/// <see cref="MethodBuilder"/> does not implement many methods such as <see cref="MethodBase.GetParameters()"/>,
/// therefore a facade to retrieve metadata from those stored in <see cref="DynamicFunction"/> is required.
/// <br/><br/>
/// Use this struct as a parameter instead of <see cref="PropertyInfo"/> to make dynamic properties behave correctly.
/// </summary>
public readonly struct PropertyDescriptor
{
    private readonly PropertyInfo? _property;
    
    private readonly DynamicProperty? _builder;

    public PropertyDescriptor(PropertyInfo property)
    {
        _property = property;
        _builder = null;
    }
    public PropertyDescriptor(DynamicProperty property)
    {
        _builder = property;
        _property = null;
    }
    
    public MethodDescriptor? Getter
    {
        get
        {
            if (_builder != null)
            {
                if (_builder.Getter != null)
                    return new MethodDescriptor(_builder.Getter);
                return null;
            }
            if (_property != null)
            {
                if (_property.GetMethod != null)
                    return new MethodDescriptor(_property.GetMethod);
                return null;
            }
            throw new InvalidOperationException(
                "This property facade is not bound to a property or a builder.");
        }
    }

    public MethodDescriptor? Setter
    {
        get
        {
            if (_builder != null)
            {
                if (_builder.Setter != null)
                    return new MethodDescriptor(_builder.Setter);
                return null;
            }
            if (_property != null)
            {
                if (_property.SetMethod != null)
                    return new MethodDescriptor(_property.SetMethod);
                return null;
            }
            throw new InvalidOperationException(
                "This property facade is not bound to a property or a builder.");
        }
    }

    public static implicit operator PropertyDescriptor(PropertyInfo property) => new(property);
    
    public static implicit operator PropertyDescriptor(DynamicProperty builder) => new(builder);
}