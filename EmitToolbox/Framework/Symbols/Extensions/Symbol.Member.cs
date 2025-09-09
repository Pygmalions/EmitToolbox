namespace EmitToolbox.Framework.Symbols.Extensions;

public static class SymbolMemberExtensions
{
    public static void InvokeMethod(this ISymbol symbol, MethodInfo method, params Span<ISymbol> arguments)
    {
        var parameters = method.GetParameters();
        if (parameters.Length != arguments.Length)
            throw new ArgumentException(
                "The number of arguments does not match the number of parameters.", nameof(arguments));
        
        if (!method.IsStatic)
            symbol.EmitLoadAsTarget();

        foreach (var (index, parameter) in parameters.Index())
            arguments[index].EmitLoadAsParameter(parameter);
        
        symbol.Context.Code.Emit(method.IsStatic ? OpCodes.Call : OpCodes.Callvirt, method);
    }
}