using EmitToolbox.Framework.Extensions;

namespace EmitToolbox.Framework.Symbols.Operations;

/// <summary>
/// Emit the specified instruction after emitting the specified symbols as values.
/// </summary>
public class InstructionOperation<TResult>(OpCode instruction, IReadOnlyCollection<ISymbol> symbols) 
    : OperationSymbol<TResult>(symbols)
{
    public override void EmitContent()
    {
        foreach (var symbol in symbols)
            symbol.EmitAsValue();
        Context.Code.Emit(instruction);
    }
}