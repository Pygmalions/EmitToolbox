using JetBrains.Annotations;

namespace EmitToolbox.Builders;

[MustDisposeResource]
public class GotoOnlyBlock : IDisposable
{
    private bool _disposed;

    private readonly CodeLabel _end;
    
    public GotoOnlyBlock(CodeLabel label)
    {
        _end = label.Context.DefineLabel();
        _end.Goto();
        label.Mark();
    }

    public void Dispose()
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(GotoOnlyBlock));
        GC.SuppressFinalize(this);
        _disposed = true;
        _end.Mark();
    }
}

public static class GotoOnlyBlockExtensions
{
    /// <summary>
    /// Mark this label in a scope that can only be jumped to.
    /// If the instructions are executed in sequence, then this scope will be skipped.
    /// </summary>
    /// <param name="label">Label to form a goto-only scope.</param>
    /// <returns>Goto-only block.</returns>
    [MustDisposeResource]
    public static GotoOnlyBlock MarkGotoOnlyScope(this CodeLabel label)
        => new(label);
}