using System.Diagnostics.Contracts;
using System.Numerics;
using EmitToolbox.Symbols;
using EmitToolbox.Symbols.Literals;
using EmitToolbox.Symbols.Operations;
using EmitToolbox.Utilities;

namespace EmitToolbox.Extensions;

public static class LiteralValueExtensions
{
    extension<TLiteral>(IAssignableSymbol<TLiteral> self) where TLiteral : unmanaged, INumber<TLiteral>
    {
        /// <summary>
        /// Assign a literal value to this symbol.
        /// </summary>
        /// <param name="literal">Literal value to assign to this symbol.</param>
        public void AssignValue(TLiteral literal)
        {
            self.AssignValue(LiteralSymbolFactory.Create(self.Context, literal));
        }
    }
    
    extension<TLiteral>(ISymbol<TLiteral>) where TLiteral : unmanaged, INumber<TLiteral>
    {
        [Pure]
        public static IOperationSymbol<TLiteral> operator +(ISymbol<TLiteral> a, TLiteral b)
            => new InstructionOperation<TLiteral>(OpCodes.Add, 
                [a, LiteralSymbolFactory.Create(a.Context, b)]);
        
        [Pure]
        public static IOperationSymbol<TLiteral> operator -(ISymbol<TLiteral> a, TLiteral b)
            => new InstructionOperation<TLiteral>(OpCodes.Sub, 
                [a, LiteralSymbolFactory.Create(a.Context, b)]);
        
        [Pure]
        public static IOperationSymbol<TLiteral> operator *(ISymbol<TLiteral> a, TLiteral b)
            => new InstructionOperation<TLiteral>(OpCodes.Mul, 
                [a, LiteralSymbolFactory.Create(a.Context, b)]);
        
        [Pure]
        public static IOperationSymbol<TLiteral> operator /(ISymbol<TLiteral> a, TLiteral b)
            => new InstructionOperation<TLiteral>(OpCodes.Div, 
                [a, LiteralSymbolFactory.Create(a.Context, b)]);
        
        [Pure]
        public static IOperationSymbol<TLiteral> operator %(ISymbol<TLiteral> a, TLiteral b)
            => new InstructionOperation<TLiteral>(OpCodes.Rem, 
                [a, LiteralSymbolFactory.Create(a.Context, b)]);
        
        [Pure]
        public static IOperationSymbol<TLiteral> operator |(ISymbol<TLiteral> a, TLiteral b)
            => new InstructionOperation<TLiteral>(OpCodes.Or, 
                [a, LiteralSymbolFactory.Create(a.Context, b)]);
        
        [Pure]
        public static IOperationSymbol<TLiteral> operator &(ISymbol<TLiteral> a, TLiteral b)
            => new InstructionOperation<TLiteral>(OpCodes.And, 
                [a, LiteralSymbolFactory.Create(a.Context, b)]);
        
        [Pure]
        public static IOperationSymbol<TLiteral> operator ^(ISymbol<TLiteral> a, TLiteral b)
            => new InstructionOperation<TLiteral>(OpCodes.Xor, 
                [a, LiteralSymbolFactory.Create(a.Context, b)]);
        
        [Pure]
        public static IOperationSymbol<bool> operator >(ISymbol<TLiteral> a, TLiteral b)
            => new InstructionOperation<bool>(
                PrimitiveTypeMetadata<TLiteral>.IsUnsigned.Value ? OpCodes.Cgt_Un : OpCodes.Cgt, 
                [a, LiteralSymbolFactory.Create(a.Context, b)]);
        
        [Pure]
        public static IOperationSymbol<bool> operator <(ISymbol<TLiteral> a, TLiteral b)
            => new InstructionOperation<bool>(
                PrimitiveTypeMetadata<TLiteral>.IsUnsigned.Value ? OpCodes.Clt_Un : OpCodes.Clt, 
                [a, LiteralSymbolFactory.Create(a.Context, b)]);

        [Pure]
        public static IOperationSymbol<bool> operator >=(ISymbol<TLiteral> a, TLiteral b)
            => (a < b).Not();

        [Pure]
        public static IOperationSymbol<bool> operator <=(ISymbol<TLiteral> a, TLiteral b)
            => (a > b).Not();
    }
    
    extension<TLiteral>(ISymbol<TLiteral> self) 
        where TLiteral : unmanaged, INumber<TLiteral>
    {
        [Pure]
        public IOperationSymbol<bool> IsEqualTo(TLiteral literal)
            => new InstructionOperation<bool>(OpCodes.Ceq, 
                [self, LiteralSymbolFactory.Create(self.Context, literal)]);

        [Pure]
        public IOperationSymbol<bool> IsNotEqualTo(TLiteral literal)
            => self.IsEqualTo(literal).Not();
    }
    
    extension(ISymbol<bool> self)
    {
        [Pure]
        public IOperationSymbol<bool> IsEqualTo(bool literal)
            => new InstructionOperation<bool>(OpCodes.Ceq, 
                [self, new LiteralBooleanSymbol(self.Context, literal)]);

        [Pure]
        public IOperationSymbol<bool> IsNotEqualTo(bool literal)
            => self.IsEqualTo(literal).Not();
    }
    
    extension(ISymbol<string> self)
    {
        [Pure]
        public IOperationSymbol<bool> IsEqualTo(string literal)
            => self.IsEqualTo(new LiteralStringSymbol(self.Context, literal));

        [Pure]
        public IOperationSymbol<bool> IsNotEqualTo(string literal)
            => self.IsEqualTo(literal).Not();
    }

    extension(IAssignableSymbol<string> self)
    {
        public void AssignValue(string value)
            => self.AssignContent(new LiteralStringSymbol(self.Context, value));
    }
    
    extension(IAssignableSymbol<bool> self)
    {
        public void AssignValue(bool value)
            => self.AssignContent(new LiteralBooleanSymbol(self.Context, value));
    }
}