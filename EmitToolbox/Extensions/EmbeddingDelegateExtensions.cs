using EmitToolbox.Symbols;
using EmitToolbox.Symbols.Operations;
using EmitToolbox.Utilities;
using JetBrains.Annotations;
using PureAttribute = System.Diagnostics.Contracts.PureAttribute;

namespace EmitToolbox.Extensions;

public static class EmbeddingDelegateExtensions
{
    extension(DynamicFunction self)
    {
        [Pure]
        public IOperationSymbol<TResult> EmbedStaticDelegate<TResult>(
            [RequireStaticDelegate] Delegate handler,
            params IReadOnlyCollection<ISymbol> arguments)
        {
            if (handler.Method.DeclaringType?.Assembly is {} assembly)
                self.DeclaringType.DeclaringAssembly.IgnoreVisibilityChecksToAssembly(assembly);
            var method = handler.Method;
            if (handler.HasCapturedVariables)
                throw new ArgumentException("Cannot embed non-static delegate.", nameof(handler));
            return new InvocationOperation<TResult>(
                method,
                handler.Target != null ? self.Null(handler.Target!.GetType()) : null,
                arguments);
        }

        public VariableSymbol? EmbedStaticDelegate(
            [RequireStaticDelegate] Delegate handler, 
            params IReadOnlyCollection<ISymbol> arguments)
        {
            if (handler.Method.DeclaringType?.Assembly is {} assembly)
                self.DeclaringType.DeclaringAssembly.IgnoreVisibilityChecksToAssembly(assembly);
            var method = handler.Method;
            if (handler.HasCapturedVariables)
                throw new ArgumentException("Cannot embed non-static delegate.", nameof(handler));
            return handler.Target == null 
                ? self.Invoke(method, arguments) 
                : self.Null(handler.Target.GetType()).Invoke(method, arguments);
        }

        public void EmbedStaticDelegate([RequireStaticDelegate] Action action)
            => self.EmbedStaticDelegate(action, []);

        public void EmbedStaticDelegate<TArgument1>(
            ISymbol<TArgument1> argument1,
            [RequireStaticDelegate] Action<TArgument1> action)
            => self.EmbedStaticDelegate(action, argument1);

        public void EmbedStaticDelegate<TArgument1, TArgument2>(
            ISymbol<TArgument1> argument1,
            ISymbol<TArgument2> argument2,
            [RequireStaticDelegate] Action<TArgument1, TArgument2> action)
            => self.EmbedStaticDelegate(action, argument1, argument2);

        public void EmbedStaticDelegate<TArgument1, TArgument2, TArgument3>(
            ISymbol<TArgument1> argument1,
            ISymbol<TArgument2> argument2,
            ISymbol<TArgument3> argument3,
            [RequireStaticDelegate] Action<TArgument1, TArgument2, TArgument3> action)
            => self.EmbedStaticDelegate(action, argument1, argument2, argument3);

        public void EmbedStaticDelegate<TArgument1, TArgument2, TArgument3, TArgument4>(
            ISymbol<TArgument1> argument1,
            ISymbol<TArgument2> argument2,
            ISymbol<TArgument3> argument3,
            ISymbol<TArgument4> argument4,
            [RequireStaticDelegate] Action<TArgument1, TArgument2, TArgument3, TArgument4> action)
            => self.EmbedStaticDelegate(action, argument1, argument2, argument3, argument4);

        public void EmbedStaticDelegate<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(
            ISymbol<TArgument1> argument1,
            ISymbol<TArgument2> argument2,
            ISymbol<TArgument3> argument3,
            ISymbol<TArgument4> argument4,
            ISymbol<TArgument5> argument5,
            [RequireStaticDelegate] Action<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5> action)
            => self.EmbedStaticDelegate(action, argument1, argument2, argument3, argument4, argument5);

        [Pure]
        public IOperationSymbol<TResult> EmbedStaticDelegate<TResult>([RequireStaticDelegate] Func<TResult> functor)
            => self.EmbedStaticDelegate<TResult>(functor, []);

        [Pure]
        public IOperationSymbol<TResult> EmbedStaticDelegate<TArgument1, TResult>(
            ISymbol<TArgument1> argument1,
            [RequireStaticDelegate] Func<TArgument1, TResult> functor)
            => self.EmbedStaticDelegate<TResult>(functor, argument1);

        [Pure]
        public IOperationSymbol<TResult> EmbedStaticDelegate<TArgument1, TArgument2, TResult>(
            ISymbol<TArgument1> argument1,
            ISymbol<TArgument2> argument2,
            [RequireStaticDelegate] Func<TArgument1, TArgument2, TResult> functor)
            => self.EmbedStaticDelegate<TResult>(functor, argument1, argument2);

        [Pure]
        public IOperationSymbol<TResult> EmbedStaticDelegate<TArgument1, TArgument2, TArgument3, TResult>(
            ISymbol<TArgument1> argument1,
            ISymbol<TArgument2> argument2,
            ISymbol<TArgument3> argument3,
            [RequireStaticDelegate] Func<TArgument1, TArgument2, TArgument3, TResult> functor)
            => self.EmbedStaticDelegate<TResult>(functor, argument1, argument2, argument3);

        [Pure]
        public IOperationSymbol<TResult> EmbedStaticDelegate<TArgument1, TArgument2, TArgument3, TArgument4, TResult>(
            ISymbol<TArgument1> argument1,
            ISymbol<TArgument2> argument2,
            ISymbol<TArgument3> argument3,
            ISymbol<TArgument4> argument4,
            [RequireStaticDelegate] Func<TArgument1, TArgument2, TArgument3, TArgument4, TResult> functor)
            => self.EmbedStaticDelegate<TResult>(functor, argument1, argument2, argument3, argument4);

        [Pure]
        public IOperationSymbol<TResult> EmbedStaticDelegate<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TResult>(
            ISymbol<TArgument1> argument1,
            ISymbol<TArgument2> argument2,
            ISymbol<TArgument3> argument3,
            ISymbol<TArgument4> argument4,
            ISymbol<TArgument5> argument5,
            [RequireStaticDelegate] Func<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TResult> functor)
            => self.EmbedStaticDelegate<TResult>(functor, argument1, argument2, argument3, argument4, argument5);
    }
}