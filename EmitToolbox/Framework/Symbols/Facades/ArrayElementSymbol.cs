namespace EmitToolbox.Framework.Symbols.Facades;

public class ArrayElementSymbol<TElement>(ValueSymbol<TElement[]> array, ValueSymbol<int> index)
    : VariableSymbol<TElement>(array.Context)
{
    public override void EmitDirectlyLoadValue()
    {
        array.EmitLoadAsValue();
        index.EmitLoadAsValue();
        Context.Code.Emit(OpCodes.Ldelem, typeof(TElement));
    }

    public override void EmitDirectlyLoadAddress()
    {
        array.EmitLoadAsValue();
        index.EmitLoadAsValue();
        Context.Code.Emit(OpCodes.Ldelema, typeof(TElement));
    }

    public override void EmitDirectlyStoreValue()
    {
        TemporaryVariable.EmitStoreFromValue();
        array.EmitLoadAsValue();
        index.EmitLoadAsValue();
        TemporaryVariable.EmitLoadAsValue();
        Context.Code.Emit(OpCodes.Stelem, typeof(TElement));
    }
}