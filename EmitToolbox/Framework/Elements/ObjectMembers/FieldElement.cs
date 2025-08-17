using System.Linq.Expressions;

namespace EmitToolbox.Framework.Elements.ObjectMembers;

public class FieldElement<TValue>(MethodContext context, ValueElement? target, FieldInfo field)
    : VariableElement<TValue>(context)
{
    public ValueElement? Target { get; } = field.IsStatic
        ? null
        : target ?? throw new ArgumentException(
            "Target element for an instance field cannot be null.", nameof(target));

    public FieldInfo Field { get; } = field;

    protected internal override void EmitLoadAsValue()
    {
        Target?.EmitLoadAsTarget();
        Context.Code.Emit(OpCodes.Ldfld, Field);
    }

    protected internal override void EmitLoadAsAddress()
    {
        Target?.EmitLoadAsTarget();
        Context.Code.Emit(OpCodes.Ldflda, Field);
    }

    protected internal override void EmitStoreValue()
    {
        if (Field.IsStatic)
        {
            Context.Code.Emit(OpCodes.Stfld, Field);
            return;
        }

        var value = Context.DefineVariable<TValue>();
        value.EmitStoreValue();

        Target!.EmitLoadAsTarget();
        value.EmitLoadAsValue();
        Context.Code.Emit(OpCodes.Stfld, Field);
    }
}

public static class FieldElementExtension
{
    public static FieldElement<TValue> GetField<TTarget, TValue>(
        this ValueElement<TTarget> target, Expression<Func<TTarget, TValue>> expression)
    {
        return expression.Body is not MemberExpression memberExpression
            ? throw new ArgumentException("Expression must be a field access expression.", nameof(expression))
            : new FieldElement<TValue>(target.Context, target, (FieldInfo)memberExpression.Member);
    }
}