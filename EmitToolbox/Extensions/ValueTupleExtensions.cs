using System.Diagnostics.Contracts;
using EmitToolbox.Symbols;
using EmitToolbox.Symbols.Operations;

namespace EmitToolbox.Extensions;

public static class ValueTupleExtensions
{
    extension(DynamicFunction self)
    {
        [Pure]
        public IOperationSymbol<ValueTuple<T1, T2>> NewValueTuple<T1, T2>(
            ISymbol<T1> item1, ISymbol<T2> item2)
        {
            return new InstantiationOperation<(T1, T2)>(
                typeof(ValueTuple<T1, T2>).GetConstructor(
                    [typeof(T1), typeof(T2)])!,
                [item1, item2]);
        }

        [Pure]
        public IOperationSymbol<ValueTuple<T1, T2, T3>> NewValueTuple<T1, T2, T3>(
            ISymbol<T1> item1, ISymbol<T2> item2, ISymbol<T3> item3)
        {
            return new InstantiationOperation<(T1, T2, T3)>(
                typeof(ValueTuple<T1, T2, T3>).GetConstructor(
                    [typeof(T1), typeof(T2), typeof(T3)])!,
                [item1, item2, item3]);
        }

        [Pure]
        public IOperationSymbol<ValueTuple<T1, T2, T3, T4>> NewValueTuple<T1, T2, T3, T4>(
            ISymbol<T1> item1, ISymbol<T2> item2, ISymbol<T3> item3, ISymbol<T4> item4)
        {
            return new InstantiationOperation<(T1, T2, T3, T4)>(
                typeof(ValueTuple<T1, T2, T3, T4>).GetConstructor(
                    [typeof(T1), typeof(T2), typeof(T3), typeof(T4)])!,
                [item1, item2, item3, item4]);
        }

        [Pure]
        public IOperationSymbol<ValueTuple<T1, T2, T3, T4, T5>> NewValueTuple<T1, T2, T3, T4, T5>(
            ISymbol<T1> item1, ISymbol<T2> item2, ISymbol<T3> item3, ISymbol<T4> item4, ISymbol<T5> item5)
        {
            return new InstantiationOperation<(T1, T2, T3, T4, T5)>(
                typeof(ValueTuple<T1, T2, T3, T4, T5>).GetConstructor([
                    typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)
                ])!,
                [item1, item2, item3, item4, item5]);
        }
    }
}