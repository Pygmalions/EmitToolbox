using EmitToolbox.Symbols;
using JetBrains.Annotations;

namespace EmitToolbox.Extensions;

[MustDisposeResource]
public class LoopBlock : IDisposable
{
    private readonly ILGenerator _code;
    
    private bool _disposed;
        
    /// <summary>
    /// Label pointing to the first instruction of this scope.
    /// </summary>
    public Label Entry { get; }
        
    /// <summary>
    /// Label pointing to the last instruction of this scope.
    /// </summary>
    public Label Exit { get; }

    /// <summary>
    /// Construct a loop scope.
    /// </summary>
    /// <param name="code">IL stream to construct a scope in.</param>
    /// <param name="condition">
    /// Optional looping condition.
    /// If null, then the loop will only stop when any of <see cref="Break"/>,
    /// <see cref="BreakIfTrue"/>, <see cref="BreakIfFalse"/> is called.
    /// </param>
    /// <param name="whenConditionIs">
    /// The loop will only continue when the condition is equal to this value,
    /// </param>
    public LoopBlock(
        ILGenerator code, 
        ISymbol<bool>? condition = null, 
        bool whenConditionIs = true)
    {
        _code = code;
        Entry = _code.DefineLabel();
        Exit = _code.DefineLabel();
            
        _code.MarkLabel(Entry);

        if (condition == null) 
            return;
        // Stop the loop when the condition is not equal to the required value.
        condition.LoadAsValue();
        _code.Emit(whenConditionIs ? OpCodes.Brfalse : OpCodes.Brtrue, Exit);
    }

    public void Dispose()
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(BranchBlock));
        _disposed = true;
        _code.Emit(OpCodes.Br,Entry);
        _code.MarkLabel(Exit);
    }

    /// <summary>
    /// Go back to the beginning of this loop scope.
    /// </summary>
    public void Continue() => _code.Emit(OpCodes.Br, Entry);
    
    /// <summary>
    /// Go back to the beginning of this loop scope if the specified condition is true.
    /// </summary>
    public void ContinueIfTrue(ISymbol<bool> condition)
    {
        condition.LoadAsValue();
        _code.Emit(OpCodes.Brtrue, Entry);
    }
    
    /// <summary>
    /// Go back to the beginning of this loop scope if the specified condition is false.
    /// </summary>
    public void ContinueIfFalse(ISymbol<bool> condition)
    {
        condition.LoadAsValue();
        _code.Emit(OpCodes.Brfalse, Entry);
    }
        
    /// <summary>
    /// Exit this loop scope.
    /// </summary>
    public void Break() => _code.Emit(OpCodes.Br, Exit);

    /// <summary>
    /// Exit this loop scope if the specified condition is true.
    /// </summary>
    public void BreakIfTrue(ISymbol<bool> condition)
    {
        condition.LoadAsValue();
        _code.Emit(OpCodes.Brtrue, Exit);
    }
    
    /// <summary>
    /// Exit this loop scope if the specified condition is false.
    /// </summary>
    public void BreakIfFalse(ISymbol<bool> condition)
    {
        condition.LoadAsValue();
        _code.Emit(OpCodes.Brfalse, Exit);
    }
}

public static class LoopBlockExtensions
{
    extension(DynamicFunction function)
    {
        [MustDisposeResource]
        public LoopBlock Loop()
            => new(function.Code);

        [MustDisposeResource]
        public LoopBlock LoopWhenTrue(ISymbol<bool> condition)
            => new(function.Code, condition);

        [MustDisposeResource]
        public LoopBlock LoopWhenFalse(ISymbol<bool>? condition)
            => new(function.Code, condition, false);

        [MustDisposeResource]
        public LoopBlock While(ISymbol<bool> condition)
            => new(function.Code, condition);
    }
}