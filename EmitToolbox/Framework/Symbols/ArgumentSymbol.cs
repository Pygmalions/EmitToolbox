namespace EmitToolbox.Framework.Symbols;

public class ArgumentSymbol<TValue>(MethodBuildingContext context, int index, bool isReference = false)
    : VariableSymbol<TValue>(context, isReference)
{
    /// <summary>
    /// Index of this argument in the method argument list.
    /// </summary>
    public int Index { get; } = index;
    
    public override void EmitDirectlyStoreValue()
    {
        Context.Code.Emit(OpCodes.Starg, Index);
    }

    public override void EmitDirectlyLoadValue()
    {
        Context.Code.Emit(OpCodes.Ldarg, Index);
    }

    public override void EmitDirectlyLoadAddress()
    {
        Context.Code.Emit(OpCodes.Ldarga, Index);
    }
}