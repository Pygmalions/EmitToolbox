using System.Linq.Expressions;

namespace EmitToolbox.Framework.Symbols.Members;

public class FieldSymbol<TValue> : VariableSymbol<TValue>
{
    public FieldSymbol(MethodBuildingContext context, ValueSymbol target, FieldInfo field)
        : base(context, field.FieldType.IsByRef)
    {
        if (field.IsStatic)
            throw new ArgumentException("Cannot create an instance field symbol for a static field.", nameof(field));
        if (!target.ValueType.IsAssignableTo(field.DeclaringType))
            throw new ArgumentException(
                "Target type is not assignable to the declaring type of the field.", nameof(target));
        Target = target;
        Field = field;
    }

    /// <summary>
    /// Target symbol for the field.
    /// </summary>
    public ValueSymbol Target { get; }

    /// <summary>
    /// Field wrapped in this symbol.
    /// </summary>
    public FieldInfo Field { get; }

    public override void EmitDirectlyStoreValue()
    {
        TemporaryVariable.EmitStoreFromValue();
        Target.EmitLoadAsTarget();
        TemporaryVariable.EmitLoadAsValue();
        Context.Code.Emit(OpCodes.Stfld, Field);
    }

    public override void EmitDirectlyLoadValue()
    {
        Target.EmitLoadAsTarget();
        Context.Code.Emit(OpCodes.Ldfld, Field);
    }

    public override void EmitDirectlyLoadAddress()
    {
        Target.EmitLoadAsTarget();
        Context.Code.Emit(OpCodes.Ldflda, Field);
    }
}

public class StaticFieldSymbol<TValue> : VariableSymbol<TValue>
{
    public StaticFieldSymbol(MethodBuildingContext context, FieldInfo field)
        : base(context, field.FieldType.IsByRef)
    {
        if (!field.IsStatic)
            throw new ArgumentException("Cannot create a static field symbol for an instance field.", nameof(field));
        Field = field;
    }

    public FieldInfo Field { get; }

    public override void EmitDirectlyStoreValue()
    {
        Context.Code.Emit(OpCodes.Stsfld, Field);
    }

    public override void EmitDirectlyLoadValue()
    {
        Context.Code.Emit(OpCodes.Ldsfld, Field);
    }

    public override void EmitDirectlyLoadAddress()
    {
        Context.Code.Emit(OpCodes.Ldsflda, Field);
    }
}

public static class FieldSymbolExtension
{
    public static FieldSymbol<TField> GetField<TTarget, TField>(
        this ValueSymbol<TTarget> target, Expression<Func<TTarget, TField>> expression)
    {
        return expression.Body is not MemberExpression memberExpression
            ? throw new ArgumentException("Expression must be a field access expression.", nameof(expression))
            : new FieldSymbol<TField>(target.Context, target, (FieldInfo)memberExpression.Member);
    }

    public static FieldSymbol<TField> SetField<TTarget, TField, TValue>(
        this ValueSymbol<TTarget> target, Expression<Func<TTarget, TField>> expression,
        ValueSymbol<TValue> value)
        where TValue : TField
    {
        var field = GetField(target, expression);
        field.Assign(value);
        return field;
    }
}