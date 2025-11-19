using System.Diagnostics.Contracts;
using EmitToolbox.Symbols;

namespace EmitToolbox.Extensions;

public static class SetExtensions
{
    extension<TElement>(ISymbol<IReadOnlySet<TElement>> self)
    {
        [Pure]
        public IOperationSymbol<bool> Contains(ISymbol<TElement> item)
            => self.Invoke<bool>(
                typeof(IReadOnlySet<TElement>).GetMethod(nameof(IReadOnlySet<>.Contains))!,
                [item]);

        [Pure]
        public IOperationSymbol<bool> IsProperSubsetOf(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(
                typeof(IReadOnlySet<TElement>).GetMethod(nameof(IReadOnlySet<>.IsProperSubsetOf))!,
                [other]);

        [Pure]
        public IOperationSymbol<bool> IsProperSupersetOf(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(
                typeof(IReadOnlySet<TElement>).GetMethod(nameof(IReadOnlySet<>.IsProperSupersetOf))!,
                [other]);

        [Pure]
        public IOperationSymbol<bool> IsSubsetOf(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(
                typeof(IReadOnlySet<TElement>).GetMethod(nameof(IReadOnlySet<>.IsSubsetOf))!,
                [other]);

        [Pure]
        public IOperationSymbol<bool> IsSupersetOf(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(
                typeof(IReadOnlySet<TElement>).GetMethod(nameof(IReadOnlySet<>.IsSupersetOf))!,
                [other]);

        [Pure]
        public IOperationSymbol<bool> Overlaps(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(
                typeof(IReadOnlySet<TElement>).GetMethod(nameof(IReadOnlySet<>.Overlaps))!,
                [other]);

        [Pure]
        public IOperationSymbol<bool> SetEquals(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(
                typeof(IReadOnlySet<TElement>).GetMethod(nameof(IReadOnlySet<>.SetEquals))!,
                [other]);
    }
    
    extension<TElement>(ISymbol<ISet<TElement>> self)
    {
        public VariableSymbol<bool> Add(ISymbol<TElement> item)
            => self.Invoke<bool>(typeof(ISet<TElement>).GetMethod(nameof(ISet<>.Add))!, [item]).ToSymbol();

        [Pure]
        public IOperationSymbol<bool> Contains(ISymbol<TElement> item)
            => self.Invoke<bool>(typeof(ISet<TElement>).GetMethod(nameof(IReadOnlySet<>.Contains))!, [item]);
        
        public void UnionWith(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke(typeof(ISet<TElement>).GetMethod(nameof(ISet<>.UnionWith))!, [other]);

        public void IntersectWith(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke(typeof(ISet<TElement>).GetMethod(nameof(ISet<>.IntersectWith))!, [other]);

        public void ExceptWith(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke(typeof(ISet<TElement>).GetMethod(nameof(ISet<>.ExceptWith))!, [other]);

        public void SymmetricExceptWith(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke(typeof(ISet<TElement>).GetMethod(nameof(ISet<>.SymmetricExceptWith))!, [other]);

        [Pure]
        public IOperationSymbol<bool> IsProperSubsetOf(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(typeof(ISet<TElement>).GetMethod(nameof(IReadOnlySet<>.IsProperSubsetOf))!, [other]);

        [Pure]
        public IOperationSymbol<bool> IsProperSupersetOf(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(typeof(ISet<TElement>).GetMethod(nameof(IReadOnlySet<>.IsProperSupersetOf))!, [other]);

        [Pure]
        public IOperationSymbol<bool> IsSubsetOf(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(typeof(ISet<TElement>).GetMethod(nameof(IReadOnlySet<>.IsSubsetOf))!, [other]);

        [Pure]
        public IOperationSymbol<bool> IsSupersetOf(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(typeof(ISet<TElement>).GetMethod(nameof(IReadOnlySet<>.IsSupersetOf))!, [other]);

        [Pure]
        public IOperationSymbol<bool> Overlaps(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(typeof(ISet<TElement>).GetMethod(nameof(IReadOnlySet<>.Overlaps))!, [other]);

        [Pure]
        public IOperationSymbol<bool> SetEquals(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(typeof(ISet<TElement>).GetMethod(nameof(IReadOnlySet<>.SetEquals))!, [other]);
    }
}