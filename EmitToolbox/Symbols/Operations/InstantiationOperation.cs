using EmitToolbox.Extensions;
using EmitToolbox.Utilities;

namespace EmitToolbox.Symbols.Operations;

public class InstantiationOperation(ConstructorInfo constructor, IReadOnlyCollection<ISymbol> arguments)
    : OperationSymbol(arguments,
        constructor.DeclaringType ??
        throw new ArgumentException(
            "Cannot determine the content type of this operation: " +
            "specified constructor does not have a declaring type."))
{
    public override void LoadContent()
    {
        foreach (var (parameter, argument) in constructor.GetParameters().Zip(arguments))
            argument.LoadForParameter(parameter);
        Context.Code.Emit(OpCodes.Newobj, constructor);
    }
}

public class InstantiationOperation<TContent> : OperationSymbol<TContent>
{
    private readonly ConstructorInfo _constructor;
    
    private readonly IReadOnlyCollection<ISymbol> _arguments;
    
    public InstantiationOperation(ConstructorInfo constructor, IReadOnlyCollection<ISymbol> arguments)
        : base(arguments)
    {
        _constructor = constructor;
        _arguments = arguments;
        if (_constructor.DeclaringType is { } type && !type.IsDirectlyAssignableTo(typeof(TContent)))
            throw new ArgumentException(
                $"Declaring type of the specified constructor '{type}' is not directly assignable to " +
                $"the specified representation type '{typeof(TContent)}'.");
    }
    
    public override void LoadContent()
    {
        foreach (var (parameter, argument) in _constructor.GetParameters().Zip(_arguments))
            argument.LoadForParameter(parameter);
        Context.Code.Emit(OpCodes.Newobj, _constructor);
    }
}