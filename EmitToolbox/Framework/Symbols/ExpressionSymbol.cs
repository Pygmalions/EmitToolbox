namespace EmitToolbox.Framework.Symbols;

public class ExpressionSymbol<TValue>(DynamicMethod context) : ISymbol<TValue>
{
    public DynamicMethod Context { get; } = context;

    public Type ValueType { get; } = typeof(TValue);
    
    public void EmitLoadContent() => Expression().EmitLoadContent();

    public required Func<ISymbol<TValue>> Expression { get; init; }
}