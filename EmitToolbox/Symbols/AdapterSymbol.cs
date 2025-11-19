using System.Runtime.CompilerServices;
using EmitToolbox.Utilities;

namespace EmitToolbox.Symbols;

public class AdapterSymbol<TContent> : IOperationSymbol<TContent>
    where TContent : allows ref struct
{
    private readonly ISymbol _symbol;

    public AdapterSymbol(ISymbol symbol)
    {
        if (!typeof(TContent).IsDirectlyAssignableFrom(symbol.BasicType))
            throw new ArgumentException(
                $"The content type of the specified symbol '{symbol.BasicType}' mismatch " +
                "the generic type of this adapter.");
        _symbol = symbol;
    }

    public Type ContentType => _symbol.ContentType;

    public DynamicFunction Context => _symbol.Context;

    public void LoadContent() => _symbol.LoadContent();
}

public class AssignableAdapterSymbol<TContent>(IAssignableSymbol symbol)
    : AdapterSymbol<TContent>(symbol), IAssignableSymbol<TContent>
    where TContent : allows ref struct
{
    public void StoreContent() => symbol.StoreContent();

    public void AssignContent(ISymbol<TContent> other) => symbol.AssignContent(other);
}

public class AddressableAdapterSymbol<TContent>(IAddressableSymbol symbol)
    : AdapterSymbol<TContent>(symbol), IAddressableSymbol<TContent>
    where TContent : allows ref struct
{
    public void LoadAddress() => symbol.LoadAddress();
}

public static class SymbolAdapterExtensions
{
    extension(ISymbol symbol)
    {
        [OverloadResolutionPriority(-3)]
#nullable disable
        public AdapterSymbol<TContent> AsSymbol<TContent>()
#nullable restore
            where TContent : allows ref struct
            => new(symbol);
    }

    [OverloadResolutionPriority(-2)]
#nullable disable
    public static AssignableAdapterSymbol<TContent> AsSymbol<TContent>(this IAssignableSymbol symbol)
#nullable restore
        where TContent : allows ref struct
        => new(symbol);

    [OverloadResolutionPriority(-1)]
#nullable disable
    public static AddressableAdapterSymbol<TContent> AsSymbol<TContent>(this IAddressableSymbol symbol)
#nullable restore
        where TContent : allows ref struct
        => new(symbol);
}