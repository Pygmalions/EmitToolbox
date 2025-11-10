namespace EmitToolbox.Framework.Symbols.Literals;

public readonly struct LiteralStringSymbol(DynamicMethod context, string value) : ILiteralSymbol<string>
{
    public string Value => value;
    
    public DynamicMethod Context => context;
    
    public void LoadContent()
    {
        Context.Code.Emit(OpCodes.Ldstr, value);
    }
}