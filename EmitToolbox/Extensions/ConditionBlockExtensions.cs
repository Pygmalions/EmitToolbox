using EmitToolbox.Symbols;
using JetBrains.Annotations;

namespace EmitToolbox.Extensions;

[MustDisposeResource]
public class BranchBlock : IDisposable
{
    private readonly ILGenerator _code;

    private readonly Label _labelEnd;

    private bool _disposed;

    public BranchBlock(ILGenerator code, ISymbol<bool> condition, bool whenConditionIs = true)
    {
        _code = code;

        _labelEnd = code.DefineLabel();

        condition.LoadAsValue();
        _code.Emit(whenConditionIs ? OpCodes.Brfalse : OpCodes.Brtrue, _labelEnd);
    }

    public void Dispose()
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(BranchBlock));
        _disposed = true;
        _code.Emit(OpCodes.Nop);
        _code.MarkLabel(_labelEnd);
    }
}

public static class ConditionBlockExtensions
{
    extension(DynamicFunction self)
    {
        /// <summary>
        /// Code in this scope will only be executed when the condition is true.
        /// </summary>
        [MustDisposeResource]
        public BranchBlock If(ISymbol<bool> condition)
            => new(self.Code, condition);

        /// <summary>
        /// Code in this scope will only be executed when the condition is false.
        /// </summary>
        [MustDisposeResource]
        public BranchBlock IfNot(ISymbol<bool> condition)
            => new(self.Code, condition, false);


        /// <summary>
        /// Code in this scope will only be executed when the condition is false.
        /// </summary>
        public void IfElse(ISymbol<bool> condition, Action onTrue, Action onFalse)
        {
            var code = self.Code;
            var labelElse = code.DefineLabel();
            var labelEnd = code.DefineLabel();
            
            condition.LoadAsValue();
            code.Emit(OpCodes.Brfalse, labelElse);
            
            // True:
            onTrue();
            code.Emit(OpCodes.Br, labelEnd);
            
            // False:
            code.MarkLabel(labelElse);
            onFalse();
            
            code.MarkLabel(labelEnd);
        }
    }
}