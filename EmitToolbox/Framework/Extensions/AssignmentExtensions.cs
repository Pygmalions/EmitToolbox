using EmitToolbox.Framework.Symbols;
using EmitToolbox.Framework.Utilities;

namespace EmitToolbox.Framework.Extensions;

public static class AssignmentExtensions
{
    private static void CopyValue(ISymbol source, ISymbol destination)
    {
        var basicType = destination.BasicType;
        var code =
            CrossContextException.EnsureContext(source, destination).Code;

        // When the destination holds a value type:
        if (basicType.IsValueType)
        {
            // When the destination holds a reference or is addressable:
            if (destination.CanLoadAsReference)
            {
                destination.LoadAsReference();

                if (source.CanLoadAsReference)
                {
                    source.LoadAsReference();
                    code.Emit(OpCodes.Cpobj, basicType);
                    return;
                }

                source.LoadAsValue();
                if (basicType.IsPrimitive)
                    code.Emit(PrimitiveTypeMetadata.GetInstructionForIndirectStoring(basicType));
                else
                    code.Emit(OpCodes.Stobj, basicType);
                return;
            }
        }
        // When the destination holds a reference to a reference type:
        else if (destination.ContentType.IsByRef)
        {
            destination.LoadContent();
            source.LoadAsValue();
            code.Emit(OpCodes.Stind_Ref);
            return;
        }

        if (destination is not IAssignableSymbol assignable)
            throw new InvalidOperationException(
                "Cannot copy value: the destination symbol is not a reference nor assignable.");

        assignable.AssignContent(source);
    }

    /// <summary>
    /// Assign the value of the source symbol to the destination symbol (this symbol).
    /// If the source and destination are both value types and can be loaded as references,
    /// then this method will perform a value copy to optimize the performance.
    /// </summary>
    /// <param name="destination">Destination symbol to copy value to.</param>
    /// <param name="source">Source symbol to copy value from.</param>
    /// <typeparam name="TContent">Type of the content to copy.</typeparam>
    public static void AssignValue<TContent>(
        this IAssignableSymbol<TContent> destination, ISymbol<TContent> source)
        where TContent : allows ref struct
        => CopyValue(source, destination);

    /// <summary>
    /// Assign the value of the source symbol to the destination symbol (this symbol).
    /// If the source and destination are both value types and can be loaded as references,
    /// then this method will perform a value copy to optimize the performance.
    /// </summary>
    /// <param name="destination">Destination symbol to copy value to.</param>
    /// <param name="source">Source symbol to copy value from.</param>
    public static void AssignValue(this IAssignableSymbol destination, ISymbol source)
        => CopyValue(source, destination);

    extension<TContent>(ISymbol<TContent> self) where TContent : allows ref struct
    {
        /// <summary>
        /// Copy the value of the source symbol to the destination symbol (this symbol).
        /// If the source and destination are both value types and can be loaded as references,
        /// then this method will perform a value copy to optimize the performance.
        /// </summary>
        /// <param name="source">Source symbol to copy value from.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the destination symbol is not a reference nor assignable.
        /// </exception>
        public void CopyValueFrom(ISymbol<TContent> source)
            => CopyValue(source, self);

        /// <summary>
        /// Copy the value of the source symbol (this symbol) to the destination symbol.
        /// If the source and destination are both value types and can be loaded as references,
        /// then this method will perform a value copy to optimize the performance.
        /// </summary>
        /// <param name="destination">Destination symbol to copy value to.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the destination symbol is not a reference nor assignable.
        /// </exception>
        public void CopyValueTo(ISymbol<TContent> destination)
            => CopyValue(self, destination);
    }

    extension(ISymbol self)
    {
        /// <summary>
        /// Copy the value of the source symbol to the destination symbol (this symbol).
        /// If the source and destination are both value types and can be loaded as references,
        /// then this method will perform a value copy to optimize the performance.
        /// </summary>
        /// <param name="source">Source symbol to copy value from.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the destination symbol is not a reference nor assignable.
        /// </exception>
        public void CopyValueFrom(ISymbol source)
            => CopyValue(source, self);

        /// <summary>
        /// Copy the value of the source symbol (this symbol) to the destination symbol.
        /// If the source and destination are both value types and can be loaded as references,
        /// then this method will perform a value copy to optimize the performance.
        /// </summary>
        /// <param name="destination">Destination symbol to copy value to.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the destination symbol is not a reference nor assignable.
        /// </exception>
        public void CopyValueTo(ISymbol destination)
            => CopyValue(self, destination);
    }
}