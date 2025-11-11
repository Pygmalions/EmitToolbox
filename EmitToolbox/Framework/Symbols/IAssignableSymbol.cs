using EmitToolbox.Framework.Symbols.Operations;

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

    /// <summary>
    /// Store the content from the evaluation stack into this symbol.
    /// The content on the stack should be of the same type as <see cref="ISymbol.ContentType"/> of this symbol.
    /// </summary>
    void StoreContent();
}

public interface IAssignableSymbol<in TContent> : IAssignableSymbol where TContent : allows ref struct
{
    /// <summary>
    /// Assign a symbol to this symbol.
    /// If this symbol contains a reference (<see cref="ISymbol.ContentType"/> is a by-ref type),
    /// then another symbol is stored as a reference into this symbol;
    /// otherwise, another symbol is stored as a (dereferenced) value into this symbol.
    /// </summary>
    /// <param name="other">Another symbol whose value or address to assign to this symbol.</param>
    void AssignContent(ISymbol<TContent> other);

    void IAssignableSymbol.AssignContent(ISymbol other)
    {
        if (other is ISymbol<TContent> typedOther)
            AssignContent(typedOther);
        var basicType = typeof(TContent);
        if (basicType.IsValueType && other.BasicType != basicType ||
            !other.ContentType.IsAssignableTo(typeof(TContent)))
            throw new ArgumentException(
                $"Specified symbol of type '{other.BasicType}' is not assignable to this symbol of type '{basicType}'.");
        AssignContent(other.AsSymbol<TContent>());
    }
}