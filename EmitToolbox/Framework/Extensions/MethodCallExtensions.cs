using System.Linq.Expressions;
using EmitToolbox.Framework.Facades;
using EmitToolbox.Framework.Symbols;
using EmitToolbox.Framework.Symbols.Operations;
using JetBrains.Annotations;

namespace EmitToolbox.Framework.Extensions;

public static class MethodCallExtensions
{
    // Extension methods for invoking instance methods and accessing instance properties from metadata.
    extension(ISymbol self)
    {
        /// <summary>
        /// Invoke an instance method on this symbol.
        /// </summary>
        /// <param name="method">Instance method to invoke.</param>
        /// <param name="arguments">Arguments to pass to the method.</param>
        /// <returns>
        /// Null if the method returns void, otherwise a variable symbol which holds the return value.
        /// </returns>
        public VariableSymbol? Invoke(MethodDescriptor method, IReadOnlyCollection<ISymbol>? arguments = null)
        {
            var invocation = new InvocationOperation(method, self, arguments ?? []);
            invocation.LoadContent();
            if (method.ReturnType == typeof(void))
                return null;
            var result = new VariableSymbol(self.Context, method.ReturnType);
            result.StoreContent();
            return result;
        }

        [MustUseReturnValue("This operation is emitted when and only when it is evaluated.")]
        public IOperationSymbol<TResult> Invoke<TResult>(
            MethodDescriptor method, IReadOnlyCollection<ISymbol>? arguments = null)
            => new InvocationOperation<TResult>(method, self, arguments ?? []);
        
        [MustUseReturnValue("This operation is emitted when and only when it is evaluated.")]
        public IOperationSymbol<TProperty> GetPropertyValue<TProperty>(PropertyDescriptor property)
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
        public IOperationSymbol<TResult> Invoke<TResult>(
            Expression<Func<TContent, TResult?>> selector, IReadOnlyCollection<ISymbol>? arguments = null)
        {
            return selector.Body is not MethodCallExpression expression
                ? throw new InvalidOperationException("The selector expression is not a method call.")
                : self.Invoke<TResult>(expression.Method, arguments);
        }

        [MustUseReturnValue("This operation is emitted when and only when it is evaluated.")]
        public IOperationSymbol<TProperty> GetPropertyValue<TProperty>(
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
        /// <summary>
        /// Invoke a static method.
        /// </summary>
        /// <param name="method">Static method to invoke.</param>
        /// <param name="arguments">Arguments to pass to the method.</param>
        /// <returns>
        /// Null if the method returns void, otherwise a variable symbol which holds the return value.
        /// </returns>
        public VariableSymbol? Invoke(MethodDescriptor method, IReadOnlyCollection<ISymbol>? arguments = null)
        {
            var invocation = new InvocationOperation(method, null, arguments ?? []);
            invocation.LoadContent();
            if (method.ReturnType == typeof(void))
                return null;
            var result = new VariableSymbol(self, method.ReturnType);
            result.StoreContent();
            return result;
        }

        public VariableSymbol? Invoke(Expression<Action> selector, IReadOnlyCollection<ISymbol>? arguments = null)
        {
            return selector.Body is not MethodCallExpression expression 
                ? throw new InvalidOperationException("The selector expression is not a method call.") 
                : self.Invoke(expression.Method, arguments);
        }

        [MustUseReturnValue("This operation is emitted when and only when it is evaluated.")]
        public IOperationSymbol<TResult> Invoke<TResult>(
            MethodDescriptor method, IReadOnlyCollection<ISymbol>? arguments = null)
            => new InvocationOperation<TResult>(method, null, arguments ?? [], context: self);

        [MustUseReturnValue("This operation is emitted when and only when it is evaluated.")]
        public IOperationSymbol<TResult> Invoke<TResult>(
            Expression<Func<TResult>> selector, IReadOnlyCollection<ISymbol>? arguments = null)
            => selector.Body is not MethodCallExpression expression
                ? throw new InvalidOperationException("The selector expression is not a method call.")
                : self.Invoke<TResult>(expression.Method, arguments ?? []);

        [MustUseReturnValue("This operation is emitted when and only when it is evaluated.")]
        public IOperationSymbol<TProperty> GetPropertyValue<TProperty>(PropertyDescriptor property)
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
        public IOperationSymbol<TProperty> GetPropertyValue<TProperty>(
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