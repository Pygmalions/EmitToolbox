using EmitToolbox.Symbols;
using JetBrains.Annotations;

namespace EmitToolbox.Extensions;

[MustDisposeResource]
public class CodeBlock : IDisposable
{
    private readonly ILGenerator _code;
    
    private bool _disposed;
        
    /// <summary>
    /// Label pointing to the first instruction of this scope.
    /// </summary>
    public Label Beginning { get; }
        
    /// <summary>
    /// Label pointing to the last instruction of this scope.
    /// </summary>
    public Label Ending { get; }

    public CodeBlock(ILGenerator code)
    {
        _code = code;
        Beginning = _code.DefineLabel();
        Ending = _code.DefineLabel();
            
        _code.MarkLabel(Beginning);
    }

    public void Dispose()
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(BranchBlock));
        _disposed = true;
        _code.Emit(OpCodes.Nop);
        _code.MarkLabel(Ending);
    }

    /// <summary>
    /// Jump to the beginning label of this scope.
    /// </summary>
    public void GotoBegin() => _code.Emit(OpCodes.Br, Beginning);
    
    /// <summary>
    /// Jump to the beginning label of this scope if the specified condition is true.
    /// </summary>
    public void GotoBeginIfTrue(ISymbol<bool> condition)
    {
        condition.LoadAsValue();
        _code.Emit(OpCodes.Brtrue, Beginning);
    }
    
    /// <summary>
    /// Jump to the beginning label of this scope if the specified condition is false.
    /// </summary>
    public void GotoBeginIfFalse(ISymbol<bool> condition)
    {
        condition.LoadAsValue();
        _code.Emit(OpCodes.Brfalse, Beginning);
    }
        
    /// <summary>
    /// Jump to the ending label of this scope.
    /// </summary>
    public void GotoEnd() => _code.Emit(OpCodes.Br, Ending);

    /// <summary>
    /// Jump to the ending label of this scope if the specified condition is true.
    /// </summary>
    public void GotoEndIfTrue(ISymbol<bool> condition)
    {
        condition.LoadAsValue();
        _code.Emit(OpCodes.Brtrue, Ending);
    }
    
    /// <summary>
    /// Jump to the ending label of this scope if the specified condition is false.
    /// </summary>
    public void GotoEndIfFalse(ISymbol<bool> condition)
    {
        condition.LoadAsValue();
        _code.Emit(OpCodes.Brfalse, Ending);
    }
}

public static class CodeBlockExtensions
{
    [MustDisposeResource]
    public static LoopBlock Scope(this DynamicFunction function, ISymbol<bool>? condition = null)
        => new(function.Code, condition);
}