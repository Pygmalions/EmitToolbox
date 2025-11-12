using EmitToolbox.Framework.Symbols;
using EmitToolbox.Framework.Symbols.Literals;
using EmitToolbox.Framework.Symbols.Operations;

namespace EmitToolbox.Framework.Extensions;

public static class NullabilityExtensions
{
    private class IsObjectNull(ISymbol target) : OperationSymbol<bool>([target])
    {
        public override void LoadContent()
        {
            var code = Context.Code;
            target.LoadAsValue();
            code.Emit(OpCodes.Ldnull);
            code.Emit(OpCodes.Ceq);
        }
    }

    private class IsObjectNotNull(ISymbol target) : OperationSymbol<bool>([target])
    {
        public override void LoadContent()
        {
            var code = Context.Code;
            target.LoadAsValue();
            code.Emit(OpCodes.Ldnull);
            code.Emit(OpCodes.Cgt_Un);
        }
    }

    extension<TContent>(ISymbol<TContent> self) where TContent : class?
    {
        public OperationSymbol<bool> IsNull()
            => new IsObjectNull(self);

        public OperationSymbol<bool> IsNotNull()
            => new IsObjectNotNull(self);
    }
    
    extension<TContent>(IAssignableSymbol<TContent> self) where TContent : class?
    {
        public void AssignNull()
            => self.CopyValueFrom(new LiteralNullSymbol<TContent>());
    }

    extension<TContent>(ISymbol<TContent?> self) where TContent : struct
    {
        public OperationSymbol<bool> HasValue()
            => new InvocationOperation<bool>(
                typeof(TContent?).GetProperty(nameof(Nullable<>.HasValue))!.GetMethod!,
                self, []);

        public OperationSymbol<TContent> GetValue()
            => new InvocationOperation<TContent>(
                typeof(TContent?).GetProperty(nameof(Nullable<>.Value))!.GetMethod!,
                self, []);
    }
    
    extension<TContent>(ISymbol<TContent> self) where TContent : struct
    {
        public VariableSymbol<TContent?> ToNullable()
        {
            return self.Context.New<TContent?>(
                typeof(TContent?).GetConstructor([typeof(TContent)])!, [self]);
        }
    }
}