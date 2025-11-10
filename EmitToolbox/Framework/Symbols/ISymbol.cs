namespace EmitToolbox.Framework.Symbols;

public interface ISymbol
{
    /// <summary>
    /// Content type of this symbol.
    /// </summary>
    Type ContentType { get; }

    /// <summary>
    /// Dynamic method that declares this method.
    /// </summary>
    DynamicMethod Context { get; }

    /// <summary>
    /// Load the content of this symbol into the evaluation stack.
    /// </summary>
    void LoadContent();
}

public interface ISymbol<out TContent> : ISymbol where TContent : allows ref struct
{
}

public static class SymbolExtensions
{
    extension(ISymbol self)
    {
        /// <summary>
        /// Basic type of this symbol, without any modifiers including by-ref and pointer.
        /// </summary>
        public Type BasicType => self.ContentType.BasicType;
    }
}