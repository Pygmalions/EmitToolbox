namespace EmitToolbox.Framework.Symbols.Facades;

public class ArrayElementSymbol<TElement>(ValueSymbol<TElement[]> array, ValueSymbol<int> index)
    : VariableSymbol<TElement>(array.Context)
{
    protected internal override void EmitDirectlyLoadValue()
    {
        array.EmitLoadAsValue();
        index.EmitLoadAsValue();
        Context.Code.Emit(OpCodes.Ldelem, typeof(TElement));
    }

    protected internal override void EmitDirectlyLoadAddress()
    {
        array.EmitLoadAsValue();
        index.EmitLoadAsValue();
        Context.Code.Emit(OpCodes.Ldelema, typeof(TElement));
    }

    protected override void EmitDirectlyStoreValue()
    {
        TemporaryVariable.EmitStoreFromValue();
        array.EmitLoadAsValue();
        index.EmitLoadAsValue();
        TemporaryVariable.EmitLoadAsValue();
        Context.Code.Emit(OpCodes.Stelem, typeof(TElement));
    }
}