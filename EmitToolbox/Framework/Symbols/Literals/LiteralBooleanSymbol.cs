namespace EmitToolbox.Framework.Symbols.Literals;

public readonly struct LiteralBooleanSymbol(DynamicMethod context, bool value) : ILiteralSymbol<bool>
{
    public bool Value => value;

    public DynamicMethod Context => context;
    
    public void LoadContent()
    {
        Context.Code.Emit(OpCodes.Ldc_I4_S, value ? 1 : 0);
    }
}