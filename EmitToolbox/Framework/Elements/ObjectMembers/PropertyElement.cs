using System.Linq.Expressions;

namespace EmitToolbox.Framework.Elements.ObjectMembers;

public class PropertyElement<TValue>(MethodContext context, ValueElement? target, PropertyInfo property)
    : VariableElement<TValue>(context)
{
    public ValueElement? Target { get; } = property.GetMethod?.IsStatic == false || 
                                           property.SetMethod?.IsStatic == false
        ? target ?? throw new ArgumentException(
            "Target element for an instance property cannot be null.", nameof(target))
        : null;

    public PropertyInfo Property { get; } = property;
    
    protected internal override void EmitLoadAsValue()
    {
        if (Property.GetMethod == null)
            throw new InvalidOperationException($"Property '{Property.Name}' does not have a getter.");
        Target?.EmitLoadAsTarget();
        Context.Code.Emit(Property.GetMethod.IsVirtual ? OpCodes.Callvirt : OpCodes.Call,
            Property.GetMethod);
    }

    protected internal override void EmitLoadAsAddress()
    {
        var value = Context.DefineVariable<TValue>();
        EmitLoadAsValue();
        value.EmitStoreValue();
        value.EmitLoadAsAddress();
    }

    protected internal override void EmitStoreValue()
    {
        if (Property.SetMethod == null)
            throw new InvalidOperationException($"Property '{Property.Name}' does not have a getter.");
        
        var value = Context.DefineVariable<TValue>();
        value.EmitStoreValue();
        
        Target?.EmitLoadAsTarget();
        value.EmitLoadAsValue();
        Context.Code.Emit(Property.SetMethod.IsVirtual ? OpCodes.Callvirt : OpCodes.Call,
            Property.SetMethod);
    }
}

public static class PropertyElementExtension
{
    public static PropertyElement<TValue> GetProperty<TTarget, TValue>(
        this ValueElement<TTarget> target, Expression<Func<TTarget, TValue>> expression)
    {
        return expression.Body is not MemberExpression memberExpression
            ? throw new ArgumentException("Expression must be a property access expression.", nameof(expression))
            : new PropertyElement<TValue>(target.Context, target, (PropertyInfo)memberExpression.Member);
    }
}