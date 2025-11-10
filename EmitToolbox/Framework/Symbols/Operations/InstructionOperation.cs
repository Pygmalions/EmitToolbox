using EmitToolbox.Framework.Extensions;

namespace EmitToolbox.Framework.Symbols.Operations;

/// <summary>
/// Emit the specified instruction after emitting the specified symbols as values.
/// </summary>
public class InstructionOperation<TResult>(OpCode instruction, IReadOnlyCollection<ISymbol> symbols) 
    : OperationSymbol<TResult>(symbols)
{
    public override void LoadContent()
    {
        foreach (var symbol in symbols)
            symbol.LoadAsValue();
        Context.Code.Emit(instruction);
    }
}