namespace EmitToolbox.Framework.Facades;

/// <summary>
/// Facade for <see cref="MethodBase"/> and <see cref="DynamicFunction"/>.
/// <see cref="MethodBuilder"/> does not implement many methods such as <see cref="MethodBase.GetParameters()"/>,
/// therefore a facade to retrieve metadata from those stored in <see cref="DynamicFunction"/> is required.
/// <br/><br/>
/// Use this struct as a parameter instead of <see cref="MethodInfo"/> to make dynamic methods behave correctly.
/// </summary>
public readonly struct MethodDescriptor
{
    private readonly MethodBase? _method;

    private readonly DynamicFunction? _builder;

    public MethodDescriptor(MethodBase method)
    {
        _method = method;
        _builder = null;
    }

    public MethodDescriptor(DynamicFunction function)
    {
        _method = null;
        _builder = function;
    }

    public MethodBase Method
    {
        get
        {
            if (_method != null)
                return _method;
            if (_builder != null)
                return _builder.BuildingMethod;
            throw new InvalidOperationException(
                "This method facade is not bound to a method or a builder.");
        }
    }

    public IEnumerable<Type> ParameterTypes
    {
        get
        {
            if (_builder != null)
                return _builder.ParameterTypes;
            return _method switch
            {
                MethodInfo method => method.GetParameters()
                    .Select(parameter => parameter.ParameterType),
                ConstructorInfo constructor => constructor.GetParameters()
                    .Select(parameter => parameter.ParameterType),
                _ => throw new InvalidOperationException(
                    "This method facade is not bound to a method or a builder.")
            };
        }
    }

    public Type ReturnType
    {
        get
        {
            if (_builder != null)
                return _builder.ReturnType;
            return _method switch
            {
                MethodInfo method => method.ReturnType,
                ConstructorInfo => typeof(void),
                _ => throw new InvalidOperationException(
                    "This method facade is not bound to a method or a builder.")
            };
        }
    }

    public static implicit operator MethodDescriptor(MethodBase method)
        => new(method);

    public static implicit operator MethodDescriptor(DynamicFunction builder)
        => new(builder);
}