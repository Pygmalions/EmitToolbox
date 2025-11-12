using System.Linq.Expressions;
using EmitToolbox.Framework.Facades;
using EmitToolbox.Framework.Symbols;
using EmitToolbox.Framework.Symbols.Operations;
using JetBrains.Annotations;

namespace EmitToolbox.Framework.Extensions;

public static class MethodCallExtensions
{
    private static VariableSymbol? EmitCallInstruction(
        DynamicFunction context,
        MethodDescriptor descriptor, ISymbol? target,
        IReadOnlyCollection<ISymbol>? arguments,
        bool forceDirectCall = false)
    {
        if (target is null && !descriptor.Method.IsStatic)
            throw new ArgumentException("Cannot invoke an instance method without specifying a target.");
        if (target is not null && descriptor.Method.IsStatic)
            throw new ArgumentException("Cannot invoke a static method on a target instance.");
        if (descriptor.Method.IsAbstract && forceDirectCall)
            throw new ArgumentException(
                "Cannot invoke an abstract method with forcing direct call.");

        if (target != null)
        {
            if (target.Context != context)
                throw new CrossContextException(
                    "The target symbol belongs to a different context other than the specified context.");
            target.LoadAsTarget();
        }

        if (arguments != null)
        {
            if (arguments.Any(symbol => symbol.Context != context))
                throw new CrossContextException(
                    "A argument symbol belongs to a different context other than the specified context.");
            foreach (var (parameter, symbol) in descriptor.ParameterTypes.Zip(arguments))
                symbol.LoadForType(parameter);
        }
        
        switch (descriptor.Method)
        {
            case MethodInfo method:
                context.Code.Emit(
                    forceDirectCall || method.IsStatic || !method.IsVirtual
                        ? OpCodes.Call
                        : OpCodes.Callvirt,
                    method);
                if (method.ReturnType != typeof(void))
                {
                    var result = context.Variable(method.ReturnType);
                    result.StoreContent();
                    return result;
                }
                break;
            case ConstructorInfo constructor:
                context.Code.Emit(OpCodes.Call, constructor);
                break;
        }

        return null;
    }

    // Extension methods for invoking instance methods and accessing instance properties from metadata.
    extension(ISymbol self)
    {
        public VariableSymbol? Invoke(MethodDescriptor method, IReadOnlyCollection<ISymbol>? arguments = null)
            => EmitCallInstruction(self.Context, method, self, arguments);

        [MustUseReturnValue("This operation is emitted when and only when it is evaluated.")]
        public OperationSymbol<TResult> Invoke<TResult>(
            MethodDescriptor method, IReadOnlyCollection<ISymbol>? arguments = null)
            => new InvocationOperation<TResult>(method, self, arguments ?? []);
        
        [MustUseReturnValue("This operation is emitted when and only when it is evaluated.")]
        public OperationSymbol<TProperty> GetPropertyValue<TProperty>(PropertyDescriptor property)
        {
            if (property.Getter is not { Method.IsStatic: false } getter)
                throw new InvalidOperationException(
                    "Cannot get property value: the property is static or does not have a getter.");
            return self.Invoke<TProperty>(getter, []);
        }

        public void SetPropertyValue<TProperty>(PropertyDescriptor property, ISymbol<TProperty> value)
        {
            if (property.Setter is not { Method.IsStatic: false } setter)
                throw new InvalidOperationException(
                    "Cannot get property value: the property is static or does not have a setter.");
            self.Invoke(setter, [value]);
        }
    }

    // Extension methods for invoking instance methods and accessing instance properties from selector expressions.
    extension<TContent>(ISymbol<TContent> self)
    {
        public VariableSymbol? Invoke(Expression<Action<TContent>> selector, IReadOnlyCollection<ISymbol>? arguments = null)
        {
            return selector.Body is not MethodCallExpression expression 
                ? throw new InvalidOperationException("The selector expression is not a method call.") 
                : self.Invoke(expression.Method, arguments);
        }
        
        [MustUseReturnValue("This operation is emitted when and only when it is evaluated.")]
        public OperationSymbol<TResult> Invoke<TResult>(
            Expression<Func<TContent, TResult?>> selector, IReadOnlyCollection<ISymbol>? arguments = null)
        {
            return selector.Body is not MethodCallExpression expression
                ? throw new InvalidOperationException("The selector expression is not a method call.")
                : self.Invoke<TResult>(expression.Method, arguments);
        }

        [MustUseReturnValue("This operation is emitted when and only when it is evaluated.")]
        public OperationSymbol<TProperty> GetPropertyValue<TProperty>(
            Expression<Func<TContent, TProperty?>> selector)
        {
            return selector.Body is not MemberExpression { Member: PropertyInfo property }
                ? throw new InvalidOperationException("The selector expression is not a property access.")
                : self.GetPropertyValue<TProperty>(property);
        }

        public void SetPropertyValue<TProperty>(
            Expression<Func<TContent, TProperty?>> selector, ISymbol<TProperty> value)
        {
            if (selector.Body is not MemberExpression { Member: PropertyInfo property })
                throw new InvalidOperationException("The selector expression is not a property access.");
            self.SetPropertyValue(property, value);
        }
    }

    extension(DynamicFunction self)
    {
        public VariableSymbol? Invoke(MethodDescriptor method, IReadOnlyCollection<ISymbol>? arguments = null)
            => EmitCallInstruction(self, method, null, arguments ?? [], true);

        public VariableSymbol? Invoke(Expression<Action> selector, IReadOnlyCollection<ISymbol>? arguments = null)
        {
            return selector.Body is not MethodCallExpression expression 
                ? throw new InvalidOperationException("The selector expression is not a method call.") 
                : self.Invoke(expression.Method, arguments);
        }

        [MustUseReturnValue("This operation is emitted when and only when it is evaluated.")]
        public OperationSymbol<TResult> Invoke<TResult>(
            MethodDescriptor method, IReadOnlyCollection<ISymbol>? arguments = null)
            => new InvocationOperation<TResult>(method, null, arguments ?? [], context: self);

        [MustUseReturnValue("This operation is emitted when and only when it is evaluated.")]
        public OperationSymbol<TResult> Invoke<TResult>(
            Expression<Func<TResult?>> selector, IReadOnlyCollection<ISymbol>? arguments = null)
            => selector.Body is not MethodCallExpression expression
                ? throw new InvalidOperationException("The selector expression is not a method call.")
                : self.Invoke<TResult>(expression.Method, arguments ?? []);

        [MustUseReturnValue("This operation is emitted when and only when it is evaluated.")]
        public OperationSymbol<TProperty> GetPropertyValue<TProperty>(PropertyDescriptor property)
        {
            if (property.Getter is not { Method.IsStatic: true } getter)
                throw new InvalidOperationException(
                    "Cannot get property value: the property is not static or does not have a getter.");
            return self.Invoke<TProperty>(getter);
        }

        public void SetPropertyValue<TProperty>(PropertyDescriptor property, ISymbol<TProperty> value)
        {
            if (property.Setter is not { Method.IsStatic: true } setter)
                throw new InvalidOperationException(
                    "Cannot get property value: the property is not static or does not have a setter.");
            self.Invoke(setter, [value]);
        }

        [MustUseReturnValue("This operation is emitted when and only when it is evaluated.")]
        public OperationSymbol<TProperty> GetPropertyValue<TProperty>(
            Expression<Func<TProperty?>> selector)
        {
            return selector.Body is not MemberExpression { Member: PropertyInfo property }
                ? throw new InvalidOperationException("The selector expression is not a property access.")
                : self.GetPropertyValue<TProperty>(property);
        }

        public void SetPropertyValue<TProperty>(
            Expression<Func<TProperty?>> selector, ISymbol<TProperty> value)
        {
            if (selector.Body is not MemberExpression { Member: PropertyInfo property })
                throw new InvalidOperationException("The selector expression is not a property access.");
            self.SetPropertyValue(property, value);
        }
    }
}