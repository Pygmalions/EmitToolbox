namespace EmitToolbox.Extensions;

public static class EmitMathExtensions
{
    extension(ILGenerator code)
    {
        public void Add()
            => code.Emit(OpCodes.Add);

        public void Subtract()
            => code.Emit(OpCodes.Sub);

        public void Multiply()
            => code.Emit(OpCodes.Mul);

        public void Divide()
            => code.Emit(OpCodes.Div);
    }
}