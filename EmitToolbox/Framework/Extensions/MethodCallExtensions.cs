using System.Linq.Expressions;
using EmitToolbox.Framework.Facades;
using EmitToolbox.Framework.Symbols;
using EmitToolbox.Framework.Symbols.Operations;

namespace EmitToolbox.Framework.Extensions;

public static class MethodCallExtensions
{
    private static void EmitCallInstruction(
        DynamicMethod context,
        MethodFacade facade, ISymbol? target,
        IReadOnlyCollection<ISymbol> arguments,
        bool forceDirectCall = false)
    {
        if (target is null && !facade.Method.IsStatic)
            throw new ArgumentException("Cannot invoke an instance method without specifying a target.");
        if (target is not null && facade.Method.IsStatic)
            throw new ArgumentException("Cannot invoke a static method on a target instance.");
        if (facade.Method.IsAbstract && forceDirectCall)
            throw new ArgumentException(
                "Cannot invoke an abstract method with forcing direct call.");
        
        if (target != null)
        {
            if (target.Context != context)
                throw new CrossContextException(
                    "The target symbol belongs to a different context other than the specified context.");
            target.EmitAsTarget();
        }

        if (arguments.Any(symbol => symbol.Context != context))
            throw new CrossContextException(
                "A argument symbol belongs to a different context other than the specified context.");

        foreach (var (parameter, symbol) in facade.ParameterTypes.Zip(arguments))
            symbol.EmitForType(parameter);

        switch (facade.Method)
        {
            case MethodInfo method:
                context.Code.Emit(
                    forceDirectCall || method.IsStatic || !method.IsVirtual
                        ? OpCodes.Call
                        : OpCodes.Callvirt,
                    method);
                break;
            case ConstructorInfo constructor:
                context.Code.Emit(OpCodes.Call, constructor);
                break;
        }
    }

    // Extension methods for invoking instance methods and accessing instance properties from metadata.
    extension(ISymbol self)
    {
        public void Invoke(MethodFacade method, params IReadOnlyCollection<ISymbol> arguments)
            => EmitCallInstruction(self.Context, method, self, arguments);

        public OperationSymbol<TResult> Invoke<TResult>(
            MethodFacade method, params IReadOnlyCollection<ISymbol> arguments)
            => new InvocationOperation<TResult>(method, self, arguments);
        
        public OperationSymbol<TProperty> GetPropertyValue<TProperty>(PropertyFacade property)
        {
            if (property.Getter is not { Method.IsStatic: false } getter)
                throw new InvalidOperationException(
                    "Cannot get property value: the property is static or does not have a getter.");
            return self.Invoke<TProperty>(getter);
        }

        public void SetPropertyValue<TProperty>(PropertyFacade property, ISymbol<TProperty> value)
        {
            if (property.Setter is not { Method.IsStatic: false } setter)
                throw new InvalidOperationException(
                    "Cannot get property value: the property is static or does not have a setter.");
            self.Invoke(setter, value);
        }
    }

    // Extension methods for invoking instance methods and accessing instance properties from selector expressions.
    extension<TContent>(ISymbol<TContent> self)
    {
        public void Invoke(Expression<Action<TContent>> selector, params IReadOnlyCollection<ISymbol> arguments)
        {
            if (selector.Body is not MethodCallExpression expression)
                throw new InvalidOperationException("The selector expression is not a method call.");
            self.Invoke(expression.Method, arguments);
        }

        public OperationSymbol<TResult> Invoke<TResult>(
            Expression<Func<TContent, TResult>> selector, params IReadOnlyCollection<ISymbol> arguments)
        {
            return selector.Body is not MethodCallExpression expression
                ? throw new InvalidOperationException("The selector expression is not a method call.")
                : self.Invoke<TResult>(expression.Method, arguments);
        }

        public OperationSymbol<TProperty> GetPropertyValue<TProperty>(
            Expression<Func<TContent, TProperty>> selector)
        {
            return selector.Body is not MemberExpression { Member: PropertyInfo property }
                ? throw new InvalidOperationException("The selector expression is not a property access.")
                : self.GetPropertyValue<TProperty>(property);
        }

        public void SetPropertyValue<TProperty>(
            Expression<Func<TContent, TProperty>> selector, ISymbol<TProperty> value)
        {
            if (selector.Body is not MemberExpression { Member: PropertyInfo property })
                throw new InvalidOperationException("The selector expression is not a property access.");
            self.SetPropertyValue(property, value);
        }
    }

    extension(DynamicMethod self)
    {
        public void Invoke(MethodFacade method, params IReadOnlyCollection<ISymbol> arguments)
            => EmitCallInstruction(self, method, null, arguments, true);

        public void Invoke(Expression<Action> selector, params IReadOnlyCollection<ISymbol> arguments)
        {
            if (selector.Body is not MethodCallExpression expression)
                throw new InvalidOperationException("The selector expression is not a method call.");
            self.Invoke(expression.Method, arguments);
        }

        public OperationSymbol<TResult> Invoke<TResult>(
            MethodFacade method, params IReadOnlyCollection<ISymbol> arguments)
            => new InvocationOperation<TResult>(method, null, arguments, context: self);

        public OperationSymbol<TResult> Invoke<TResult>(
            Expression<Func<TResult>> selector, params IReadOnlyCollection<ISymbol> arguments)
            => selector.Body is not MethodCallExpression expression
                ? throw new InvalidOperationException("The selector expression is not a method call.")
                : self.Invoke<TResult>(expression.Method, arguments);

        public OperationSymbol<TProperty> GetPropertyValue<TProperty>(PropertyFacade property)
        {
            if (property.Getter is not { Method.IsStatic: true } getter)
                throw new InvalidOperationException(
                    "Cannot get property value: the property is not static or does not have a getter.");
            return self.Invoke<TProperty>(getter);
        }

        public void SetPropertyValue<TProperty>(PropertyFacade property, ISymbol<TProperty> value)
        {
            if (property.Setter is not { Method.IsStatic: true } setter)
                throw new InvalidOperationException(
                    "Cannot get property value: the property is not static or does not have a setter.");
            self.Invoke(setter, value);
        }
        
        public OperationSymbol<TProperty> GetPropertyValue<TProperty>(
            Expression<Func<TProperty>> selector)
        {
            return selector.Body is not MemberExpression { Member: PropertyInfo property }
                ? throw new InvalidOperationException("The selector expression is not a property access.")
                : self.GetPropertyValue<TProperty>(property);
        }

        public void SetPropertyValue<TProperty>(
            Expression<Func<TProperty>> selector, ISymbol<TProperty> value)
        {
            if (selector.Body is not MemberExpression { Member: PropertyInfo property })
                throw new InvalidOperationException("The selector expression is not a property access.");
            self.SetPropertyValue(property, value);
        }
    }
}