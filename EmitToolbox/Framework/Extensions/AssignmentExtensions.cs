using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework.Extensions;

public static class AssignmentExtensions
{
    /// <summary>
    /// Copy the value of the source symbol to the destination symbol (this symbol).
    /// If the source and destination are both value types and can be loaded as references,
    /// then this method will perform a value copy to optimize the performance.
    /// </summary>
    /// <param name="destination">Destination symbol to copy value to.</param>
    /// <param name="source">Source symbol to copy value from.</param>
    /// <typeparam name="TContent">Type of the content to copy.</typeparam>
    public static void CopyValueFrom<TContent>(
        this IAssignableSymbol<TContent> destination, ISymbol<TContent> source)
    {
        var code = destination.Context.Code;
        
        if (!typeof(TContent).IsValueType)
        {
            destination.Assign(source);
            return;
        }
        
        if (destination.CanLoadAsReference)
        {
            destination.EmitAsReference();
            
            if (source.CanLoadAsReference)
            {
                source.EmitAsReference();
                code.Emit(OpCodes.Cpobj, typeof(TContent));
                return;
            }
            
            source.EmitAsValue();
            code.Emit(OpCodes.Stobj, typeof(TContent));
            return;
        }
        
        destination.Assign(source);
    }

    /// <summary>
    /// Copy the value of the source symbol (this symbol) to the destination symbol.
    /// If the source and destination are both value types and can be loaded as references,
    /// then this method will perform a value copy to optimize the performance.
    /// </summary>
    /// <param name="destination">Destination symbol to copy value to.</param>
    /// <param name="source">Source symbol to copy value from.</param>
    /// <typeparam name="TContent">Type of the content to copy.</typeparam>
    public static void CopyValueTo<TContent>(this ISymbol<TContent> source, IAssignableSymbol<TContent> destination)
    {
        destination.CopyValueFrom(source);
    }
}