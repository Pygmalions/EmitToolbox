using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework.Extensions;

public static class LabelExtensions
{
    extension(DynamicFunction self)
    {
        /// <summary>
        /// Define a label for jumping to.
        /// This label needs to be marked somewhere before this method is being built.
        /// </summary>
        public CodeLabel DefineLabel() => new(self.Code);
    }
    
    public readonly struct CodeLabel(ILGenerator code)
    {
        public Label Label { get; } = code.DefineLabel();
        
        /// <summary>
        /// Mark this label at the current position of the IL stream.
        /// </summary>
        public void MarkLabel() => code.MarkLabel(Label);

        /// <summary>
        /// Goto this label if the specified condition is true.
        /// </summary>
        /// <param name="condition">Condition to check.</param>
        public void GotoIfTrue(ISymbol<bool> condition)
        {
            condition.LoadAsValue();
            code.Emit(OpCodes.Brtrue, Label);
        }

        /// <summary>
        /// Goto this label if the specified condition is false.
        /// </summary>
        /// <param name="condition">Condition to check.</param>
        public void GotoIfFalse(ISymbol<bool> condition)
        {
            condition.LoadAsValue();
            code.Emit(OpCodes.Brfalse, Label);
        }
    }
}