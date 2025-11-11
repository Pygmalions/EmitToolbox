using System.Numerics;
using EmitToolbox.Framework.Symbols;
using EmitToolbox.Framework.Symbols.Operations;
using EmitToolbox.Framework.Utilities;

namespace EmitToolbox.Framework.Extensions;

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
        public static OperationSymbol<TContent> operator +(ISymbol<TContent> a, ISymbol<TContent> b)
            => Add(a, b);
        
        public OperationSymbol<TContent> Add(ISymbol<TContent> other)
        {
            if (typeof(TContent).IsPrimitive)
                return new InstructionOperation<TContent>(OpCodes.Add, [self, other]);
            return new InvocationOperation<TContent>(
                GetOperatorMethod<TContent>("op_Addition"),
                null, [self, other]);
        }

        public OperationSymbol<TContent> CheckedAdd(ISymbol<TContent> other)
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
        public static OperationSymbol<TContent> operator -(ISymbol<TContent> a, ISymbol<TContent> b)
            => Subtract(a, b);
        public OperationSymbol<TContent> Subtract(ISymbol<TContent> other)
        {
            if (typeof(TContent).IsPrimitive)
                return new InstructionOperation<TContent>(OpCodes.Sub, [self, other]);
            return new InvocationOperation<TContent>(
                GetOperatorMethod<TContent>("op_Subtraction"),
                null, [self, other]);
        }

        public OperationSymbol<TContent> CheckedSubtract(ISymbol<TContent> other)
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
        public static OperationSymbol<TContent> operator *(ISymbol<TContent> a, ISymbol<TContent> b)
            => Multiply(a, b);
        
        public OperationSymbol<TContent> Multiply(ISymbol<TContent> other)
        {
            if (typeof(TContent).IsPrimitive)
                return new InstructionOperation<TContent>(OpCodes.Mul, [self, other]);
            return new InvocationOperation<TContent>(
                GetOperatorMethod<TContent>("op_Multiply"),
                null, [self, other]);
        }

        public OperationSymbol<TContent> CheckedMultiply(ISymbol<TContent> other)
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
        public static OperationSymbol<TContent> operator /(ISymbol<TContent> a, ISymbol<TContent> b)
            => Divide(a, b);
        
        public OperationSymbol<TContent> Divide(ISymbol<TContent> other)
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
        public static OperationSymbol<TContent> operator %(ISymbol<TContent> a, ISymbol<TContent> b)
            => Modulus(a, b);
        
        public OperationSymbol<TContent> Modulus(ISymbol<TContent> other)
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
        public static OperationSymbol<TContent> operator &(ISymbol<TContent> a, ISymbol<TContent> b)
            => a.BitwiseAnd(b);
        
        public OperationSymbol<TContent> BitwiseAnd(ISymbol<TContent> other)
        {
            if (typeof(TContent).IsPrimitive)
                return new InstructionOperation<TContent>(OpCodes.And, [self, other]);
            return new InvocationOperation<TContent>(
                GetOperatorMethod<TContent>("op_BitwiseAnd"),
                null, [self, other]);
        }
        
        public static OperationSymbol<TContent> operator |(ISymbol<TContent> a, ISymbol<TContent> b)
            => a.BitwiseOr(b);
        
        public OperationSymbol<TContent> BitwiseOr(ISymbol<TContent> other)
        {
            if (typeof(TContent).IsPrimitive)
                return new InstructionOperation<TContent>(OpCodes.Or, [self, other]);
            return new InvocationOperation<TContent>(
                GetOperatorMethod<TContent>("op_BitwiseOr"),
                null, [self, other]);
        }
        
        public static OperationSymbol<TContent> operator ^(ISymbol<TContent> a, ISymbol<TContent> b)
            => a.BitwiseXor(b);

        public OperationSymbol<TContent> BitwiseXor(ISymbol<TContent> other)
        {
            if (typeof(TContent).IsPrimitive)
                return new InstructionOperation<TContent>(OpCodes.Xor, [self, other]);
            return new InvocationOperation<TContent>(
                GetOperatorMethod<TContent>("op_BitwiseXor"),
                null, [self, other]);
        }
    }
}