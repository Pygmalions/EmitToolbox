namespace EmitToolbox.Framework.Symbols.Members;

public class MethodSymbol(MethodBuildingContext context, ValueSymbol? target, MethodInfo method)
{
    public MethodBuildingContext Context { get; } = context;
    
    public ValueSymbol? Target { get; } = method.IsStatic
        ? null
        : target ?? throw new ArgumentException(
            "Target element for an instance method cannot be null.", nameof(target));

    public MethodInfo Method { get; } = method;
    
    public bool EnableVirtualCalling { get; init; } = true;
    
    protected ParameterInfo[] Parameters { get; } = method.GetParameters();
    
    public void Invoke(ValueSymbol[] parameters)
    {
        if (Method.ReturnType != typeof(void))
            throw new Exception($"Method {Method.Name} does not return void.");
        
        Target?.EmitLoadAsTarget();

        foreach (var (index, parameter) in parameters.Index())
        {
            parameter.EmitLoadAsParameter(Parameters[index]);
        }

        if (EnableVirtualCalling && !Method.IsStatic)
        {
            Context.Code.Emit(OpCodes.Callvirt, Method);
        }
        else
        {
            Context.Code.Emit(OpCodes.Call, Method);
        }
    }
    
    public VariableSymbol<TResult> Invoke<TResult>(ValueSymbol[] parameters)
    {
        if (!Method.ReturnType.IsAssignableTo(typeof(TResult)))
            throw new Exception($"Method {Method.Name} cannot return type {typeof(TResult).Name}.");

        Target?.EmitLoadAsTarget();

        foreach (var (index, parameter) in parameters.Index())
        {
            parameter.EmitLoadAsParameter(Parameters[index]);
        }

        if (EnableVirtualCalling && !Method.IsStatic)
        {
            Context.Code.Emit(OpCodes.Callvirt, Method);
        }
        else
        {
            Context.Code.Emit(OpCodes.Call, Method);
        }
        
        var result = Context.Variable<TResult>();
        result.EmitStoreFromValue();
        return result;
    }
}
