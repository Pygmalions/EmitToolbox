using System.Diagnostics.Contracts;
using EmitToolbox.Framework.Symbols;
using EmitToolbox.Framework.Symbols.Literals;

namespace EmitToolbox.Framework.Extensions;

public static class ListExtensions
{
    extension<TElement>(ISymbol<IReadOnlyList<TElement>> self)
    {
        [Pure]
        public OperationSymbol<TElement> ElementAt(ISymbol<int> index)
            => self.Invoke<TElement>(
                typeof(IReadOnlyList<TElement>).GetMethod("get_Item", [typeof(int)])!,
                [index]);

        [Pure]
        public OperationSymbol<TElement> ElementAt(int index)
            => self.ElementAt(LiteralSymbolFactory.Create(self.Context, index));
    }
    
    extension<TElement>(ISymbol<IList<TElement>> self)
    {
        [Pure]
        public ItemSymbol<TElement> ElementAt(ISymbol<int> index)
            => new(self, index);

        [Pure]
        public ItemSymbol<TElement> ElementAt(int index)
            => self.ElementAt(LiteralSymbolFactory.Create(self.Context, index));

        [Pure]
        public OperationSymbol<int> IndexOf(ISymbol<TElement> item)
            => self.Invoke<int>(typeof(IList<TElement>).GetMethod(nameof(IList<>.IndexOf))!, [item]);

        public void Insert(ISymbol<int> index, ISymbol<TElement> item)
            => self.Invoke(typeof(IList<TElement>).GetMethod(nameof(IList<>.Insert))!, [index, item]);
        
        public void RemoveAt(ISymbol<int> index)
            => self.Invoke(typeof(IList<TElement>).GetMethod(nameof(IList<>.RemoveAt))!, [index]);
    }
}