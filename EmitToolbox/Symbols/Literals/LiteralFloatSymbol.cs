namespace EmitToolbox.Symbols.Literals;

public readonly struct LiteralFloat32Symbol(DynamicFunction context, float value) : ILiteralSymbol<float>
{
    public DynamicFunction Context => context;

    public float Value => value;

    public void LoadContent()
        => Context.Code.Emit(OpCodes.Ldc_R4, Value);
}

public readonly struct LiteralFloat64Symbol(DynamicFunction context, double value) : ILiteralSymbol<double>
{
    public DynamicFunction Context => context;

    public double Value => value;

    public void LoadContent()
        => Context.Code.Emit(OpCodes.Ldc_R8, Value);
}