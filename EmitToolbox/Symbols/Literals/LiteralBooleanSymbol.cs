namespace EmitToolbox.Symbols.Literals;

public readonly struct LiteralBooleanSymbol(DynamicFunction context, bool value) : ILiteralSymbol<bool>
{
    public bool Value => value;

    public DynamicFunction Context => context;
    
    public void LoadContent()
    {
        Context.Code.Emit(OpCodes.Ldc_I4_S, value ? 1 : 0);
    }
}