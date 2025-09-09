using EmitToolbox.Framework.Symbols.Extensions;

namespace EmitToolbox.Framework.Symbols.Facades;

public class ArrayFacade<TElement>(ISymbol<TElement[]> array) : ISymbol<TElement[]>
{
    public DynamicMethod Context => array.Context;
    
    public Type ValueType => array.ValueType;
    
    public Type ElementType { get; } = array.ValueType.GetElementType()!;
    
    public void EmitLoadContent() => array.EmitLoadContent();

    public ArrayElementSymbol<TElement> this[int index]
        => new(array, Context.Value(index));

    public ArrayElementSymbol<TElement> this[ISymbol<int> index]
        => new(this, index);

    public void SetElement<TValue>(int index, ISymbol<TValue> value) where TValue : TElement
    {
        array.EmitLoadAsValue();
        Context.Code.Emit(OpCodes.Ldc_I4, index);
        value.EmitLoadAsValue();
        Context.Code.Emit(OpCodes.Stelem, typeof(TElement));
    }
    
    public void SetElement<TValue>(ISymbol<int> index, ISymbol<TValue> value) where TValue : TElement
    {
        array.EmitLoadAsValue();
        index.EmitLoadAsValue();
        value.EmitLoadAsValue();
        Context.Code.Emit(OpCodes.Stelem, typeof(TElement));
    }
    
    public VariableSymbol<TElement> GetElement(int index)
    {
        var element = Context.Variable<TElement>();
        array.EmitLoadAsValue();
        Context.Code.Emit(OpCodes.Ldc_I4, index);
        if (ElementType.IsValueType)
            Context.Code.Emit(OpCodes.Ldelem, typeof(TElement));
        else
            Context.Code.Emit(OpCodes.Ldelem_Ref);
        element.EmitStoreFromValue();
        return element;
    }
    
    public VariableSymbol<TElement> GetElement(ISymbol<int> index)
    {
        var element = Context.Variable<TElement>();
        array.EmitLoadAsValue();
        index.EmitLoadAsValue();
        if (ElementType.IsValueType)
            Context.Code.Emit(OpCodes.Ldelem, typeof(TElement));
        else
            Context.Code.Emit(OpCodes.Ldelem_Ref);
        element.EmitStoreFromValue();
        return element;
    }
}

public static class ArrayFacadeExtensions
{
    public static ArrayFacade<TElement> AsArray<TElement>(this ISymbol<TElement[]> array)
        => new(array);
}