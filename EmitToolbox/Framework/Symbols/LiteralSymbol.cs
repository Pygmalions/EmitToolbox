namespace EmitToolbox.Framework.Symbols;

public abstract class LiteralSymbol : ISymbol
{
    public abstract DynamicMethod Context { get; }

    public abstract Type ContentType { get; }

    public abstract void EmitLoadContent();
}

public abstract class LiteralSymbol<TValue>(DynamicMethod context, TValue value) : LiteralSymbol, ISymbol<TValue>
{
    public sealed override Type ContentType { get; } = typeof(TValue);

    public override DynamicMethod Context { get; } = context;

    /// <summary>
    /// The literal value represented by this symbol.
    /// </summary>
    public TValue Value { get; } = value;
}