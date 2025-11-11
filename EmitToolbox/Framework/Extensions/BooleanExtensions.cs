using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework.Extensions;

public static class BooleanExtensions
{
    private class BooleanNotOperation(ISymbol<bool> target) : OperationSymbol<bool>(target.Context)
    {
        public override void LoadContent()
        {
            target.LoadAsValue();
            Context.Code.Emit(OpCodes.Ldc_I4_0);
            Context.Code.Emit(OpCodes.Ceq);
        }
    }

    private class BooleanOrOperation(ISymbol<bool> a, ISymbol<bool> b)
        : OperationSymbol<bool>([a, b])
    {
        public override void LoadContent()
        {
            var code = Context.Code;

            a.LoadAsValue();
            b.LoadAsValue();
            code.Emit(OpCodes.Or);
            code.Emit(OpCodes.Ldc_I4_0);
            code.Emit(OpCodes.Cgt_Un);
        }
    }

    private class BooleanAndOperation(ISymbol<bool> a, ISymbol<bool> b)
        : OperationSymbol<bool>([a, b])
    {
        public override void LoadContent()
        {
            var code = Context.Code;
            a.LoadAsValue();
            b.LoadAsValue();
            code.Emit(OpCodes.And);
            code.Emit(OpCodes.Ldc_I4_0);
            code.Emit(OpCodes.Cgt_Un);
        }
    }

    internal class ConditionOrOperation(IReadOnlyCollection<ISymbol> conditions) :
        OperationSymbol<bool>(conditions)
    {
        public override void LoadContent()
        {
            var code = Context.Code;
            var labelEnd = code.DefineLabel();
            var labelTrue = code.DefineLabel();

            foreach (var condition in conditions)
            {
                condition.LoadAsValue();
                code.Emit(OpCodes.Brtrue, labelTrue);
            }

            code.Emit(OpCodes.Ldc_I4_0);
            code.Emit(OpCodes.Br, labelEnd);

            code.MarkLabel(labelTrue);

            code.Emit(OpCodes.Ldc_I4_1);
            code.Emit(OpCodes.Br, labelEnd);

            code.MarkLabel(labelEnd);
        }
    }

    internal class ConditionAndOperation(IReadOnlyCollection<ISymbol> conditions) :
        OperationSymbol<bool>(conditions)
    {
        public override void LoadContent()
        {
            var code = Context.Code;
            var labelEnd = code.DefineLabel();
            var labelFalse = code.DefineLabel();

            foreach (var condition in conditions)
            {
                condition.LoadAsValue();
                code.Emit(OpCodes.Brfalse, labelFalse);
            }

            code.Emit(OpCodes.Ldc_I4_1);
            code.Emit(OpCodes.Br, labelEnd);

            code.MarkLabel(labelFalse);

            code.Emit(OpCodes.Ldc_I4_0);
            code.Emit(OpCodes.Br, labelEnd);

            code.MarkLabel(labelEnd);
        }
    }

    extension(ISymbol<bool>)
    {
        /// <summary>
        /// Perform a boolean not operation.
        /// </summary>
        public static OperationSymbol<bool> operator !(ISymbol<bool> symbol)
            => symbol.Not();

        /// <summary>
        /// Perform a boolean and operation.
        /// </summary>
        public static OperationSymbol<bool> operator &(ISymbol<bool> a, ISymbol<bool> b)
            => a.And(b);

        /// <summary>
        /// Perform a boolean or operation.
        /// </summary>
        public static OperationSymbol<bool> operator |(ISymbol<bool> a, ISymbol<bool> b)
            => a.Or(b);
    }
    
    extension(ISymbol<bool> self)
    {
        
        public OperationSymbol<bool> Not()
            => new BooleanNotOperation(self);

        public OperationSymbol<bool> Or(ISymbol<bool> other)
            => new BooleanOrOperation(self, other);

        public OperationSymbol<bool> Or(params IEnumerable<ISymbol<bool>> others)
        {
            var result = others.Aggregate<ISymbol<bool>, OperationSymbol<bool>?>(
                    null, 
                    (current, other) => (current ?? self).Or(other));
            return result ?? throw new Exception("No other boolean symbols are provided.");
        }

        public OperationSymbol<bool> And(ISymbol<bool> other)
            => new BooleanAndOperation(self, other);

        public OperationSymbol<bool> And(params IEnumerable<ISymbol<bool>> others)
        {
            var result = others.Aggregate<ISymbol<bool>, OperationSymbol<bool>?>(
                null,
                (current, other) => (current ?? self).And(other));
            return result ?? throw new Exception("No other boolean symbols are provided.");
        }
        
        /// <summary>
        /// Compared to boolean or operation,
        /// this operation immediately returns true when it encounters the first true condition.
        /// It has the same effect as the '||' operator.
        /// </summary>
        /// <param name="conditions">Conditions to evaluate.</param>
        /// <returns>Evaluation operation of these conditions.</returns>
        public OperationSymbol<bool> ConditionOr(params IEnumerable<ISymbol<bool>> conditions)
            => new ConditionOrOperation(conditions.ToList());
        
        /// <summary>
        /// Compared to boolean or operation,
        /// this operation immediately returns false when it encounters the first false condition.
        /// It has the same effect as the '&&' operator.
        /// </summary>
        /// <param name="conditions">Conditions to evaluate.</param>
        /// <returns>Evaluation operation of these conditions.</returns>
        public OperationSymbol<bool> ConditionAnd(params IEnumerable<ISymbol<bool>> conditions)
            => new ConditionAndOperation(conditions.ToList());
    }
}