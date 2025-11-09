namespace EmitToolbox.Framework.Symbols.Literals;

public readonly struct LiteralNullSymbol(DynamicMethod context, Type type) : ISymbol
{
    public DynamicMethod Context { get; } = context;
    
    public Type ContentType { get; } = type;
    
    public void EmitContent() => Context.Code.Emit(OpCodes.Ldnull);
}

public readonly struct LiteralNull<TContent>(DynamicMethod context) : ISymbol, ISymbol<TContent?>
{
    public DynamicMethod Context { get; } = context;
    
    public Type ContentType => typeof(TContent);
    
    public void EmitContent() => Context.Code.Emit(OpCodes.Ldnull);
}