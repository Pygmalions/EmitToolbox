using System.Diagnostics.Contracts;
using EmitToolbox.Symbols;
using EmitToolbox.Symbols.Operations;
using EmitToolbox.Utilities;

namespace EmitToolbox.Extensions;

public static class StringExtensions
{
    extension(ISymbol self)
    {
        [Pure]
        public IOperationSymbol<string> ToString()
            => self.Invoke<string>(
                self.BasicType.GetMethod(nameof(ToString), Type.EmptyTypes)!, []);
    }

    extension(ISymbol<string> self)
    {
        [Pure]
        public static IOperationSymbol<string> operator +(ISymbol<string> a, ISymbol<string> b)
        {
            var context = CrossContextException.EnsureContext(a, b);
            return context.Invoke(() => string.Concat(Any<string>.Value, Any<string>.Value),
                [a, b]);
        }
        
        [Pure]
        public IOperationSymbol<bool> IsEqualTo(ISymbol<string> literal)
            => new InvocationOperation<bool>(
                typeof(string).GetMethod(nameof(string.Equals), [typeof(string)])!, 
                self, [literal]);

        [Pure]
        public IOperationSymbol<bool> IsNotEqualTo(ISymbol<string> literal)
            => self.IsEqualTo(literal).Not();
    }
}