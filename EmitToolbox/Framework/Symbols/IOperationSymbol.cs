namespace EmitToolbox.Framework.Symbols;

/// <summary>
/// The operation contained in an operation symbol is executed everytime when and only when its content is emitted
/// through <see cref="ISymbol.LoadContent"/> method.
/// </summary>
/// <remarks>Implement this interface to allow using operation extension methods.</remarks>
public interface IOperationSymbol : ISymbol
{
}

/// <summary>
/// The operation contained in an operation symbol is executed everytime when and only when its content is emitted
/// through <see cref="ISymbol.LoadContent"/> method.
/// </summary>
/// <remarks>Implement this interface to allow using operation extension methods.</remarks>
public interface IOperationSymbol<out TContent> : IOperationSymbol, ISymbol<TContent>
    where TContent : allows ref struct
{
}

/// <summary>
/// Base class for quickly implement operation symbols.
/// </summary>
public abstract class OperationSymbol(DynamicFunction context, Type contentType)
    : IOperationSymbol
{
    protected OperationSymbol(IEnumerable<ISymbol> symbols, Type contentType)
        : this(CrossContextException.EnsureContext(symbols), contentType)
    {
    }

    public Type ContentType { get; } = contentType;

    public DynamicFunction Context { get; } = context;

    public abstract void LoadContent();
}

/// <summary>
/// Base class for quickly implement operation symbols.
/// </summary>
public abstract class OperationSymbol<TResult>(DynamicFunction context, ContentModifier? modifier = null)
    : OperationSymbol(context, modifier.Decorate<TResult>()), IOperationSymbol<TResult>
    where TResult : allows ref struct
{
    protected OperationSymbol(IEnumerable<ISymbol> symbols, ContentModifier? modifier = null)
        : this(CrossContextException.EnsureContext(symbols), modifier)
    {
    }
}

public static class OperationSymbolExtensions
{
    extension(IOperationSymbol self)
    {
        /// <summary>
        /// Execute the operation and store the result into the specified symbol.
        /// </summary>
        /// <param name="target">Writable symbol to store the result into.</param>
        public void ToSymbol(IAssignableSymbol target)
        {
            target.AssignContent(self);
        }

        /// <summary>
        /// Execute the operation and discard the result.
        /// </summary>
        public void DiscardResult()
        {
            self.LoadContent();
            self.Context.Code.Emit(OpCodes.Pop);
        }
    }

    extension<TContent>(IOperationSymbol<TContent> self)
    {
        /// <summary>
        /// Execute the operation and store the result into the specified symbol.
        /// </summary>
        /// <param name="target">Writable symbol to store the result into.</param>
        public void ToSymbol(IAssignableSymbol<TContent> target)
        {
            target.AssignContent(self);
        }

        /// <summary>
        /// Execute the operation and store the result into a new variable.
        /// </summary>
        /// <returns>Newly defined variable symbol that stores the result of the operation.</returns>
        public VariableSymbol<TContent> ToSymbol()
        {
            var variable = self.Context.Variable<TContent>();
            self.ToSymbol(variable);
            return variable;
        }
    }
}