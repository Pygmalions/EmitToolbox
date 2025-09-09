namespace EmitToolbox.Framework.Symbols.Traits;

/// <summary>
/// Symbols implementing this interface can use optimized instructions to
/// be assigned from other symbols.
/// </summary>
public interface ISpecialAssignee : IAssignableSymbol
{
    void AssignFrom(ISymbol symbol);
}

public interface ISpecialAssignee<TValue> : ISpecialAssignee, IAssignableSymbol<TValue>
{
    void AssignFrom(ISymbol<TValue> symbol);
}