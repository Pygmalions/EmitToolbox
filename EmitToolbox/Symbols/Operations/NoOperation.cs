namespace EmitToolbox.Symbols.Operations;

/// <summary>
/// This operation does nothing.
/// </summary>
/// <param name="symbol">Symbol whose content to return as the result of this operation.</param>
public class NoOperation(ISymbol symbol) : OperationSymbol(symbol.Context, symbol.ContentType)
{
    public override void LoadContent() => symbol.LoadContent();
}

/// <summary>
/// This operation does nothing.
/// </summary>
/// <param name="symbol">Symbol whose content to return as the result of this operation.</param>
public class NoOperation<TContent>(ISymbol<TContent> symbol) 
    : OperationSymbol(symbol.Context, symbol.ContentType), IOperationSymbol<TContent>
{
    public override void LoadContent() => symbol.LoadContent();
}