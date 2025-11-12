using System.Diagnostics.Contracts;
using EmitToolbox.Framework.Symbols;
using EmitToolbox.Framework.Symbols.Literals;

namespace EmitToolbox.Framework.Extensions;

public static class CollectionExtensions
{
    extension<TElement>(ISymbol<IReadOnlyCollection<TElement>> self)
    {
        [Pure]
        public OperationSymbol<int> Length => self.GetPropertyValue<int>(
            typeof(IReadOnlyCollection<TElement>)
                .GetProperty(nameof(IReadOnlyCollection<>.Count))!);
    }

    extension<TElement>(ISymbol<ICollection<TElement>> self)
    {
        public void Add(ISymbol<TElement> item)
            => self.Invoke(
                typeof(ICollection<TElement>).GetMethod(nameof(ICollection<>.Add))!,
                [item]);

        public void Clear()
            => self.Invoke(typeof(ICollection<TElement>).GetMethod(nameof(ICollection<>.Clear))!);

        [Pure]
        public OperationSymbol<bool> Contains(ISymbol<TElement> item)
            => self.Invoke<bool>(
                typeof(ICollection<TElement>).GetMethod(nameof(ICollection<>.Contains))!,
                [item]);

        public void CopyTo(ISymbol<TElement[]> array, ISymbol<int> arrayIndex)
            => self.Invoke(
                typeof(ICollection<TElement>).GetMethod(nameof(ICollection<>.CopyTo))!,
                [array, arrayIndex]);

        public void CopyTo(ISymbol<TElement[]> array, int arrayIndex)
            => self.CopyTo(array, new LiteralInteger32Symbol(self.Context, arrayIndex));

        public VariableSymbol<bool> Remove(ISymbol<TElement> item)
            => self
                .Invoke<bool>(
                    typeof(ICollection<TElement>).GetMethod(nameof(ICollection<>.Remove))!,
                    [item])
                .ToSymbol();
    }
}