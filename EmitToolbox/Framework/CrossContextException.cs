using System.Diagnostics;

namespace EmitToolbox.Framework;

public class CrossContextException() : Exception("Symbols cannot be used in different DynamicMethod contexts.")
{
    [StackTraceHidden, DebuggerStepThrough]
    public static void Examine(params Span<ISymbol> symbols)
    {
        DynamicMethod? context = null;
        foreach (var symbol in symbols)
        {
            if (context is null)
            {
                context = symbol.Context;
                continue;
            }

            if (!ReferenceEquals(context, symbol.Context))
                throw new CrossContextException();
        }
    }
}