using System.Diagnostics.Contracts;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework.Extensions;

public static class BoxingExtensions
{
    internal class Boxing(ISymbol target)
        : OperationSymbol<object>(target.Context)
    {
        public override void LoadContent()
        {
            target.LoadAsValue();
            Context.Code.Emit(OpCodes.Box, target.BasicType);
        }
    }

    internal class UnboxingAsValue<TValue>(ISymbol target)
        : OperationSymbol<TValue>(target.Context)
    {
        public override void LoadContent()
        {
            target.LoadAsValue();
            Context.Code.Emit(OpCodes.Unbox_Any, typeof(TValue));
        }
    }

    internal class UnboxingAsReference<TValue>(ISymbol target)
        : OperationSymbol<TValue>(target.Context, ContentModifier.Reference)
    {
        public override void LoadContent()
        {
            target.LoadAsValue();
            Context.Code.Emit(OpCodes.Unbox, typeof(TValue));
        }
    }

    internal class ConvertingToObject(ISymbol target)
        : OperationSymbol<object>(target.Context)
    {
        public override void LoadContent()
            => target.LoadAsObject();
    }

    extension(ISymbol self)
    {
        public void LoadAsObject()
        {
            self.LoadAsValue();
            if (!self.BasicType.IsValueType)
                return;
            self.Context.Code.Emit(OpCodes.Box, self.BasicType);
        }
    }

    /// <summary>
    /// Box a value type into an object.
    /// </summary>
    /// <param name="symbol">Symbol to box.</param>
    /// <typeparam name="TContent">Type of the symbol.</typeparam>
    /// <returns>Boxing operation.</returns>
    [Pure]
    public static OperationSymbol<object> Box<TContent>(this ISymbol<TContent> symbol)
        where TContent : struct
    {
        return new Boxing(symbol);
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
    [Pure]
    public static OperationSymbol<TContent> Unbox<TContent>(this ISymbol<object> symbol,
        bool asReference = false)
        where TContent : struct
    {
        return asReference
            ? new UnboxingAsReference<TContent>(symbol)
            : new UnboxingAsValue<TContent>(symbol);
    }

    /// <summary>
    /// Convert the specified symbol to an object.
    /// If it is already an object, then the result will be the same as the input;
    /// otherwise, it will be boxed.
    /// </summary>
    /// <param name="symbol">Symbol to convert.</param>
    /// <returns>Conversion operation.</returns>
    [Pure]
    public static OperationSymbol<object> ToObject(this ISymbol symbol)
        => new ConvertingToObject(symbol);
}