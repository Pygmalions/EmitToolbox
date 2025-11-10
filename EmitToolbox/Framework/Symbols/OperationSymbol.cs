namespace EmitToolbox.Framework.Symbols;

/// <summary>
/// The operation contained in an operation symbol is executed everytime when and only when its content is emitted
/// through <see cref="LoadContent"/> method.
/// </summary>
public abstract class OperationSymbol<TProduction>(DynamicMethod context, ContentModifier? modifier = null)
    : ISymbol<TProduction>
    where TProduction : allows ref struct
{
    protected OperationSymbol(IEnumerable<ISymbol> symbols, ContentModifier? modifier = null)
        : this(CrossContextException.EnsureContext(symbols), modifier)
    {
    }

    public Type ContentType { get; } = modifier.Decorate<TProduction>();

    public DynamicMethod Context { get; } = context;

    public abstract void LoadContent();
}

public static class OperationSymbolExtensions
{
    extension<TProduction>(OperationSymbol<TProduction> self)
    {
        /// <summary>
        /// Execute the operation and store the result into the specified symbol.
        /// </summary>
        /// <param name="target">Writable symbol to store the result into.</param>
        public void ToSymbol(IAssignableSymbol<TProduction> target)
        {
            target.AssignContent(self);
        }

        /// <summary>
        /// Execute the operation and store the result into a new variable.
        /// </summary>
        /// <returns>Newly defined variable symbol that stores the result of the operation.</returns>
        public VariableSymbol<TProduction> ToSymbol()
        {
            var variable = self.Context.Variable<TProduction>();
            self.ToSymbol(variable);
            return variable;
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
}