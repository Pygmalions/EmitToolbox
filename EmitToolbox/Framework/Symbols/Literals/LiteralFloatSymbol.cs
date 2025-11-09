namespace EmitToolbox.Framework.Symbols.Literals;

public readonly struct LiteralFloat32Symbol(DynamicMethod context, float value) : ILiteralSymbol<float>
{
    public DynamicMethod Context => context;

    public float Value => value;

    public void EmitContent()
        => Context.Code.Emit(OpCodes.Ldc_R4, Value);
}

public readonly struct LiteralFloat64Symbol(DynamicMethod context, double value) : ILiteralSymbol<double>
{
    public DynamicMethod Context => context;

    public double Value => value;

    public void EmitContent()
        => Context.Code.Emit(OpCodes.Ldc_R8, Value);
}