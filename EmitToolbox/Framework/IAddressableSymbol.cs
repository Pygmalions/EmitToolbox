namespace EmitToolbox.Framework;

public interface IAddressableSymbol : ISymbol
{
    /// <summary>
    /// Directly load the address of this value symbol into the stack.
    /// </summary>
    void EmitLoadAddress();
}

public interface IAddressableSymbol<TType> : IAddressableSymbol, ISymbol<TType>
{
    
}