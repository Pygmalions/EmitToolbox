namespace EmitToolbox.Framework.Symbols.Literals;

public readonly struct LiteralNullSymbol(DynamicFunction context, Type type) : ISymbol
{
    public DynamicFunction Context { get; } = context;
    
    public Type ContentType { get; } = type;
    
    public void LoadContent() => Context.Code.Emit(OpCodes.Ldnull);
}

public readonly struct LiteralNull<TContent>(DynamicFunction context) : ISymbol, ISymbol<TContent?>
{
    public DynamicFunction Context { get; } = context;
    
    public Type ContentType => typeof(TContent);
    
    public void LoadContent() => Context.Code.Emit(OpCodes.Ldnull);
}