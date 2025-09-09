namespace EmitToolbox.Framework;

public interface IAssignableSymbol : ISymbol
{
    /// <summary>
    /// Directly store the content on the stack into this variable symbol.
    /// </summary>
    void EmitStoreContent();
}

public interface IAssignableSymbol<TType> : IAssignableSymbol, ISymbol<TType>
{
    
}