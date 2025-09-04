using System.Runtime.CompilerServices;

namespace EmitToolbox.Framework.Symbols;

public abstract class LiteralValueSymbol<TValue>(MethodBuildingContext context, TValue value)
    : ValueSymbol<TValue>(context)
{
    /// <summary>
    /// Value of this literal value symbol.
    /// </summary>
    public TValue Value { get; } = value;

    public override void EmitDirectlyLoadAddress()
    {
        EmitLoadAsValue();
        TemporaryVariable.EmitStoreFromValue();
        TemporaryVariable.EmitLoadAsAddress();
    }
    
    public sealed override void EmitLoadAsValue()
        => EmitDirectlyLoadValue();

    public sealed override void EmitLoadAsAddress()
        => EmitDirectlyLoadAddress();
}