using System.Diagnostics.Contracts;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework.Extensions;

public static class SetExtensions
{
    extension<TElement>(ISymbol<IReadOnlySet<TElement>> self)
    {
        [Pure]
        public OperationSymbol<bool> Contains(ISymbol<TElement> item)
            => self.Invoke<bool>(
                typeof(IReadOnlySet<TElement>).GetMethod(nameof(IReadOnlySet<>.Contains))!,
                [item]);

        [Pure]
        public OperationSymbol<bool> IsProperSubsetOf(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(
                typeof(IReadOnlySet<TElement>).GetMethod(nameof(IReadOnlySet<>.IsProperSubsetOf))!,
                [other]);

        [Pure]
        public OperationSymbol<bool> IsProperSupersetOf(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(
                typeof(IReadOnlySet<TElement>).GetMethod(nameof(IReadOnlySet<>.IsProperSupersetOf))!,
                [other]);

        [Pure]
        public OperationSymbol<bool> IsSubsetOf(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(
                typeof(IReadOnlySet<TElement>).GetMethod(nameof(IReadOnlySet<>.IsSubsetOf))!,
                [other]);

        [Pure]
        public OperationSymbol<bool> IsSupersetOf(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(
                typeof(IReadOnlySet<TElement>).GetMethod(nameof(IReadOnlySet<>.IsSupersetOf))!,
                [other]);

        [Pure]
        public OperationSymbol<bool> Overlaps(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(
                typeof(IReadOnlySet<TElement>).GetMethod(nameof(IReadOnlySet<>.Overlaps))!,
                [other]);

        [Pure]
        public OperationSymbol<bool> SetEquals(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(
                typeof(IReadOnlySet<TElement>).GetMethod(nameof(IReadOnlySet<>.SetEquals))!,
                [other]);
    }
    
    extension<TElement>(ISymbol<ISet<TElement>> self)
    {
        public VariableSymbol<bool> Add(ISymbol<TElement> item)
            => self.Invoke<bool>(typeof(ISet<TElement>).GetMethod(nameof(ISet<>.Add))!, [item]).ToSymbol();

        [Pure]
        public OperationSymbol<bool> Contains(ISymbol<TElement> item)
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
        public OperationSymbol<bool> IsProperSubsetOf(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(typeof(ISet<TElement>).GetMethod(nameof(IReadOnlySet<>.IsProperSubsetOf))!, [other]);

        [Pure]
        public OperationSymbol<bool> IsProperSupersetOf(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(typeof(ISet<TElement>).GetMethod(nameof(IReadOnlySet<>.IsProperSupersetOf))!, [other]);

        [Pure]
        public OperationSymbol<bool> IsSubsetOf(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(typeof(ISet<TElement>).GetMethod(nameof(IReadOnlySet<>.IsSubsetOf))!, [other]);

        [Pure]
        public OperationSymbol<bool> IsSupersetOf(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(typeof(ISet<TElement>).GetMethod(nameof(IReadOnlySet<>.IsSupersetOf))!, [other]);

        [Pure]
        public OperationSymbol<bool> Overlaps(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(typeof(ISet<TElement>).GetMethod(nameof(IReadOnlySet<>.Overlaps))!, [other]);

        [Pure]
        public OperationSymbol<bool> SetEquals(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(typeof(ISet<TElement>).GetMethod(nameof(IReadOnlySet<>.SetEquals))!, [other]);
    }
}