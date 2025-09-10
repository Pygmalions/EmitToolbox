namespace EmitToolbox.Framework;

public interface ISymbol
{
    /// <summary>
    /// Method context in which this symbol is defined.
    /// </summary>
    DynamicMethod Context { get; }
    
    /// <summary>
    /// Type of this value.
    /// </summary>
    Type ContentType { get; }
    
    /// <summary>
    /// Directly load the content of this value symbol into the stack.
    /// </summary>
    void EmitLoadContent();
}

public interface ISymbol<TType> : ISymbol
{
    
}