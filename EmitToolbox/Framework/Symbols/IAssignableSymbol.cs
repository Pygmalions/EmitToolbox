namespace EmitToolbox.Framework.Symbols;

public interface IAssignableSymbol : ISymbol
{
    /// <summary>
    /// Assign a symbol to this symbol.
    /// If this symbol contains a reference (<see cref="ISymbol.ContentType"/> is a by-ref type),
    /// then the address of another symbol is stored into this symbol. 
    /// </summary>
    /// <param name="other">Another symbol whose value or address to assign to this symbol.</param>
    void Assign(ISymbol other);
}

public interface IAssignableSymbol<in TContent> : ISymbol where TContent : allows ref struct
{
    /// <summary>
    /// Assign a symbol to this symbol.
    /// If this symbol contains a reference (<see cref="ISymbol.ContentType"/> is a by-ref type),
    /// then the address of another symbol is stored into this symbol. 
    /// </summary>
    /// <param name="other">Another symbol whose value or address to assign to this symbol.</param>
    void Assign(ISymbol<TContent> other);
}