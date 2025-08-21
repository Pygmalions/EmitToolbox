namespace EmitToolbox.Framework.Symbols.Facades;

public class ArrayFacade<TElement>(ValueSymbol<TElement[]> array) : ValueSymbol<TElement[]>(array.Context)
{
    protected internal override void EmitDirectlyLoadValue() => array.EmitDirectlyLoadValue();

    protected internal override void EmitDirectlyLoadAddress() => array.EmitDirectlyLoadAddress();

    protected internal override void EmitLoadAsValue() => array.EmitLoadAsValue();

    protected internal override void EmitLoadAsAddress() => array.EmitLoadAsAddress();

    public ArrayElementSymbol<TElement> this[int index]
        => new(array, Context.Value(index));

    public ArrayElementSymbol<TElement> this[ValueSymbol<int> index]
        => new(this, index);

    public void Assign<TValue>(int index, ValueSymbol<TValue> value) where TValue : TElement
    {
        array.EmitLoadAsValue();
        Context.Code.Emit(OpCodes.Ldc_I4, index);
        value.EmitLoadAsValue();
        Context.Code.Emit(OpCodes.Stelem, typeof(TElement));
    }
    
    public void Assign<TValue>(ValueSymbol<int> index, ValueSymbol<TValue> value) where TValue : TElement
    {
        array.EmitLoadAsValue();
        index.EmitLoadAsValue();
        value.EmitLoadAsValue();
        Context.Code.Emit(OpCodes.Stelem, typeof(TElement));
    }
}

public static class ArrayFacadeExtensions
{
    public static ArrayFacade<TElement> AsArray<TElement>(this ValueSymbol<TElement[]> array)
        => new(array);
}