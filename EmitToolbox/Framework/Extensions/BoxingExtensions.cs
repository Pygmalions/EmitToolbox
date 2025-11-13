using System.Diagnostics.Contracts;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework.Extensions;

public static class BoxingExtensions
{
    private class Boxing(ISymbol target) : OperationSymbol<object>(target.Context)
    {
        public override void LoadContent()
        {
            target.LoadAsValue();
            Context.Code.Emit(OpCodes.Box, target.BasicType);
        }
    }

    private class UnboxingAsValue(ISymbol target, Type type) : OperationSymbol(target.Context, type)
    {
        public override void LoadContent()
        {
            target.LoadAsValue();
            Context.Code.Emit(OpCodes.Unbox_Any, ContentType);
        }
    }

    private class UnboxingAsReference(ISymbol target, Type type)
        : OperationSymbol(target.Context, type.MakeByRefType())
    {
        public override void LoadContent()
        {
            target.LoadAsValue();
            Context.Code.Emit(OpCodes.Unbox, type);
        }
    }

    private class ConvertingToObject(ISymbol target) : OperationSymbol<object>(target.Context)
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

        /// <summary>
        /// Unbox a value type from an object.
        /// </summary>
        /// <param name="type">Value type to unbox.</param>
        /// <param name="asReference">
        /// If true, the unboxed symbol will contain a reference to the boxed value in the object.
        /// </param>
        /// <returns>Unboxing operation.</returns>
        [Pure]
        public IOperationSymbol Unbox(Type type, bool asReference = false)
        {
            if (self.BasicType != typeof(object))
                throw new ArgumentException($"Cannot unbox from '{self.BasicType.Name}'.");
            if (!type.IsValueType)
                throw new ArgumentException(
                    $"Specified type '{type}' to unbox is not a value type.", nameof(type));
            return asReference
                ? new UnboxingAsReference(self, type)
                : new UnboxingAsValue(self, type);
        }

        /// <summary>
        /// Convert the specified symbol to an object.
        /// If it is already an object, then the result will be the same as the input;
        /// otherwise, it will be boxed.
        /// </summary>
        [Pure]
        public IOperationSymbol<object> ToObject()
        {
            return self.BasicType.IsValueType ? new ConvertingToObject(self) : self.CastTo<object>();
        }
    }

    /// <summary>
    /// Box a value type into an object.
    /// </summary>
    /// <param name="symbol">Symbol to box.</param>
    /// <typeparam name="TContent">Type of the symbol.</typeparam>
    /// <returns>Boxing operation.</returns>
    [Pure]
    public static OperationSymbol<object> Box<TContent>(this ISymbol<TContent> symbol) where TContent : struct
        => new Boxing(symbol);

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
    public static IOperationSymbol<TContent> Unbox<TContent>(
        this ISymbol<object> symbol,
        bool asReference = false) where TContent : struct
    {
        OperationSymbol operation = asReference
            ? new UnboxingAsReference(symbol, typeof(TContent))
            : new UnboxingAsValue(symbol, typeof(TContent));
        return operation.AsSymbol<TContent>();
    }
}