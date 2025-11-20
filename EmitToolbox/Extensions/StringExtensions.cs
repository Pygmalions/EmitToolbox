using EmitToolbox.Symbols;
using EmitToolbox.Symbols.Literals;
using EmitToolbox.Symbols.Operations;
using EmitToolbox.Utilities;
using JetBrains.Annotations;

namespace EmitToolbox.Extensions;

public static class StringExtensions
{
    extension(ISymbol self)
    {
        [System.Diagnostics.Contracts.Pure]
        public IOperationSymbol<string> ToString()
            => self.Invoke<string>(
                self.BasicType.GetMethod(nameof(ToString), Type.EmptyTypes)!, []);
    }

    extension(ISymbol<string> self)
    {
        [System.Diagnostics.Contracts.Pure]
        public static IOperationSymbol<string> operator +(ISymbol<string> a, ISymbol<string> b)
        {
            var context = CrossContextException.EnsureContext(a, b);
            return context.Invoke(() => string.Concat(Any<string>.Value, Any<string>.Value),
                [a, b]);
        }

        [System.Diagnostics.Contracts.Pure]
        public IOperationSymbol<bool> IsEqualTo(ISymbol<string> literal)
            => new InvocationOperation<bool>(
                typeof(string).GetMethod(nameof(string.Equals), [typeof(string)])!,
                self, [literal]);

        [System.Diagnostics.Contracts.Pure]
        public IOperationSymbol<bool> IsNotEqualTo(ISymbol<string> literal)
            => self.IsEqualTo(literal).Not();

        [System.Diagnostics.Contracts.Pure]
        public IOperationSymbol<string> Format(IReadOnlyCollection<ISymbol<object?>> arguments)
        {
            var symbolArguments = self.Context.NewArray<object?>(arguments.Count);
            foreach (var (index, argument) in arguments.Index())
                symbolArguments.ElementAt(index).AssignContent(argument);
            return self.Context.Invoke(
                () => string.Format(Any<string>.Value, Any<object?[]>.Value),
                [self, symbolArguments]);
        }
        
        [System.Diagnostics.Contracts.Pure]
        public IOperationSymbol<string> Format(IReadOnlyCollection<ISymbol> arguments)
        {
            var symbolArguments = self.Context.NewArray<object?>(arguments.Count);
            foreach (var (index, argument) in arguments.Index())
                symbolArguments.ElementAt(index).AssignContent(argument.ToObject());
            return self.Context.Invoke(
                () => string.Format(Any<string>.Value, Any<object?[]>.Value),
                [self, symbolArguments]);
        }
    }

    extension(DynamicFunction self)
    {
        [System.Diagnostics.Contracts.Pure]
        public IOperationSymbol<string> FormatString(
            [StructuredMessageTemplate] string format, IReadOnlyCollection<ISymbol<object?>> arguments)
            => new LiteralStringSymbol(self, format).Format(arguments);
        
        [System.Diagnostics.Contracts.Pure]
        public IOperationSymbol<string> FormatString(
            [StructuredMessageTemplate] string format, IReadOnlyCollection<ISymbol> arguments)
            => new LiteralStringSymbol(self, format).Format(arguments);
    }
}