namespace EmitToolbox.Framework.Elements.ObjectMembers;

public class MethodElement(MethodContext context, ValueElement? target, MethodInfo method)
{
    public MethodContext Context { get; } = context;
    
    public ValueElement? Target { get; } = method.IsStatic
        ? null
        : target ?? throw new ArgumentException(
            "Target element for an instance method cannot be null.", nameof(target));

    public MethodInfo Method { get; } = method;
    
    public bool EnableVirtualCalling { get; init; } = true;
    
    public void Invoke(ValueElement[] parameters)
    {
        if (Method.ReturnType != typeof(void))
            throw new Exception($"Method {Method.Name} does not return void.");
        
        Target?.EmitLoadAsTarget();

        foreach (var parameter in parameters)
        {
            parameter.EmitLoadAsValue();
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
    
    public VariableElement<TResult> Invoke<TResult>(ValueElement[] parameters)
    {
        if (!Method.ReturnType.IsAssignableTo(typeof(TResult)))
            throw new Exception($"Method {Method.Name} cannot return type {typeof(TResult).Name}.");

        Target?.EmitLoadAsTarget();

        foreach (var parameter in parameters)
        {
            parameter.EmitLoadAsValue();
        }

        if (EnableVirtualCalling && !Method.IsStatic)
        {
            Context.Code.Emit(OpCodes.Callvirt, Method);
        }
        else
        {
            Context.Code.Emit(OpCodes.Call, Method);
        }
        
        var result = Context.DefineVariable<TResult>();
        result.EmitStoreValue();
        return result;
    }
}
