using EmitToolbox.Extensions;
using EmitToolbox.Facades;

namespace EmitToolbox.Symbols.Operations;

/// <summary>
/// Invoke the specified method with the specified arguments.
/// These arguments will be emitted as required by the method signature:
/// emitting as references for 'in', 'out' and 'ref' parameters, and as values for normal parameters.
/// </summary>
public class InvocationOperation : OperationSymbol
{
    public MethodDescriptor Site { get; }

    public ISymbol? Target { get; }

    public IReadOnlyCollection<ISymbol> Arguments { get; }

    public bool ForceDirectCall { get; }

    /// <summary>
    /// Create an operation that invokes a method.
    /// </summary>
    /// <param name="context">Context of this invocation.</param>
    /// <param name="site">Method to invoke.</param>
    /// <param name="target">Target symbol to invoke the method on.</param>
    /// <param name="arguments">Argument symbols to pass to the method.</param>
    /// <param name="forceDirectCall">
    /// If true, the method will be called directly (<see cref="OpCodes.Call"/>),
    /// bypassing virtual dispatch (<see cref="OpCodes.Callvirt"/>).
    /// </param>
    public InvocationOperation(
        MethodDescriptor site,
        ISymbol? target,
        IReadOnlyCollection<ISymbol> arguments,
        bool forceDirectCall = false,
        DynamicFunction? context = null) : 
        base(CrossContextException.EnsureContext(context, [target, ..arguments]),
            site.ReturnType)
    {
        Site = site;
        Target = target;
        Arguments = arguments;
        ForceDirectCall = forceDirectCall;

        var method = site.Method;
        if (Target is null && !method.IsStatic)
            throw new ArgumentException("Cannot invoke an instance method without specifying a target.");
        if (Target is not null && method.IsStatic)
            throw new ArgumentException("Cannot invoke a static method on a target instance.");
        if (method.IsAbstract && forceDirectCall)
            throw new ArgumentException(
                "Cannot invoke an abstract method with forcing direct call.");
    }

    public override void LoadContent()
    {
        Target?.LoadAsTarget();

        foreach (var (parameter, symbol) in Site.ParameterTypes.Zip(Arguments))
            symbol.LoadForType(parameter);

        switch (Site.Method)
        {
            case MethodInfo method:
                Context.Code.Emit(
                    ForceDirectCall || method.IsStatic || !method.IsVirtual
                        ? OpCodes.Call
                        : OpCodes.Callvirt,
                    method);
                break;
            case ConstructorInfo constructor:
                Context.Code.Emit(OpCodes.Call, constructor);
                break;
            default:
                throw new InvalidOperationException(
                    $"Unsupported method type '{Site.Method.GetType()}'.");
        }
    }
}

public class InvocationOperation<TResult>(
    MethodDescriptor site,
    ISymbol? target,
    IReadOnlyCollection<ISymbol> arguments,
    bool forceDirectCall = false,
    DynamicFunction? context = null) 
    : InvocationOperation(site, target, arguments, forceDirectCall, context), IOperationSymbol<TResult>
{
}