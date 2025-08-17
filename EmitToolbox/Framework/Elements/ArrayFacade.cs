namespace EmitToolbox.Framework.Elements;

public class ArrayFacade<TElement>(ValueElement<TElement[]> array) 
    : ValueElement<TElement[]>(array.Context)
{
    protected internal override void EmitLoadAsValue() => array.EmitLoadAsValue();

    protected internal override void EmitLoadAsAddress() => array.EmitLoadAsAddress();

    public ValueElement<TElement> this[int index]
    {
        get
        {
            var value = Context.DefineVariable<TElement>();
            
            array.EmitLoadAsValue();
            Context.Code.Emit(OpCodes.Ldc_I4, index);
            Context.Code.Emit(OpCodes.Ldelem, typeof(TElement));
            value.EmitStoreValue();

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
    
    public ValueElement<TElement> this[ValueElement<int> index]
    {
        get
        {
            var value = Context.DefineVariable<TElement>();
            
            array.EmitLoadAsValue();
            index.EmitLoadAsValue();
            Context.Code.Emit(OpCodes.Ldelem, typeof(TElement));
            value.EmitStoreValue();

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