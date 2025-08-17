using EmitToolbox.Framework.Elements;

namespace EmitToolbox.Framework;

public partial class MethodContext
{
    internal MethodContext(ILGenerator code)
    {
        Code = code;
    }

    internal ILGenerator Code { get; }
    
    public VariableElement<TVariable> DefineVariable<TVariable>()
    {
        return new LocalVariableElement<TVariable>(this);
    }
    
    public ArrayFacade<TElement> NewArray<TElement>(int length)
    {
        Code.Emit(OpCodes.Ldc_I4, length);
        Code.Emit(OpCodes.Newarr, typeof(TElement));
        var array = DefineVariable<TElement[]>();
        array.EmitStoreValue();
        return new ArrayFacade<TElement>(array);
    }
    
    public ArrayFacade<TElement> NewArray<TElement>(ValueElement<int> length)
    {
        length.EmitLoadAsValue();
        Code.Emit(OpCodes.Newarr, typeof(TElement));
        var array = DefineVariable<TElement[]>();
        array.EmitStoreValue();
        return new ArrayFacade<TElement>(array);
    }
}