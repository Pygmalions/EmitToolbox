using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework.Extensions;

public static class CollectionExtensions
{
    extension<TElement>(ISymbol<IReadOnlyCollection<TElement>> self)
    {
        public OperationSymbol<int> Length => self.GetPropertyValue<int>(
            typeof(IReadOnlyCollection<TElement>)
                .GetProperty(nameof(IReadOnlyCollection<TElement>.Count))!);
    }

    extension<TElement>(ISymbol<ICollection<TElement>> self)
    {
        public void Add(ISymbol<TElement> item)
            => self.Invoke(
                typeof(ICollection<TElement>).GetMethod(nameof(ICollection<>.Add))!,
                [item]);

        public void Clear()
            => self.Invoke(typeof(ICollection<TElement>).GetMethod(nameof(ICollection<>.Clear))!);

        public OperationSymbol<bool> Contains(ISymbol<TElement> item)
            => self.Invoke<bool>(
                typeof(ICollection<TElement>).GetMethod(nameof(ICollection<>.Contains))!,
                [item]);

        public void CopyTo(ISymbol<TElement[]> array, ISymbol<int> arrayIndex)
            => self.Invoke(
                typeof(ICollection<TElement>).GetMethod(nameof(ICollection<>.CopyTo))!,
                [array, arrayIndex]);

        public VariableSymbol<bool> Remove(ISymbol<TElement> item)
            => self
                .Invoke<bool>(
                    typeof(ICollection<TElement>).GetMethod(nameof(ICollection<>.Remove))!,
                    [item])
                .ToSymbol();
    }
}