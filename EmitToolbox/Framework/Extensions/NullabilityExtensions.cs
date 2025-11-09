using EmitToolbox.Framework.Symbols;
using EmitToolbox.Framework.Symbols.Operations;

namespace EmitToolbox.Framework.Extensions;

public static class NullabilityExtensions
{
    private class CheckingObjectIsNull(ISymbol target) : OperationSymbol<bool>([target])
    {
        public override void EmitContent()
        {
            var code = Context.Code;
            target.EmitAsValue();
            code.Emit(OpCodes.Ldnull);
            code.Emit(OpCodes.Ceq);
        }
    }

    private class CheckingObjectIsNotNull(ISymbol target) : OperationSymbol<bool>([target])
    {
        public override void EmitContent()
        {
            var code = Context.Code;
            target.EmitAsValue();
            code.Emit(OpCodes.Ldnull);
            code.Emit(OpCodes.Cgt_Un);
        }
    }

    extension<TContent>(ISymbol<TContent> self) where TContent : class?
    {
        public OperationSymbol<bool> IsNull()
            => new CheckingObjectIsNull(self);

        public OperationSymbol<bool> IsNotNull()
            => new CheckingObjectIsNotNull(self);
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
                typeof(TContent?).GetConstructor([typeof(TContent)])!, self);
        }
    }
}