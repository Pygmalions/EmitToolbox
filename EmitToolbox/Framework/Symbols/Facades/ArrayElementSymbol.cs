using EmitToolbox.Framework.Symbols.Extensions;

namespace EmitToolbox.Framework.Symbols.Facades;

public class ArrayElementSymbol<TElement>(ISymbol array, ISymbol<int> index)
    : IAssignableSymbol<TElement>, IAddressableSymbol<TElement>
{
    public DynamicMethod Context => array.Context;

    public Type ValueType { get; } = typeof(TElement);
    
    public void EmitLoadContent()
    {
        array.EmitLoadAsValue();
        index.EmitLoadAsValue();
        if (ValueType.IsValueType)
            Context.Code.Emit(OpCodes.Ldelem, typeof(TElement));
        else
            Context.Code.Emit(OpCodes.Ldelem_Ref);
    }

    public void EmitStoreContent()
    {
        var temporary = Context.Variable<TElement>();
        temporary.EmitStoreFromValue();
        
        array.EmitLoadAsValue();
        index.EmitLoadAsValue();
        temporary.EmitLoadAsValue();
        if (ValueType.IsValueType)
            Context.Code.Emit(OpCodes.Ldelem, typeof(TElement));
        else
            Context.Code.Emit(OpCodes.Ldelem_Ref);
    }

    public void EmitLoadAddress()
    {
        array.EmitLoadAsValue();
        index.EmitLoadAsValue();
        Context.Code.Emit(OpCodes.Ldelema, typeof(TElement));
    }
}