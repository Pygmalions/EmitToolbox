using EmitToolbox.Symbols;

namespace EmitToolbox.Extensions;

public static class LabelExtensions
{
    extension(DynamicFunction self)
    {
        /// <summary>
        /// Define a label for jumping to.
        /// This label needs to be marked somewhere before this method is being built.
        /// </summary>
        public CodeLabel DefineLabel() => new(self);
    }
    
    public readonly struct CodeLabel(DynamicFunction context)
    {
        public DynamicFunction Context { get; } = context;
        
        public Label Label { get; } = context.Code.DefineLabel();
        
        /// <summary>
        /// Mark this label at the current position of the IL stream.
        /// </summary>
        public void Mark() => Context.Code.MarkLabel(Label);

        /// <summary>
        /// Unconditionally jump to this label.
        /// </summary>
        public void Goto() => Context.Code.Emit(OpCodes.Br, Label);
        
        /// <summary>
        /// Goto this label if the specified condition is true.
        /// </summary>
        /// <param name="condition">Condition to check.</param>
        public void GotoIfTrue(ISymbol<bool> condition)
        {
            condition.LoadAsValue();
            Context.Code.Emit(OpCodes.Brtrue, Label);
        }

        /// <summary>
        /// Goto this label if the specified condition is false.
        /// </summary>
        /// <param name="condition">Condition to check.</param>
        public void GotoIfFalse(ISymbol<bool> condition)
        {
            condition.LoadAsValue();
            Context.Code.Emit(OpCodes.Brfalse, Label);
        }
    }
}