using EmitToolbox.Framework.Extensions;

namespace EmitToolbox.Framework.Symbols.Operations;

/// <summary>
/// Emit the specified instruction after emitting the specified symbols as values.
/// </summary>
public class InstructionOperation(OpCode instruction, Type contentType, IReadOnlyCollection<ISymbol> symbols)
    : OperationSymbol(symbols, contentType)
{
    public override void LoadContent()
    {
        foreach (var symbol in symbols)
            symbol.LoadAsValue();
        Context.Code.Emit(instruction);
    }
}

public class InstructionOperation<TResult>(
    OpCode instruction,
    IReadOnlyCollection<ISymbol> symbols,
    ContentModifier? modifier = null)
    : InstructionOperation(instruction, modifier.Decorate<TResult>(), symbols), IOperationSymbol<TResult>
    where TResult : allows ref struct
{
}