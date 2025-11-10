using System.Numerics;
using EmitToolbox.Framework.Symbols;
using EmitToolbox.Framework.Symbols.Literals;
using EmitToolbox.Framework.Symbols.Operations;
using EmitToolbox.Framework.Utilities;

namespace EmitToolbox.Framework.Extensions;

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
        public static OperationSymbol<TLiteral> operator +(ISymbol<TLiteral> a, TLiteral b)
            => new InstructionOperation<TLiteral>(OpCodes.Add, 
                [a, LiteralSymbolFactory.Create(a.Context, b)]);
        
        public static OperationSymbol<TLiteral> operator -(ISymbol<TLiteral> a, TLiteral b)
            => new InstructionOperation<TLiteral>(OpCodes.Sub, 
                [a, LiteralSymbolFactory.Create(a.Context, b)]);
        
        public static OperationSymbol<TLiteral> operator *(ISymbol<TLiteral> a, TLiteral b)
            => new InstructionOperation<TLiteral>(OpCodes.Mul, 
                [a, LiteralSymbolFactory.Create(a.Context, b)]);
        
        public static OperationSymbol<TLiteral> operator /(ISymbol<TLiteral> a, TLiteral b)
            => new InstructionOperation<TLiteral>(OpCodes.Div, 
                [a, LiteralSymbolFactory.Create(a.Context, b)]);
        
        public static OperationSymbol<TLiteral> operator %(ISymbol<TLiteral> a, TLiteral b)
            => new InstructionOperation<TLiteral>(OpCodes.Rem, 
                [a, LiteralSymbolFactory.Create(a.Context, b)]);
        
        public static OperationSymbol<bool> operator >(ISymbol<TLiteral> a, TLiteral b)
            => new InstructionOperation<bool>(
                PrimitiveTypeMetadata<TLiteral>.IsUnsigned.Value ? OpCodes.Cgt_Un : OpCodes.Cgt, 
                [a, LiteralSymbolFactory.Create(a.Context, b)]);
        
        public static OperationSymbol<bool> operator <(ISymbol<TLiteral> a, TLiteral b)
            => new InstructionOperation<bool>(
                PrimitiveTypeMetadata<TLiteral>.IsUnsigned.Value ? OpCodes.Clt_Un : OpCodes.Clt, 
                [a, LiteralSymbolFactory.Create(a.Context, b)]);

        public static OperationSymbol<bool> operator >=(ISymbol<TLiteral> a, TLiteral b)
            => (a < b).Not();

        public static OperationSymbol<bool> operator <=(ISymbol<TLiteral> a, TLiteral b)
            => (a > b).Not();
    }
    
    extension<TLiteral>(ISymbol<TLiteral> self) where TLiteral : unmanaged, INumber<TLiteral>
    {
        public OperationSymbol<bool> IsEqualTo(TLiteral literal)
            => new InstructionOperation<bool>(OpCodes.Ceq, 
                [self, LiteralSymbolFactory.Create(self.Context, literal)]);

        public OperationSymbol<bool> IsNotEqualTo(TLiteral literal)
            => self.IsEqualTo(literal).Not();
    }
    
    extension(ISymbol<bool> self)
    {
        public OperationSymbol<bool> IsEqualTo(bool literal)
            => new InstructionOperation<bool>(OpCodes.Ceq, 
                [self, LiteralSymbolFactory.Create(self.Context, literal)]);

        public OperationSymbol<bool> IsNotEqualTo(bool literal)
            => self.IsEqualTo(literal).Not();
    }
    
    extension(ISymbol<string> self)
    {
        public OperationSymbol<bool> IsEqualTo(string literal)
            => new InvocationOperation<bool>(
                typeof(string).GetMethod(nameof(string.Equals), [typeof(string)])!, 
                self, [LiteralSymbolFactory.Create(self.Context, literal)]);

        public OperationSymbol<bool> IsNotEqualTo(string literal)
            => self.IsEqualTo(literal).Not();
    }
}