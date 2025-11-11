using EmitToolbox.Framework.Symbols;
using EmitToolbox.Framework.Utilities;

namespace EmitToolbox.Framework.Extensions;

public static class AssignmentExtensions
{
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
        => destination.CopyValueFrom(source);
    
    /// <summary>
    /// Assign the value of the source symbol to the destination symbol (this symbol).
    /// If the source and destination are both value types and can be loaded as references,
    /// then this method will perform a value copy to optimize the performance.
    /// </summary>
    /// <param name="destination">Destination symbol to copy value to.</param>
    /// <param name="source">Source symbol to copy value from.</param>
    public static void AssignValue(this IAssignableSymbol destination, ISymbol source)
        => destination.CopyValueFrom(source);
    
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
        {
            var code = self.Context.Code;
            
            var type = typeof(TContent);
            
            if (type.IsValueType)
            {
                if (self.CanLoadAsReference)
                {
                    self.LoadAsReference();
            
                    if (source.CanLoadAsReference)
                    {
                        source.LoadAsReference();
                        code.Emit(OpCodes.Cpobj, type);
                        return;
                    }
            
                    source.LoadAsValue();
                    if (type.IsPrimitive)
                        code.Emit(PrimitiveTypeMetadata<TContent>.InstructionForIndirectStoring.Value);
                    else
                        code.Emit(OpCodes.Stobj, type);
                    return;
                }
            }
        
            if (self is not IAssignableSymbol<TContent> assignable)
                throw new InvalidOperationException(
                    "Cannot copy value from this symbol to this symbol: it is not a reference nor assignable.");
        
            assignable.AssignContent(source);
        }
        
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
            => destination.CopyValueFrom(self);
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
        {
            var code = self.Context.Code;
            
            var type = source.BasicType;

            if (type.IsValueType && type != self.BasicType ||
                !self.BasicType.IsAssignableFrom(type))
                throw new InvalidOperationException(
                    $"Cannot assign value of type '{type}' to symbol of type '{self.BasicType}'.");
            
            if (type.IsValueType)
            {
                if (self.CanLoadAsReference)
                {
                    self.LoadAsReference();
            
                    if (source.CanLoadAsReference)
                    {
                        source.LoadAsReference();
                        code.Emit(OpCodes.Cpobj, type);
                        return;
                    }
            
                    source.LoadAsValue();
                    if (type.IsPrimitive)
                        code.Emit(PrimitiveTypeMetadata.GetInstructionForIndirectStoring(type));
                    else
                        code.Emit(OpCodes.Stobj, type);
                    return;
                }
            }
        
            if (self is not IAssignableSymbol assignable)
                throw new InvalidOperationException(
                    "Cannot copy value from this symbol to this symbol: it is not a reference nor assignable.");
        
            assignable.AssignContent(source);
        }
        
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
            => destination.CopyValueFrom(self);
    }
}