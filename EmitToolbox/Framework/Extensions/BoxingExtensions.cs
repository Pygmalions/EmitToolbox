using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework.Extensions;

public static class BoxingExtensions
{
    extension(ISymbol self)
    {
        public void EmitAsObject()
        {
            self.EmitAsValue();
            if (!self.BasicType.IsValueType)
                return;
            self.Context.Code.Emit(OpCodes.Box, self.BasicType);
        }
    }

    private class BoxingOperation(ISymbol target) : OperationSymbol<object>([target])
    {
        public override void EmitContent()
        {
            target.EmitAsValue();
            Context.Code.Emit(OpCodes.Box, target.BasicType);
        }
    }
    
    private class UnboxingAsValueOperation<TValue>(ISymbol target) : 
        OperationSymbol<TValue>([target])
    {
        public override void EmitContent()
        {
            target.EmitAsValue();
            Context.Code.Emit(OpCodes.Unbox_Any, typeof(TValue));
        }
    }
    
    private class UnboxingAsReferenceOperation<TValue>(ISymbol target) : 
        OperationSymbol<TValue>([target], ContentModifier.Reference)
    {
        public override void EmitContent()
        {
            target.EmitAsValue();
            Context.Code.Emit(OpCodes.Unbox, typeof(TValue));
        }
    }

    private class ConvertingToObject(ISymbol target) : OperationSymbol<object>([target])
    {
        public override void EmitContent()
            => target.EmitAsObject();
    }

    /// <summary>
    /// Box a value type into an object.
    /// </summary>
    /// <param name="symbol">Symbol to box.</param>
    /// <typeparam name="TContent">Type of the symbol.</typeparam>
    /// <returns>Boxing operation.</returns>
    public static OperationSymbol<object> Box<TContent>(this ISymbol<TContent> symbol)
        where TContent : struct
    {
        return new BoxingOperation(symbol);
    }
    
    /// <summary>
    /// Unbox a value type from an object.
    /// </summary>
    /// <param name="symbol">Object symbol to unbox.</param>
    /// <param name="asReference">
    /// If true, the unboxed symbol will contain a reference to the boxed value in the object.
    /// </param>
    /// <typeparam name="TContent">Value type to unbox.</typeparam>
    /// <returns>Unboxing operation.</returns>
    public static OperationSymbol<TContent> Unbox<TContent>(this ISymbol<object> symbol, 
        bool asReference = false)
        where TContent : struct
    {
        return asReference 
            ? new UnboxingAsReferenceOperation<TContent>(symbol) 
            : new UnboxingAsValueOperation<TContent>(symbol);
    }
    
    /// <summary>
    /// Convert the specified symbol to an object.
    /// If it is already an object, then the result will be the same as the input;
    /// otherwise, it will be boxed.
    /// </summary>
    /// <param name="symbol">Symbol to convert.</param>
    /// <returns>Conversion operation.</returns>
    public static OperationSymbol<object> ToObject(this ISymbol symbol)
        => new ConvertingToObject(symbol);
}