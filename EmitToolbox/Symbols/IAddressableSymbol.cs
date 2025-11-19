namespace EmitToolbox.Symbols;

public interface IAddressableSymbol : ISymbol
{
    /// <summary>
    /// Emit the address of this symbol onto the evaluation stack.
    /// </summary>
    void LoadAddress();
}

// ReSharper disable once TypeParameterCanBeVariant : Content maybe set through address.
public interface IAddressableSymbol<TContent> : IAddressableSymbol, ISymbol<TContent> 
    where TContent : allows ref struct
{
}