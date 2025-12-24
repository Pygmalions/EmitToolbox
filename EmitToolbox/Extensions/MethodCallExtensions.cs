using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using EmitToolbox.Facades;
using EmitToolbox.Symbols;
using EmitToolbox.Symbols.Operations;
using EmitToolbox.Utilities;

namespace EmitToolbox.Extensions;

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

        [Pure]
        public IOperationSymbol<TResult> Invoke<TResult>(
            MethodDescriptor method, IReadOnlyCollection<ISymbol>? arguments = null)
            => new InvocationOperation<TResult>(method, self, arguments ?? []);

        [Pure]
        public IOperationSymbol GetPropertyValue(PropertyDescriptor property)
        {
            if (property.Getter is not { Method.IsStatic: false } getter)
                throw new InvalidOperationException(
                    $"Cannot get property value: the property {property.Property.Name} " +
                    $"is static or does not have a getter.");
            return new NoOperation(self.Invoke(getter, [])!);
        }

        [Pure]
        public IOperationSymbol<TProperty> GetPropertyValue<TProperty>(PropertyDescriptor property)
        {
            if (property.Getter is not { Method.IsStatic: false } getter)
                throw new InvalidOperationException(
                    $"Cannot get property value: the property {property.Property.Name} " +
                    $"is static or does not have a getter.");
            if (!property.PropertyType.BasicType.IsDirectlyAssignableTo(typeof(TProperty)))
                throw new InvalidOperationException(
                    $"Cannot get property value: " +
                    $"the property of type '{property.PropertyType}' is not directly assignable to " +
                    $"the representation type '{typeof(TProperty)}'.");
            return self.Invoke<TProperty>(getter, []);
        }

        public void SetPropertyValue(PropertyDescriptor property, ISymbol value)
        {
            if (property.Setter is not { Method.IsStatic: false } setter)
                throw new InvalidOperationException(
                    $"Cannot set property value: the property '{property.Property.Name}' " +
                    $"is static or does not have a setter.");
            if (!value.BasicType.IsDirectlyAssignableTo(property.PropertyType.BasicType))
                throw new InvalidOperationException(
                    $"Cannot set property value: " +
                    $"the specified value symbol of type '{value.BasicType}' is not directly assignable to " +
                    $"the property of type '{property.PropertyType}'.");
            self.Invoke(setter, [value]);
        }
    }

    // Extension methods for invoking instance methods and accessing instance properties from selector expressions.
    extension<TContent>(ISymbol<TContent> self)
    {
        public VariableSymbol? Invoke(Expression<Action<TContent>> selector,
            IReadOnlyCollection<ISymbol>? arguments = null)
        {
            return selector.Body is not MethodCallExpression expression
                ? throw new InvalidOperationException("The selector expression is not a method call.")
                : self.Invoke(expression.Method, arguments);
        }

        [Pure]
        public IOperationSymbol<TResult> Invoke<TResult>(
            Expression<Func<TContent, TResult?>> selector, IReadOnlyCollection<ISymbol>? arguments = null)
        {
            return selector.Body is not MethodCallExpression expression
                ? throw new InvalidOperationException("The selector expression is not a method call.")
                : self.Invoke<TResult>(expression.Method, arguments);
        }

        [Pure]
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
            var invocation = new InvocationOperation(method, null, arguments ?? [], context: self);
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

        [Pure]
        public IOperationSymbol<TResult> Invoke<TResult>(
            MethodDescriptor method, IReadOnlyCollection<ISymbol>? arguments = null)
            => new InvocationOperation<TResult>(method, null, arguments ?? [], context: self);

        [Pure]
        public IOperationSymbol<TResult> Invoke<TResult>(
            Expression<Func<TResult>> selector, IReadOnlyCollection<ISymbol>? arguments = null)
            => selector.Body is not MethodCallExpression expression
                ? throw new InvalidOperationException("The selector expression is not a method call.")
                : self.Invoke<TResult>(expression.Method, arguments ?? []);

        [Pure]
        public IOperationSymbol GetPropertyValue(PropertyDescriptor property)
        {
            if (property.Getter is not { Method.IsStatic: true } getter)
                throw new InvalidOperationException(
                    $"Cannot get property value: the property {property.Property.Name} " +
                    $"is not static or does not have a getter.");
            return new NoOperation(self.Invoke(getter, [])!);
        }

        [Pure]
        public IOperationSymbol<TProperty> GetPropertyValue<TProperty>(PropertyDescriptor property)
        {
            if (property.Getter is not { Method.IsStatic: true } getter)
                throw new InvalidOperationException(
                    $"Cannot get property value: the property {property.Property.Name} " +
                    $"is not static or does not have a getter.");
            if (!property.PropertyType.BasicType.IsDirectlyAssignableTo(typeof(TProperty)))
                throw new InvalidOperationException(
                    $"Cannot get property value: " +
                    $"the property of type '{property.PropertyType}' is not directly assignable to " +
                    $"the representation type '{typeof(TProperty)}'.");
            return self.Invoke<TProperty>(getter, []);
        }

        public void SetPropertyValue(PropertyDescriptor property, ISymbol value)
        {
            if (property.Setter is not { Method.IsStatic: true } setter)
                throw new InvalidOperationException(
                    $"Cannot set property value: the property '{property.Property.Name}' " +
                    $"is not static or does not have a setter.");
            if (!value.BasicType.IsDirectlyAssignableTo(property.PropertyType.BasicType))
                throw new InvalidOperationException(
                    $"Cannot set property value: " +
                    $"the specified value symbol of type '{value.BasicType}' is not directly assignable to " +
                    $"the property of type '{property.PropertyType}'.");
            self.Invoke(setter, [value]);
        }

        [Pure]
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

    extension<TSymbol>(IEnumerable<TSymbol> self) where TSymbol : ISymbol
    {
        /// <summary>
        /// Load this sequence of symbols as parameters for a method invocation.
        /// </summary>
        /// <param name="parameters">Sequence of parameters.</param>
        /// <param name="context">
        /// Optional context specified for all symbols;
        /// if null, then symbols are only required to be from the same context.</param>
        /// <exception cref="CrossContextException">
        /// Thrown if the symbols are not from the same context.
        /// </exception>
        public void LoadForParameters(IEnumerable<ParameterInfo> parameters, DynamicFunction? context = null)
        {
            foreach (var (symbol, parameter) in self.StrictlyZip(parameters))
            {
                if (context == null)
                    context = symbol.Context;
                else if (symbol.Context != context)
                    throw new CrossContextException(
                        "One or more argument symbols are not from the context of currently building method.");
                symbol.LoadForParameter(parameter);
            }
        }
    }
}