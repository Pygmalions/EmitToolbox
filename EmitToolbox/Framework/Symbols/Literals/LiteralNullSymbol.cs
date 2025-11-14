namespace EmitToolbox.Framework.Symbols.Literals;

public readonly struct LiteralNullSymbol : ISymbol
{
    public DynamicFunction Context { get; }
    
    public Type ContentType { get; }

    public LiteralNullSymbol(DynamicFunction context, Type type)
    {
        Context = context;
        ContentType = type;
        if (type.IsValueType)
            throw new ArgumentException($"Specified type '{type}' is a value type.");
    }
    
    public void LoadContent() => Context.Code.Emit(OpCodes.Ldnull);
}

public readonly struct LiteralNullSymbol<TContent> : ISymbol<TContent?>
{
    public DynamicFunction Context { get; }
    
    public Type ContentType { get; }
    
    public LiteralNullSymbol(DynamicFunction context)
    {
        Context = context;
        ContentType = typeof(TContent);
        if (ContentType.IsValueType)
            throw new ArgumentException($"Specified type '{ContentType}' is a value type.");
    }
    
    public void LoadContent() => Context.Code.Emit(OpCodes.Ldnull);
}