namespace EmitToolbox.Framework.Symbols.Traits;

/// <summary>
/// Symbols implementing this interface can use optimized instructions to
/// assign values to other assignable symbols.
/// </summary>
public interface ISpecialAssignor : ISymbol
{
    void AssignTo(IAssignableSymbol symbol);
}

public interface ISpecialAssignor<TValue> : ISpecialAssignor, ISymbol<TValue>
{
    void AssignTo(IAssignableSymbol<TValue> symbol);
}