using EmitToolbox.Framework.Symbols;
using EmitToolbox.Framework.Symbols.Literals;
using EmitToolbox.Framework.Symbols.Operations;

namespace EmitToolbox.Framework.Extensions;

public static class EnumExtensions
{
    internal class EnumEquality<TEnum>(ISymbol<TEnum> self, ISymbol<TEnum> other) :
        OperationSymbol<bool>([self, other])
        where TEnum : struct, Enum
    {
        public override void LoadContent()
        {
            self.LoadAsValue();
            other.LoadAsValue();
            Context.Code.Emit(OpCodes.Ceq);
        }
    }
    
    internal class IsEnumHavingFlag<TEnum>(ISymbol<TEnum> self, ISymbol<TEnum> flag) :
        OperationSymbol<bool>([self, flag])
        where TEnum : struct, Enum
    {
        public override void LoadContent()
        {
            var code = Context.Code;
            self.LoadAsValue();
            flag.LoadAsValue();
            code.Emit(OpCodes.And);
            flag.LoadAsValue();
            code.Emit(OpCodes.Ceq);
        }
    }

    extension<TEnum>(ISymbol<TEnum>)
    {
        public static OperationSymbol<TEnum> operator |(ISymbol<TEnum> a, ISymbol<TEnum> b)
            => new InstructionOperation<TEnum>(OpCodes.Or, 
                [a, b]);
        
        public static OperationSymbol<TEnum> operator &(ISymbol<TEnum> a, ISymbol<TEnum> b)
            => new InstructionOperation<TEnum>(OpCodes.And, 
                [a, b]);
        
        public static OperationSymbol<TEnum> operator ^(ISymbol<TEnum> a, ISymbol<TEnum> b)
            => new InstructionOperation<TEnum>(OpCodes.Xor, 
                [a, b]);
        
        public static OperationSymbol<TEnum> operator |(ISymbol<TEnum> a, TEnum b)
            => a | LiteralSymbolFactory.Create(a.Context, b);
        
        public static OperationSymbol<TEnum> operator &(ISymbol<TEnum> a, TEnum b)
            => a & LiteralSymbolFactory.Create(a.Context, b);
        
        public static OperationSymbol<TEnum> operator ^(ISymbol<TEnum> a, TEnum b)
            => a ^ LiteralSymbolFactory.Create(a.Context, b);
    }

    extension<TEnum>(ISymbol<TEnum> self) where TEnum : struct, Enum
    {
        public OperationSymbol<bool> IsEqualTo(ISymbol<TEnum> other)
            => new EnumEquality<TEnum>(self, other);

        public OperationSymbol<bool> IsNotEqualTo(ISymbol<TEnum> other)
            => new EnumEquality<TEnum>(self, other).Not();
        
        public OperationSymbol<bool> HasFlag(ISymbol<TEnum> other)
            => new IsEnumHavingFlag<TEnum>(self, other);

        public OperationSymbol<bool> IsEqualTo(TEnum literal)
            => self.IsEqualTo(LiteralSymbolFactory.Create(self.Context, literal));
        
        public OperationSymbol<bool> IsNotEqualTo(TEnum literal)
            => self.IsEqualTo(LiteralSymbolFactory.Create(self.Context, literal)).Not();
        
        public OperationSymbol<bool> HasFlag(TEnum literal)
            => self.HasFlag(LiteralSymbolFactory.Create(self.Context, literal));
    }
}