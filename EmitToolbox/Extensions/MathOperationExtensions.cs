using System.Diagnostics.Contracts;
using System.Numerics;
using EmitToolbox.Symbols;
using EmitToolbox.Symbols.Operations;
using EmitToolbox.Utilities;

namespace EmitToolbox.Extensions;

public static class MathOperationExtensions
{
    private static MethodInfo GetOperatorMethod<TContent>(string name)
    {
        var contentType = typeof(TContent);
        return contentType.RequireMethod(name, BindingFlags.Public | BindingFlags.Static,
                   [contentType, contentType]);
    }
    
    // Addition
    extension<TContent>(ISymbol<TContent> self)
        where TContent : IAdditionOperators<TContent, TContent, TContent>
    {
        [Pure]
        public static IOperationSymbol<TContent> operator +(ISymbol<TContent> a, ISymbol<TContent> b)
            => Add(a, b);
        
        [Pure]
        public IOperationSymbol<TContent> Add(ISymbol<TContent> other)
        {
            if (typeof(TContent).IsPrimitive)
                return new InstructionOperation<TContent>(OpCodes.Add, [self, other]);
            return new InvocationOperation<TContent>(
                GetOperatorMethod<TContent>("op_Addition"),
                null, [self, other]);
        }

        [Pure]
        public IOperationSymbol<TContent> CheckedAdd(ISymbol<TContent> other)
        {
            if (typeof(TContent).IsPrimitive)
                return new InstructionOperation<TContent>(OpCodes.Add_Ovf, [self, other]);
            return new InvocationOperation<TContent>(
                GetOperatorMethod<TContent>("op_CheckedAddition"),
                null, [self, other]);
        }
    }

    // Subtraction
    extension<TContent>(ISymbol<TContent> self)
        where TContent : ISubtractionOperators<TContent, TContent, TContent>
    {
        [Pure]
        public static IOperationSymbol<TContent> operator -(ISymbol<TContent> a, ISymbol<TContent> b)
            => Subtract(a, b);
        
        [Pure]
        public IOperationSymbol<TContent> Subtract(ISymbol<TContent> other)
        {
            if (typeof(TContent).IsPrimitive)
                return new InstructionOperation<TContent>(OpCodes.Sub, [self, other]);
            return new InvocationOperation<TContent>(
                GetOperatorMethod<TContent>("op_Subtraction"),
                null, [self, other]);
        }

        [Pure]
        public IOperationSymbol<TContent> CheckedSubtract(ISymbol<TContent> other)
        {
            if (typeof(TContent).IsPrimitive)
                return new InstructionOperation<TContent>(OpCodes.Sub_Ovf, [self, other]);
            return new InvocationOperation<TContent>(
                GetOperatorMethod<TContent>("op_CheckedSubtraction"),
                null, [self, other]);
        }
    }

    // Multiplication
    extension<TContent>(ISymbol<TContent> self)
        where TContent : IMultiplyOperators<TContent, TContent, TContent>
    {
        [Pure]
        public static IOperationSymbol<TContent> operator *(ISymbol<TContent> a, ISymbol<TContent> b)
            => Multiply(a, b);
        
        [Pure]
        public IOperationSymbol<TContent> Multiply(ISymbol<TContent> other)
        {
            if (typeof(TContent).IsPrimitive)
                return new InstructionOperation<TContent>(OpCodes.Mul, [self, other]);
            return new InvocationOperation<TContent>(
                GetOperatorMethod<TContent>("op_Multiply"),
                null, [self, other]);
        }

        [Pure]
        public IOperationSymbol<TContent> CheckedMultiply(ISymbol<TContent> other)
        {
            if (typeof(TContent).IsPrimitive)
                return new InstructionOperation<TContent>(OpCodes.Mul_Ovf, [self, other]);
            return new InvocationOperation<TContent>(
                GetOperatorMethod<TContent>("op_CheckedMultiply"),
                null, [self, other]);
        }
    }

    // Division
    extension<TContent>(ISymbol<TContent> self)
        where TContent : IDivisionOperators<TContent, TContent, TContent>
    {
        [Pure]
        public static IOperationSymbol<TContent> operator /(ISymbol<TContent> a, ISymbol<TContent> b)
            => Divide(a, b);
        
        [Pure]
        public IOperationSymbol<TContent> Divide(ISymbol<TContent> other)
        {
            if (typeof(TContent).IsPrimitive)
                return new InstructionOperation<TContent>(OpCodes.Div, [self, other]);
            return new InvocationOperation<TContent>(
                GetOperatorMethod<TContent>("op_Division"),
                null, [self, other]);
        }
    }

    // Modulus
    extension<TContent>(ISymbol<TContent> self)
        where TContent : IModulusOperators<TContent, TContent, TContent>
    {
        [Pure]
        public static IOperationSymbol<TContent> operator %(ISymbol<TContent> a, ISymbol<TContent> b)
            => Modulus(a, b);
        
        [Pure]
        public IOperationSymbol<TContent> Modulus(ISymbol<TContent> other)
        {
            if (typeof(TContent).IsPrimitive)
                return new InstructionOperation<TContent>(OpCodes.Rem, [self, other]);
            return new InvocationOperation<TContent>(
                GetOperatorMethod<TContent>("op_Modulus"),
                null, [self, other]);
        }
    }
    
    // Bitwise Operations
    extension<TContent>(ISymbol<TContent> self)
        where TContent : IBitwiseOperators<TContent, TContent, TContent>
    {
        [Pure]
        public static IOperationSymbol<TContent> operator &(ISymbol<TContent> a, ISymbol<TContent> b)
            => a.BitwiseAnd(b);
        
        [Pure]
        public IOperationSymbol<TContent> BitwiseAnd(ISymbol<TContent> other)
        {
            if (typeof(TContent).IsPrimitive)
                return new InstructionOperation<TContent>(OpCodes.And, [self, other]);
            return new InvocationOperation<TContent>(
                GetOperatorMethod<TContent>("op_BitwiseAnd"),
                null, [self, other]);
        }
        
        [Pure]
        public static IOperationSymbol<TContent> operator |(ISymbol<TContent> a, ISymbol<TContent> b)
            => a.BitwiseOr(b);
        
        [Pure]
        public IOperationSymbol<TContent> BitwiseOr(ISymbol<TContent> other)
        {
            if (typeof(TContent).IsPrimitive)
                return new InstructionOperation<TContent>(OpCodes.Or, [self, other]);
            return new InvocationOperation<TContent>(
                GetOperatorMethod<TContent>("op_BitwiseOr"),
                null, [self, other]);
        }
        
        [Pure]
        public static IOperationSymbol<TContent> operator ^(ISymbol<TContent> a, ISymbol<TContent> b)
            => a.BitwiseXor(b);

        [Pure]
        public IOperationSymbol<TContent> BitwiseXor(ISymbol<TContent> other)
        {
            if (typeof(TContent).IsPrimitive)
                return new InstructionOperation<TContent>(OpCodes.Xor, [self, other]);
            return new InvocationOperation<TContent>(
                GetOperatorMethod<TContent>("op_BitwiseXor"),
                null, [self, other]);
        }
    }
}