using System.Diagnostics.Contracts;
using EmitToolbox.Symbols;
using EmitToolbox.Symbols.Literals;
using EmitToolbox.Symbols.Operations;

namespace EmitToolbox.Extensions;

public static class EnumExtensions
{
    private class EnumEquality<TEnum>(ISymbol<TEnum> self, ISymbol<TEnum> other) :
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
    
    private class IsEnumHavingFlag<TEnum>(ISymbol<TEnum> self, ISymbol<TEnum> flag) :
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

    extension<TEnum>(ISymbol<TEnum>) where TEnum : struct, Enum
    {
        [Pure]
        public static IOperationSymbol<TEnum> operator |(ISymbol<TEnum> a, ISymbol<TEnum> b)
            => new InstructionOperation<TEnum>(OpCodes.Or, 
                [a, b]);
        
        [Pure]
        public static IOperationSymbol<TEnum> operator &(ISymbol<TEnum> a, ISymbol<TEnum> b)
            => new InstructionOperation<TEnum>(OpCodes.And, 
                [a, b]);
        
        [Pure]
        public static IOperationSymbol<TEnum> operator ^(ISymbol<TEnum> a, ISymbol<TEnum> b)
            => new InstructionOperation<TEnum>(OpCodes.Xor, 
                [a, b]);
        
        [Pure]
        public static IOperationSymbol<TEnum> operator |(ISymbol<TEnum> a, TEnum b)
            => a | new LiteralEnumSymbol<TEnum>(a.Context, b);
        
        [Pure]
        public static IOperationSymbol<TEnum> operator &(ISymbol<TEnum> a, TEnum b)
            => a & new LiteralEnumSymbol<TEnum>(a.Context, b);
        
        [Pure]
        public static IOperationSymbol<TEnum> operator ^(ISymbol<TEnum> a, TEnum b)
            => a ^ new LiteralEnumSymbol<TEnum>(a.Context, b);
    }

    extension<TEnum>(ISymbol<TEnum> self) where TEnum : struct, Enum
    {
        [Pure]
        public IOperationSymbol<bool> IsEqualTo(ISymbol<TEnum> other)
            => new EnumEquality<TEnum>(self, other);

        [Pure]
        public IOperationSymbol<bool> IsNotEqualTo(ISymbol<TEnum> other)
            => new EnumEquality<TEnum>(self, other).Not();
        
        [Pure]
        public IOperationSymbol<bool> HasFlag(ISymbol<TEnum> other)
            => new IsEnumHavingFlag<TEnum>(self, other);

        [Pure]
        public IOperationSymbol<bool> IsEqualTo(TEnum literal)
            => self.IsEqualTo(new LiteralEnumSymbol<TEnum>(self.Context, literal));
        
        [Pure]
        public IOperationSymbol<bool> IsNotEqualTo(TEnum literal)
            => self.IsEqualTo(new LiteralEnumSymbol<TEnum>(self.Context, literal)).Not();
        
        [Pure]
        public IOperationSymbol<bool> HasFlag(TEnum literal)
            => self.HasFlag(new LiteralEnumSymbol<TEnum>(self.Context, literal));
    }
}