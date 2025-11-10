using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework.Extensions;

public static class DisposableExtensions
{
    public static void InvokeDispose<TContent>(this ISymbol<TContent> self) where TContent : IDisposable
    {
        self.LoadAsTarget();
        self.Context.Code.Emit(OpCodes.Callvirt, 
            typeof(IDisposable).GetMethod(nameof(IDisposable.Dispose))!);
    }
}