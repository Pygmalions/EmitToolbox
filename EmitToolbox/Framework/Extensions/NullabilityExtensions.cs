using System.Diagnostics.Contracts;
using EmitToolbox.Framework.Symbols;
using EmitToolbox.Framework.Symbols.Literals;
using EmitToolbox.Framework.Symbols.Operations;

namespace EmitToolbox.Framework.Extensions;

public static class NullabilityExtensions
{
    private class IsObjectNull(ISymbol target) : OperationSymbol<bool>(target.Context)
    {
        public override void LoadContent()
        {
            var code = Context.Code;
            target.LoadAsValue();
            code.Emit(OpCodes.Ldnull);
            code.Emit(OpCodes.Ceq);
        }
    }

    private class IsObjectNotNull(ISymbol target) : OperationSymbol<bool>(target.Context)
    {
        public override void LoadContent()
        {
            var code = Context.Code;
            target.LoadAsValue();
            code.Emit(OpCodes.Ldnull);
            code.Emit(OpCodes.Cgt_Un);
        }
    }

    private class NullableHasValue(ISymbol target) : OperationSymbol<bool>(target.Context)
    {
        public override void LoadContent()
        {
            target.LoadAsTarget();
            Context.Code.Emit(OpCodes.Call,
                target.ContentType
                    .GetProperty(nameof(Nullable<>.HasValue))!
                    .GetMethod!);
        }
    }

    private class NullableNotHasValue(ISymbol target) : OperationSymbol<bool>(target.Context)
    {
        public override void LoadContent()
        {
            var code = Context.Code;
            target.LoadAsTarget();
            code.Emit(OpCodes.Call,
                target.ContentType
                    .GetProperty(nameof(Nullable<>.HasValue))!
                    .GetMethod!);
            code.Emit(OpCodes.Ldc_I4_0);
            code.Emit(OpCodes.Ceq);
        }
    }

    extension(ISymbol self)
    {
        /// <summary>
        /// According to the content of this symbol, check if
        /// it is null (for reference types) or does not have a value (for Nullable value types).
        /// For non-nullable value types, always return false.
        /// </summary>
        /// <returns>Operation symbol or literal false (for non-nullable value types).</returns>
        [Pure]
        public IOperationSymbol<bool> HasNullValue()
        {
            var type = self.BasicType;
            if (!type.IsValueType)
                return new IsObjectNull(self);
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)
                ? new NullableNotHasValue(self)
                : new LiteralBooleanSymbol(self.Context, false).AsSymbol<bool>();
        }

        /// <summary>
        /// According to the content of this symbol, check if
        /// it is null (for reference types) or does not have a value (for Nullable value types).
        /// For non-nullable value types, always return false.
        /// </summary>
        /// <returns>Operation symbol or literal false (for non-nullable value types).</returns>
        [Pure]
        public ISymbol<bool> HasNotNullValue()
        {
            var type = self.BasicType;
            if (!type.IsValueType)
                return new IsObjectNotNull(self);
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)
                ? new NullableHasValue(self)
                : new LiteralBooleanSymbol(self.Context, true).AsSymbol<bool>();
        }
    }

    extension<TContent>(ISymbol<TContent> self) where TContent : class?
    {
        public IOperationSymbol<bool> IsNull()
            => new IsObjectNull(self);

        public IOperationSymbol<bool> IsNotNull()
            => new IsObjectNotNull(self);
    }

    extension<TContent>(ISymbol<TContent?> self) where TContent : struct
    {
        [Pure]
        public IOperationSymbol<bool> HasValue()
            => new NullableHasValue(self);

        [Pure]
        public IOperationSymbol<TContent> GetValue()
            => new InvocationOperation<TContent>(
                typeof(TContent?).GetProperty(nameof(Nullable<>.Value))!.GetMethod!,
                self, []);
    }

    extension<TContent>(ISymbol<TContent> self) where TContent : struct
    {
        [Pure]
        public VariableSymbol<TContent?> ToNullable()
        {
            return self.Context.New<TContent?>(
                typeof(TContent?).GetConstructor([typeof(TContent)])!, [self]);
        }

        public void ToNullable(IAssignableSymbol<TContent?> target)
        {
            target.AssignNew(typeof(TContent?).GetConstructor([typeof(TContent)])!, [self]);
        }
    }
}