namespace EmitToolbox.Extensions;

public static class EmitMathExtensions
{
    public static void Add(this ILGenerator code)
        => code.Emit(OpCodes.Add);

    public static void Subtract(this ILGenerator code)
        => code.Emit(OpCodes.Sub);

    public static void Multiply(this ILGenerator code)
        => code.Emit(OpCodes.Mul);

    public static void Divide(this ILGenerator code)
        => code.Emit(OpCodes.Div);
}