using System.Diagnostics.Contracts;
using System.Numerics;
using EmitToolbox.Framework.Symbols;
using EmitToolbox.Framework.Symbols.Operations;
using EmitToolbox.Framework.Utilities;

namespace EmitToolbox.Framework.Extensions;

public static class ComparisonExtensions
{
    [Pure]
    public static OperationSymbol<int> CompareTo<TSelfContent>
        (this ISymbol<TSelfContent> self, ISymbol other)
        where TSelfContent : IComparable, allows ref struct
    {
        var basicType = self.BasicType;
        if (basicType.GetMethod(nameof(IComparable.CompareTo), [other.BasicType])
            is { } specializedMethod)
            return new InvocationOperation<int>(specializedMethod, self, [other]);
        return new InvocationOperation<int>(
            typeof(IComparable).RequireMethod(
                nameof(IComparable.CompareTo), [typeof(object)]),
            self, [other.ToObject()]);
    }

    [Pure]
    public static OperationSymbol<int> CompareTo<TSelfContent, TOtherContent>
        (this ISymbol<TSelfContent> self, ISymbol<TOtherContent> other)
        where TSelfContent : IComparable<TOtherContent>, allows ref struct
    {
        return new InvocationOperation<int>(
            typeof(TSelfContent)
                .RequireMethod(nameof(IComparable<>.CompareTo),
                    BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.Instance | BindingFlags.FlattenHierarchy,
                    [other.BasicType]),
            self, [other]);
    }

    extension<TSelfContent, TOtherContent>(ISymbol<TSelfContent> self)
        where TSelfContent : IComparisonOperators<TSelfContent, TOtherContent, bool>
    {
        private static MethodInfo GetOperatorMethod(string name)
            => typeof(IComparisonOperators<TSelfContent, TOtherContent, bool>)
                .RequireMethod(name, BindingFlags.Public | BindingFlags.Static,
                    [typeof(TSelfContent), typeof(TOtherContent)]);

        [Pure]
        public static OperationSymbol<bool> operator >(ISymbol<TSelfContent> a, ISymbol<TOtherContent> b)
        {
            if (typeof(TSelfContent).IsPrimitive && typeof(TOtherContent).IsPrimitive)
            {
                return new InstructionOperation<bool>(
                    PrimitiveTypeMetadata<TSelfContent>.IsUnsigned.Value ? OpCodes.Cgt_Un : OpCodes.Cgt,
                    [a, b]);
            }

            return new InvocationOperation<bool>(
                GetOperatorMethod<TSelfContent, TOtherContent>("op_GreaterThan"),
                null, [a, b]);
        }

        [Pure]
        public static OperationSymbol<bool> operator <(ISymbol<TSelfContent> a, ISymbol<TOtherContent> b)
        {
            if (typeof(TSelfContent).IsPrimitive && typeof(TOtherContent).IsPrimitive)
            {
                return new InstructionOperation<bool>(
                    PrimitiveTypeMetadata<TSelfContent>.IsUnsigned.Value ? OpCodes.Clt_Un : OpCodes.Clt,
                    [a, b]);
            }

            return new InvocationOperation<bool>(
                GetOperatorMethod<TSelfContent, TOtherContent>("op_LessThan"),
                null, [a, b]);
        }

        [Pure]
        public static OperationSymbol<bool> operator >=(ISymbol<TSelfContent> a, ISymbol<TOtherContent> b)
        {
            if (typeof(TSelfContent).IsPrimitive && typeof(TOtherContent).IsPrimitive)
            {
                return new InstructionOperation<bool>(
                    PrimitiveTypeMetadata<TSelfContent>.IsUnsigned.Value ? OpCodes.Clt_Un : OpCodes.Clt,
                    [a, b]).Not();
            }

            return new InvocationOperation<bool>(
                GetOperatorMethod<TSelfContent, TOtherContent>("op_GreaterThanOrEqual"),
                null, [a, b]);
        }

        [Pure]
        public static OperationSymbol<bool> operator <=(ISymbol<TSelfContent> a, ISymbol<TOtherContent> b)
        {
            if (typeof(TSelfContent).IsPrimitive && typeof(TOtherContent).IsPrimitive)
            {
                return new InstructionOperation<bool>(
                    PrimitiveTypeMetadata<TSelfContent>.IsUnsigned.Value ? OpCodes.Cgt_Un : OpCodes.Cgt,
                    [a, b]).Not();
            }

            return new InvocationOperation<bool>(
                GetOperatorMethod<TSelfContent, TOtherContent>("op_LessThanOrEqual"),
                null, [a, b]);
        }

        /// <summary>
        /// Compares this symbol with another symbol using the '==' operator.
        /// </summary>
        /// <param name="other">Another symbol to compare to.</param>
        /// <returns>Comparison result.</returns>
        [Pure]
        public OperationSymbol<bool> IsEqualTo(ISymbol<TOtherContent> other)
        {
            if (typeof(TSelfContent).IsPrimitive && typeof(TOtherContent).IsPrimitive)
                return new InstructionOperation<bool>(OpCodes.Ceq, [self, other]);

            return new InvocationOperation<bool>(
                GetOperatorMethod<TSelfContent, TOtherContent>("op_Equality"),
                null, [self, other]);
        }

        /// <summary>
        /// Compares this symbol with another symbol using the '!=' operator.
        /// </summary>
        /// <param name="other">Another symbol to compare to.</param>
        /// <returns>Comparison result.</returns>
        [Pure]
        public OperationSymbol<bool> IsNotEqualTo(ISymbol<TOtherContent> other)
        {
            if (typeof(TSelfContent).IsPrimitive && typeof(TOtherContent).IsPrimitive)
                return new InstructionOperation<bool>(OpCodes.Ceq, [self, other]).Not();

            return new InvocationOperation<bool>(
                GetOperatorMethod<TSelfContent, TOtherContent>("op_Inequality"),
                null, [self, other]);
        }
    }
}