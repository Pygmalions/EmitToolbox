namespace EmitToolbox.Framework.Symbols;

public interface IAssignableSymbol : ISymbol
{
    /// <summary>
    /// Assign a symbol to this symbol.
    /// If this symbol contains a reference (<see cref="ISymbol.ContentType"/> is a by-ref type),
    /// then the address of another symbol is stored into this symbol. 
    /// </summary>
    /// <param name="other">Another symbol whose value or address to assign to this symbol.</param>
    void AssignContent(ISymbol other);
}

public interface IAssignableSymbol<in TContent> : ISymbol where TContent : allows ref struct
{
    /// <summary>
    /// Assign a symbol to this symbol.
    /// If this symbol contains a reference (<see cref="ISymbol.ContentType"/> is a by-ref type),
    /// then another symbol is stored as a reference into this symbol;
    /// otherwise, another symbol is stored as a (dereferenced) value into this symbol.
    /// </summary>
    /// <param name="other">Another symbol whose value or address to assign to this symbol.</param>
    void AssignContent(ISymbol<TContent> other);

    /// <summary>
    /// Store the content from the evaluation stack into this symbol.
    /// The content on the stack should be of the same type as <see cref="ISymbol.ContentType"/> of this symbol.
    /// </summary>
    void StoreContent();
}