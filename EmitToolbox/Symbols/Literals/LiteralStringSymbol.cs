namespace EmitToolbox.Symbols.Literals;

public readonly struct LiteralStringSymbol(DynamicFunction context, string value) : ILiteralSymbol<string>
{
    public string Value => value;
    
    public DynamicFunction Context => context;
    
    public void LoadContent()
    {
        Context.Code.Emit(OpCodes.Ldstr, value);
    }
}