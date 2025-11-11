using EmitToolbox.Framework.Extensions;

namespace EmitToolbox.Framework.Symbols;

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
        return !ContentType.IsAssignableTo(typeof(TContent)) 
            ? throw new InvalidCastException($"Type '{ContentType}' is not assignable to '{typeof(TContent)}'.")
            : new VariableSymbol<TContent>(this);
    }
}

public class VariableSymbol<TContent> : IAssignableSymbol<TContent>, IAddressableSymbol<TContent>
    where TContent : allows ref struct
{
    private readonly VariableSymbol _symbol;
    
    internal VariableSymbol(VariableSymbol symbol)
    {
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