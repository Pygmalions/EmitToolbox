using EmitToolbox.Framework.Extensions;

namespace EmitToolbox.Framework.Symbols;

public class VariableSymbol(DynamicMethod context, Type type, bool isPinned = false)
    : IAssignableSymbol, IAddressableSymbol
{
    public DynamicMethod Context { get; } = context;

    public Type ContentType { get; } = type;

    public LocalBuilder Builder { get; } = context.Code.DeclareLocal(type, isPinned);

    public void EmitContent()
        => Context.Code.Emit(OpCodes.Ldloc, Builder);
    
    public void EmitAddress()
        => Context.Code.Emit(OpCodes.Ldloca, Builder);

    public void Assign(ISymbol other)
    {
        other.EmitForSymbol(this);
        Context.Code.Emit(OpCodes.Stloc, Builder);
    }
    
    public void AssignContentFromStack()
        => Context.Code.Emit(OpCodes.Stloc, Builder);
    
    public VariableSymbol<TContent> AsSymbol<TContent>()
    {
        return !ContentType.IsAssignableTo(typeof(TContent)) 
            ? throw new InvalidCastException($"Type '{ContentType}' is not assignable to '{typeof(TContent)}'.")
            : new VariableSymbol<TContent>(this);
    }
}

public class VariableSymbol<TContent> : IAssignableSymbol<TContent>, IAddressableSymbol<TContent>
{
    private readonly VariableSymbol _symbol;
    
    internal VariableSymbol(VariableSymbol symbol)
    {
        _symbol = symbol;
    }
    
    public VariableSymbol(DynamicMethod context, ContentModifier? modifier = null, bool isPinned = false)
        : this(new VariableSymbol(context, modifier.Decorate<TContent>(), isPinned))
    {
    }

    public DynamicMethod Context => _symbol.Context;

    public Type ContentType => _symbol.ContentType;

    public void EmitContent() => _symbol.EmitContent();

    public void Assign(ISymbol<TContent> other) => _symbol.Assign(other);

    public void EmitAddress() => _symbol.EmitAddress();
    
    public void AssignContentFromStack() => _symbol.AssignContentFromStack();
}

public static class VariableSymbolExtensions
{
    extension(DynamicMethod self)
    {
        public VariableSymbol Variable(Type type, bool isPinned = false)
            => new(self, type, isPinned);

        public VariableSymbol<TContent> Variable<TContent>(ContentModifier? modifier = null, bool isPinned = false)
            => new(self, modifier, isPinned);
    }
}