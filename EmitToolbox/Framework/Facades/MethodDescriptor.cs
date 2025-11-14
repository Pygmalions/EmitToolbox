using EmitToolbox.Framework.Utilities;
using OneOf;

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
    private readonly OneOf<MethodBase, DynamicFunction> _method;

    public MethodDescriptor(MethodBase method)
    {
        _method = method ?? throw new ArgumentNullException(nameof(method));
    }

    public MethodDescriptor(DynamicFunction function)
    {
        _method = function ?? throw new ArgumentNullException(nameof(function));
    }

    public MethodBase Method => _method.Match(
        method => method,
        builder => builder.BuildingMethod
    );

    public IEnumerable<Type> ParameterTypes => _method.Match(
        method => method.GetParameterTypes(),
        builder => builder.ParameterTypes
    );

    public Type ReturnType=> _method.Match(
        metadata => metadata is MethodInfo method ? method.ReturnType : typeof(void),
        builder => builder.ReturnType
    );

    public static implicit operator MethodDescriptor(MethodBase method)
        => new(method);

    public static implicit operator MethodDescriptor(DynamicFunction builder)
        => new(builder);
}
