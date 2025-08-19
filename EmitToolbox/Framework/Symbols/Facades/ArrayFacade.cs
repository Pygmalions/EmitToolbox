namespace EmitToolbox.Framework.Symbols.Facades;

public class ArrayFacade<TElement>(ValueSymbol<TElement[]> array) : ValueSymbol<TElement[]>(array.Context)
{
    protected internal override void EmitDirectlyLoadValue() => array.EmitDirectlyLoadValue();

    protected internal override void EmitDirectlyLoadAddress() => array.EmitDirectlyLoadAddress();

    protected internal override void EmitLoadAsValue() => array.EmitLoadAsValue();

    protected internal override void EmitLoadAsAddress() => array.EmitLoadAsAddress();

    public VariableSymbol<TElement> this[int index]
    {
        get
        {
            var value = Context.Variable<TElement>();
            
            array.EmitLoadAsValue();
            Context.Code.Emit(OpCodes.Ldc_I4, index);
            Context.Code.Emit(OpCodes.Ldelem, typeof(TElement));
            value.EmitStoreFromValue();

            return value;
        }

        set
        {
            array.EmitLoadAsValue();
            Context.Code.Emit(OpCodes.Ldc_I4, index);
            value.EmitLoadAsValue();
            Context.Code.Emit(OpCodes.Stelem, typeof(TElement));
        }
    }
    
    public VariableSymbol<TElement> this[ValueSymbol<int> index]
    {
        get
        {
            var value = Context.Variable<TElement>();
            
            array.EmitLoadAsValue();
            index.EmitLoadAsValue();
            Context.Code.Emit(OpCodes.Ldelem, typeof(TElement));
            value.EmitStoreFromValue();

            return value;
        }
        
        set
        {
            array.EmitLoadAsValue();
            index.EmitLoadAsValue();
            value.EmitLoadAsValue();
            Context.Code.Emit(OpCodes.Stelem, typeof(TElement));
        }
    }
}