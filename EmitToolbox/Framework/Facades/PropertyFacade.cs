namespace EmitToolbox.Framework.Facades;

/// <summary>
/// Facade for <see cref="PropertyInfo"/> and <see cref="DynamicProperty"/>.
/// </summary>
public readonly struct PropertyFacade
{
    private readonly PropertyInfo? _property;
    
    private readonly DynamicProperty? _builder;

    public PropertyFacade(PropertyInfo property)
    {
        _property = property;
        _builder = null;
    }
    public PropertyFacade(DynamicProperty property)
    {
        _builder = property;
        _property = null;
    }
    
    public MethodFacade? Getter
    {
        get
        {
            if (_builder != null)
            {
                if (_builder.Getter != null)
                    return new MethodFacade(_builder.Getter);
                return null;
            }
            if (_property != null)
            {
                if (_property.GetMethod != null)
                    return new MethodFacade(_property.GetMethod);
                return null;
            }
            throw new InvalidOperationException(
                "This property facade is not bound to a property or a builder.");
        }
    }

    public MethodFacade? Setter
    {
        get
        {
            if (_builder != null)
            {
                if (_builder.Setter != null)
                    return new MethodFacade(_builder.Setter);
                return null;
            }
            if (_property != null)
            {
                if (_property.SetMethod != null)
                    return new MethodFacade(_property.SetMethod);
                return null;
            }
            throw new InvalidOperationException(
                "This property facade is not bound to a property or a builder.");
        }
    }

    public static implicit operator PropertyFacade(PropertyInfo property) => new(property);
    
    public static implicit operator PropertyFacade(DynamicProperty builder) => new(builder);
}