using System.Linq.Expressions;
using EmitToolbox.Framework.Symbols.Extensions;

namespace EmitToolbox.Framework.Symbols.Members;

public class PropertySymbol : IAssignableSymbol
{
    public DynamicMethod Context { get; }

    public Type ValueType { get; }

    public PropertyInfo Property { get; }

    public ISymbol? Target { get; }

    public bool EnabledVirtualCalling { get; init; } = true;

    public bool HasGetter { get; }

    public bool HasSetter { get; }

    public PropertySymbol(DynamicMethod context, PropertyInfo property, ISymbol? target)
    {
        Context = context;
        ValueType = property.PropertyType;
        if (property.GetMethod?.IsStatic == false || property.SetMethod?.IsStatic == false)
        {
            if (target == null)
                throw new ArgumentException("Cannot create a instance property symbol: target instance is null.",
                    nameof(target));
            if (!target.ValueType.WithoutByRef().IsAssignableTo(property.DeclaringType))
                throw new ArgumentException(
                    "Cannot create a instance property symbol: " +
                    "target instance cannot be assigned to the declaring type of the property.",
                    nameof(target));
            Target = target;
        }

        Property = property;
        HasGetter = property.GetMethod != null;
        HasSetter = property.SetMethod != null;
    }

    public void EmitLoadContent()
    {
        if (Property.GetMethod == null)
            throw new InvalidOperationException($"Property '{Property.Name}' does not have a getter.");

        var code = Context.Code;

        if (Target != null)
        {
            Target.EmitLoadAsTarget();
            code.Emit(EnabledVirtualCalling && Property.GetMethod.IsVirtual ? OpCodes.Callvirt : OpCodes.Call,
                Property.GetMethod);
        }
        else
        {
            code.Emit(OpCodes.Call, Property.GetMethod);
        }
    }

    public void EmitStoreContent()
    {
        if (Property.SetMethod == null)
            throw new InvalidOperationException($"Property '{Property.Name}' does not have a setter.");

        var code = Context.Code;

        if (Target != null)
        {
            var temporary = code.DeclareLocal(ValueType.WithoutByRef());
            code.Emit(OpCodes.Stloc, temporary);
            Target.EmitLoadAsTarget();
            code.Emit(OpCodes.Ldloc, temporary);
            code.Emit(EnabledVirtualCalling && Property.SetMethod.IsVirtual ? OpCodes.Callvirt : OpCodes.Call,
                Property.SetMethod);
        }
        else
        {
            code.Emit(OpCodes.Call, Property.SetMethod);
        }
    }
}

public static class PropertySymbolExtensions
{
    public static PropertySymbol PropertyOf(this ISymbol symbol, PropertyInfo property)
        => new (symbol.Context, property, symbol);
    
    public static PropertySymbol PropertyOf<TTarget, TValue>(
        this ISymbol<TTarget> target, Expression<Func<TTarget, TValue>> expression)
    {
        return expression.Body is not MemberExpression memberExpression
            ? throw new ArgumentException("Expression must be a property access expression.", nameof(expression))
            : new PropertySymbol(target.Context, (PropertyInfo)memberExpression.Member, target);
    }
}