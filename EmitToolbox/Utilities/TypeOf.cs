using System.Linq.Expressions;

namespace EmitToolbox.Utilities;

public static class TypeOf<TType>
{
    public static ConstructorInfo Constructor(Expression<Func<TType>> selector)
    {
        return selector.Body is not NewExpression {Constructor: { } constructor}
            ? throw new ArgumentException(
                "The selector expression is not a 'new' expression or the constructor is null.", nameof(selector))
            : constructor;
    }

    public static MethodInfo Method(Expression<Action<TType>> selector)
    {
        return selector.Body is not MethodCallExpression expression
            ? throw new ArgumentException("The selector expression is not a method call.", nameof(selector))
            : expression.Method;
    }

    public static MethodInfo Method<TResult>(Expression<Func<TType, TResult>> selector)
    {
        return selector.Body is not MethodCallExpression expression
            ? throw new ArgumentException("The selector expression is not a method call.", nameof(selector))
            : expression.Method;
    }

    public static FieldInfo Field<TField>(Expression<Action<TType, TField>> selector)
    {
        return selector.Body is not MemberExpression { Member: FieldInfo field }
            ? throw new ArgumentException("The selector expression is not a field access.", nameof(selector))
            : field;
    }

    public static PropertyInfo Property<TProperty>(Expression<Func<TType, TProperty>> selector)
    {
        return selector.Body is not MemberExpression { Member: PropertyInfo property }
            ? throw new ArgumentException("The selector expression is not a property access.", nameof(selector))
            : property;
    }
}