namespace EmitToolbox.Framework.Symbols.Operations;

/// <summary>
/// This operation does nothing.
/// It can be used to wrap a symbol into another generic representation.
/// </summary>
public class NoOperation<TTarget>(ISymbol target) :
    OperationSymbol<TTarget>([target], ContentModifier.Parse(target.ContentType))
    where TTarget : allows ref struct
{
    public override void LoadContent()
    {
        target.LoadContent();
    }
}

public static class NoOperationExtensions
{
    extension(ISymbol self)
    {
        /// <summary>
        /// Wrap this symbol into another type.
        /// No operation (such as boxing, conversion) will be performed.
        /// This method is useful to wrap an untyped symbol into a typed symbol.
        /// However,
        /// the behavior is correct only when the actual content type can be processed as the target type
        /// in the following operations;
        /// otherwise, the behavior is undefined.
        /// </summary>
        /// <typeparam name="TTarget">Target type to wrap this symbol into.</typeparam>
        public NoOperation<TTarget> AsSymbol<TTarget>() where TTarget : allows ref struct
            => new(self); 
    }
}