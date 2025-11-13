using OneOf;

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
    private readonly OneOf<PropertyInfo, DynamicProperty> _property;


    public PropertyDescriptor(PropertyInfo property)
    {
        _property = property;
    }

    public PropertyDescriptor(DynamicProperty property)
    {
        _property = property;
    }

    public MethodDescriptor? Getter =>
        _property.Match(
            property => property.GetMethod is { } method
                ? new MethodDescriptor(method)
                : default,
            builder => builder.Getter is { } method
                ? new MethodDescriptor(method)
                : default);

    public MethodDescriptor? Setter
        => _property.Match(
            property => property.SetMethod is { } method
                ? new MethodDescriptor(method)
                : default,
            builder => builder.Setter is { } method
                ? new MethodDescriptor(method)
                : default);

    public static implicit operator PropertyDescriptor(PropertyInfo property) => new(property);

    public static implicit operator PropertyDescriptor(DynamicProperty builder) => new(builder);
}

public readonly struct PropertyDescriptor<TTarget>
{
    private readonly OneOf<PropertyInfo, DynamicProperty> _property;

    public PropertyDescriptor(string name)
        : this(typeof(TTarget).GetProperty(name) ??
               throw new ArgumentException($"Cannot find property '{name}' on type '{typeof(TTarget)}'."))
    {
    }

    public PropertyDescriptor(PropertyInfo property)
    {
        _property = property;
    }

    public PropertyDescriptor(DynamicProperty builder)
    {
        _property = builder;
    }

    public MethodDescriptor? Getter =>
        _property.Match(
            property => property.GetMethod is { } method
                ? new MethodDescriptor(method)
                : default,
            builder => builder.Getter is { } method
                ? new MethodDescriptor(method)
                : default);

    public MethodDescriptor? Setter
        => _property.Match(
            property => property.SetMethod is { } method
                ? new MethodDescriptor(method)
                : default,
            builder => builder.Setter is { } method
                ? new MethodDescriptor(method)
                : default);

    public static implicit operator PropertyDescriptor<TTarget>(PropertyInfo property) => new(property);

    public static implicit operator PropertyDescriptor<TTarget>(DynamicProperty builder) => new(builder);

    public static implicit operator PropertyDescriptor<TTarget>(string name) => new(name);
}