namespace EmitToolbox.Framework.Elements.ObjectMembers;

public abstract class MethodElementBase(MethodContext context, ValueElement? target, MethodInfo method)
{
    public MethodContext Context { get; } = context;
    
    public ValueElement? Target { get; } = method.IsStatic
        ? null
        : target ?? throw new ArgumentException(
            "Target element for an instance method cannot be null.", nameof(target));

    public MethodInfo Method { get; } = method;
    
    public bool EnableVirtualCalling { get; init; } = true;
}
