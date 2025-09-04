using System.Diagnostics.CodeAnalysis;
using EmitToolbox.Framework.Symbols.Utilities;

namespace EmitToolbox.Framework.Symbols;

public abstract class VariableSymbol<TValue>(MethodBuildingContext context, bool isReference = false)
    : ValueSymbol<TValue>(context, isReference)
{
    [field: MaybeNull]
    protected Action<ILGenerator> ReferenceLoader =>
        field ??= ValueIndirectlyLoader.GetReferenceLoader(typeof(TValue));

    [field: MaybeNull]
    protected Action<ILGenerator> ReferenceStorer =>
        field ??= ValueIndirectlyStorer.GetReferenceStorer(typeof(TValue));

    /// <summary>
    /// Directly store the value from the stack into this value symbol,
    /// invoked when this value symbol is not a reference.
    /// </summary>
    public abstract void EmitDirectlyStoreValue();

    public sealed override void EmitLoadAsValue()
    {
        EmitDirectlyLoadValue();
        if (IsReference)
            ReferenceLoader(Context.Code);
    }

    public sealed override void EmitLoadAsAddress()
    {
        if (!IsReference)
            EmitDirectlyLoadAddress();
        else
            EmitDirectlyLoadValue();
    }

    /// <summary>
    /// Store the value from the stack into this variable symbol.
    /// </summary>
    public void EmitStoreFromValue()
    {
        if (!IsReference)
        {
            EmitDirectlyStoreValue();
            return;
        }
        
        TemporaryVariable.EmitStoreFromValue();
        EmitDirectlyLoadValue();
        TemporaryVariable.EmitLoadAsValue();
        ReferenceStorer(Context.Code);
    }
    
    public virtual void Assign<TTarget>(ValueSymbol<TTarget> value) where TTarget : TValue
    {
        value.EmitLoadAsValue();
        EmitStoreFromValue();
    }
}