using EmitToolbox.Extensions;

namespace EmitToolbox.Symbols;

public class VariableSymbol(DynamicFunction context, Type type, bool isPinned = false)
    : IAssignableSymbol, IAddressableSymbol
{
    public DynamicFunction Context { get; } = context;

    public Type ContentType { get; } = type;

    public LocalBuilder Builder { get; } = context.Code.DeclareLocal(type, isPinned);

    public void LoadContent()
        => Context.Code.Emit(OpCodes.Ldloc, Builder);

    public void LoadAddress()
        => Context.Code.Emit(OpCodes.Ldloca, Builder);

    public void AssignContent(ISymbol other)
    {
        other.LoadForSymbol(this);
        Context.Code.Emit(OpCodes.Stloc, Builder);
    }

    public void StoreContent()
        => Context.Code.Emit(OpCodes.Stloc, Builder);
    
    public VariableSymbol<TContent> AsSymbol<TContent>()
    {
        return this.BasicType != typeof(TContent)
            ? throw new InvalidCastException(
                $"Cannot convert a variable with basic type '{this.BasicType}' to '{typeof(TContent)}'.")
            : new VariableSymbol<TContent>(this);
    }
}

// This type does not inherit from VariableSymbol to not inherit the non-generic overloads.
public class VariableSymbol<TContent> : IAssignableSymbol<TContent>, IAddressableSymbol<TContent>
    where TContent : allows ref struct
{
    private readonly VariableSymbol _symbol;

    public VariableSymbol(VariableSymbol symbol)
    {
        if (symbol.BasicType != typeof(TContent))
            throw new ArgumentException(
                "Content type of the specified symbol does not match the generic type of this generic variable.");
        _symbol = symbol;
    }

    public VariableSymbol(DynamicFunction context, ContentModifier? modifier = null, bool isPinned = false)
        : this(new VariableSymbol(context, modifier.Decorate<TContent>(), isPinned))
    {
    }

    public DynamicFunction Context => _symbol.Context;

    public Type ContentType => _symbol.ContentType;

    public void LoadContent() => _symbol.LoadContent();

    public void AssignContent(ISymbol<TContent> other) => _symbol.AssignContent(other);

    public void LoadAddress() => _symbol.LoadAddress();

    public void StoreContent() => _symbol.StoreContent();
}

public static class VariableSymbolExtensions
{
    extension(DynamicFunction self)
    {
        public VariableSymbol Variable(Type type, bool isPinned = false)
            => new(self, type, isPinned);

        public VariableSymbol<TContent> Variable<TContent>(ContentModifier? modifier = null, bool isPinned = false)
            where TContent : allows ref struct
            => new(self, modifier, isPinned);
    }
}