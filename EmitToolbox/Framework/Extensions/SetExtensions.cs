using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework.Extensions;

public static class SetExtensions
{
    extension<TElement>(ISymbol<IReadOnlySet<TElement>> self)
    {
        public OperationSymbol<bool> Contains(ISymbol<TElement> item)
            => self.Invoke<bool>(
                typeof(IReadOnlySet<TElement>).GetMethod(nameof(IReadOnlySet<>.Contains))!,
                [item]);

        public OperationSymbol<bool> IsProperSubsetOf(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(
                typeof(IReadOnlySet<TElement>).GetMethod(nameof(IReadOnlySet<>.IsProperSubsetOf))!,
                [other]);

        public OperationSymbol<bool> IsProperSupersetOf(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(
                typeof(IReadOnlySet<TElement>).GetMethod(nameof(IReadOnlySet<>.IsProperSupersetOf))!,
                [other]);

        public OperationSymbol<bool> IsSubsetOf(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(
                typeof(IReadOnlySet<TElement>).GetMethod(nameof(IReadOnlySet<>.IsSubsetOf))!,
                [other]);

        public OperationSymbol<bool> IsSupersetOf(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(
                typeof(IReadOnlySet<TElement>).GetMethod(nameof(IReadOnlySet<>.IsSupersetOf))!,
                [other]);

        public OperationSymbol<bool> Overlaps(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(
                typeof(IReadOnlySet<TElement>).GetMethod(nameof(IReadOnlySet<>.Overlaps))!,
                [other]);

        public OperationSymbol<bool> SetEquals(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(
                typeof(IReadOnlySet<TElement>).GetMethod(nameof(IReadOnlySet<>.SetEquals))!,
                [other]);
    }
    
    extension<TElement>(ISymbol<ISet<TElement>> self)
    {
        public VariableSymbol<bool> Add(ISymbol<TElement> item)
            => self.Invoke<bool>(typeof(ISet<TElement>).GetMethod(nameof(ISet<>.Add))!, [item]).ToSymbol();

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

        public OperationSymbol<bool> IsProperSubsetOf(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(typeof(ISet<TElement>).GetMethod(nameof(IReadOnlySet<>.IsProperSubsetOf))!, [other]);

        public OperationSymbol<bool> IsProperSupersetOf(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(typeof(ISet<TElement>).GetMethod(nameof(IReadOnlySet<>.IsProperSupersetOf))!, [other]);

        public OperationSymbol<bool> IsSubsetOf(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(typeof(ISet<TElement>).GetMethod(nameof(IReadOnlySet<>.IsSubsetOf))!, [other]);

        public OperationSymbol<bool> IsSupersetOf(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(typeof(ISet<TElement>).GetMethod(nameof(IReadOnlySet<>.IsSupersetOf))!, [other]);

        public OperationSymbol<bool> Overlaps(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(typeof(ISet<TElement>).GetMethod(nameof(IReadOnlySet<>.Overlaps))!, [other]);

        public OperationSymbol<bool> SetEquals(ISymbol<IEnumerable<TElement>> other)
            => self.Invoke<bool>(typeof(ISet<TElement>).GetMethod(nameof(IReadOnlySet<>.SetEquals))!, [other]);
    }
}