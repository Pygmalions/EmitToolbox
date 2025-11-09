namespace EmitToolbox.Framework.Facades;

public readonly struct MethodFacade
{
    private readonly MethodBase? _method;

    private readonly DynamicMethod? _builder;

    public MethodFacade(MethodBase method)
    {
        _method = method;
        _builder = null;
    }

    public MethodFacade(DynamicMethod method)
    {
        _method = null;
        _builder = method;
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

    public static implicit operator MethodFacade(MethodBase method)
        => new(method);

    public static implicit operator MethodFacade(DynamicMethod builder)
        => new(builder);
}