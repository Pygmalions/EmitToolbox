using System.Diagnostics.Contracts;
using EmitToolbox.Symbols;

namespace EmitToolbox.Extensions;

public static class DictionaryExtensions
{
    extension<TKey, TValue>(ISymbol<IReadOnlyDictionary<TKey, TValue>> self)
    {
        [Pure]
        public IOperationSymbol<TValue> ElementAt(ISymbol<TKey> key)
            => self.Invoke<TValue>(
                typeof(IReadOnlyDictionary<TKey, TValue>).GetMethod("get_Item", [typeof(TKey)])!,
                [key]);

        [Pure]
        public IOperationSymbol<bool> ContainsKey(ISymbol<TKey> key)
            => self.Invoke<bool>(
                typeof(IReadOnlyDictionary<TKey, TValue>).GetMethod(nameof(IReadOnlyDictionary<,>.ContainsKey))!,
                [key]);

        [Pure]
        public IOperationSymbol<bool> TryGetValue(ISymbol<TKey> key, VariableSymbol<TValue> value)
            => self.Invoke<bool>(
                typeof(IReadOnlyDictionary<TKey, TValue>).GetMethod(nameof(IReadOnlyDictionary<,>.TryGetValue))!,
                [key, value]);

        [Pure]
        public IOperationSymbol<IEnumerable<TKey>> Keys
            => self.GetPropertyValue<IEnumerable<TKey>>(
                typeof(IReadOnlyDictionary<TKey, TValue>).GetProperty(nameof(IReadOnlyDictionary<,>.Keys))!);

        [Pure]
        public IOperationSymbol<IEnumerable<TValue>> Values
            => self.GetPropertyValue<IEnumerable<TValue>>(
                typeof(IReadOnlyDictionary<TKey, TValue>).GetProperty(nameof(IReadOnlyDictionary<,>.Values))!);
    }

    extension<TKey, TValue>(ISymbol<IDictionary<TKey, TValue>> self)
    {
        [Pure]
        public IOperationSymbol<TValue> ElementAt(ISymbol<TKey> key)
            => self.Invoke<TValue>(
                typeof(IDictionary<TKey, TValue>).GetMethod("get_Item", [typeof(TKey)])!,
                [key]);

        public void Set(ISymbol<TKey> key, ISymbol<TValue> value)
            => self.Invoke(
                typeof(IDictionary<TKey, TValue>).GetMethod("set_Item",
                    [typeof(TKey), typeof(TValue)])!,
                [key, value]);

        public void Add(ISymbol<TKey> key, ISymbol<TValue> value)
            => self.Invoke(typeof(IDictionary<TKey, TValue>).GetMethod(nameof(IDictionary<,>.Add),
                [typeof(TKey), typeof(TValue)])!, [key, value]);

        public VariableSymbol<bool> Remove(ISymbol<TKey> key)
            => self
                .Invoke<bool>(
                    typeof(IDictionary<TKey, TValue>).GetMethod(nameof(IDictionary<,>.Remove),
                        [typeof(TKey)])!, [key])
                .ToSymbol();

        [Pure]
        public IOperationSymbol<bool> ContainsKey(ISymbol<TKey> key)
            => self.Invoke<bool>(typeof(IDictionary<TKey, TValue>).GetMethod(nameof(IDictionary<,>.ContainsKey))!,
                [key]);

        public VariableSymbol<bool> TryGetValue(ISymbol<TKey> key, IAddressableSymbol<TValue> value)
            => self.Invoke<bool>(typeof(IDictionary<TKey, TValue>)
                        .GetMethod(nameof(IDictionary<,>.TryGetValue))!,
                    [key, value])
                .ToSymbol();

        [Pure]
        public IOperationSymbol<ICollection<TKey>> Keys
            => self.GetPropertyValue<ICollection<TKey>>(
                typeof(IDictionary<TKey, TValue>).GetProperty(nameof(IDictionary<,>.Keys))!);

        [Pure]
        public IOperationSymbol<ICollection<TValue>> Values
            => self.GetPropertyValue<ICollection<TValue>>(
                typeof(IDictionary<TKey, TValue>).GetProperty(nameof(IDictionary<,>.Values))!);
    }
}