using System.Linq.Expressions;

namespace EmitToolbox.Framework.Symbols.Members;

public class PropertySymbol<TValue> : VariableSymbol<TValue>
{
    public ValueSymbol Target { get; }

    public PropertyInfo Property { get; }

    public bool EnableVirtualCalling { get; init; } = true;

    public bool HasGetter { get; }

    public bool HasSetter { get; }

    public PropertySymbol(MethodBuildingContext context, ValueSymbol target, PropertyInfo property)
        : base(context, property.PropertyType.IsByRef)
    {
        if (property.GetMethod?.IsStatic == true || property.SetMethod?.IsStatic == true)
            throw new ArgumentException("Cannot create an instance property symbol for a static property.",
                nameof(property));
        if (!target.ValueType.IsAssignableTo(property.DeclaringType))
            throw new ArgumentException(
                "Target type is not assignable to the declaring type of the property.", nameof(target));
        Target = target;
        Property = property;
        HasGetter = property.GetMethod != null;
        HasSetter = property.SetMethod != null;
    }

    public override void EmitDirectlyStoreValue()
    {
        if (Property.SetMethod == null)
            throw new InvalidOperationException($"Property '{Property.Name}' does not have a setter.");

        TemporaryVariable.EmitStoreFromValue();
        Target.EmitLoadAsTarget();
        TemporaryVariable.EmitLoadAsValue();
        Context.Code.Emit(EnableVirtualCalling && Property.SetMethod.IsVirtual ? OpCodes.Callvirt : OpCodes.Call,
            Property.SetMethod);
    }

    public override void EmitDirectlyLoadValue()
    {
        if (Property.GetMethod == null)
            throw new InvalidOperationException($"Property '{Property.Name}' does not have a getter.");
        Target.EmitLoadAsTarget();
        Context.Code.Emit(EnableVirtualCalling && Property.GetMethod.IsVirtual ? OpCodes.Callvirt : OpCodes.Call,
            Property.GetMethod);
    }

    public override void EmitDirectlyLoadAddress()
    {
        var value = Context.Variable<TValue>();
        EmitDirectlyLoadValue();
        value.EmitStoreFromValue();
    }
}

public class StaticPropertySymbol<TValue> : VariableSymbol<TValue>
{
    public PropertyInfo Property { get; }

    public bool HasGetter { get; }

    public bool HasSetter { get; }

    public StaticPropertySymbol(MethodBuildingContext context, PropertyInfo property) : base(context)
    {
        if (property.GetMethod?.IsStatic == false || property.SetMethod?.IsStatic == false)
            throw new ArgumentException("Cannot create a static property symbol for an instance property.",
                nameof(property));
        Property = property;
        HasGetter = property.GetMethod != null;
        HasSetter = property.SetMethod != null;
    }

    public override void EmitDirectlyStoreValue()
    {
        if (Property.SetMethod == null)
            throw new InvalidOperationException($"Property '{Property.Name}' does not have a setter.");

        Context.Code.Emit(OpCodes.Call, Property.SetMethod);
    }

    public override void EmitDirectlyLoadValue()
    {
        if (Property.GetMethod == null)
            throw new InvalidOperationException($"Property '{Property.Name}' does not have a getter.");

        Context.Code.Emit(OpCodes.Call, Property.GetMethod);
    }

    public override void EmitDirectlyLoadAddress()
    {
        var value = Context.Variable<TValue>();
        EmitDirectlyLoadValue();
        value.EmitStoreFromValue();
    }
}

public static class PropertyElementExtension
{
    public static PropertySymbol<TValue> GetProperty<TTarget, TValue>(
        this ValueSymbol<TTarget> target, Expression<Func<TTarget, TValue>> expression)
    {
        return expression.Body is not MemberExpression memberExpression
            ? throw new ArgumentException("Expression must be a property access expression.", nameof(expression))
            : new PropertySymbol<TValue>(target.Context, target, (PropertyInfo)memberExpression.Member);
    }

    public static PropertySymbol<TProperty> SetProperty<TTarget, TProperty, TValue>(
        this ValueSymbol<TTarget> target, Expression<Func<TTarget, TProperty>> expression,
        ValueSymbol<TValue> value)
        where TValue : TProperty
    {
        var property = GetProperty(target, expression);
        property.Assign(value);
        return property;
    }
}