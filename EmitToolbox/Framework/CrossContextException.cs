using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework;

public class CrossContextException(string? message = null) : Exception(message)
{
    public static DynamicMethod EnsureContext(params IEnumerable<ISymbol?> symbols)
    {
        DynamicMethod? context = null;
        foreach (var symbol in symbols)
        {
            if (symbol == null)
                continue;
            if (context != null)
            {
                if (symbol.Context != context)
                    throw new CrossContextException("Symbols are not from the same context.");
                continue;
            }
            context = symbol.Context;
        }
        
        return context ?? throw new Exception("No symbol is provided to determine the context.");
    }
    
    public static DynamicMethod EnsureContext(
        DynamicMethod context, params IEnumerable<ISymbol?> symbols)
    {
        if (symbols.Any(symbol => symbol != null && symbol.Context != context))
            throw new CrossContextException("Symbols are not from the same context.");

        return context ?? throw new Exception("No symbol is provided to determine the context.");
    }
}