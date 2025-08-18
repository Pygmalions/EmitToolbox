using EmitToolbox.Framework.Elements;

namespace EmitToolbox.Framework;

public abstract partial class MethodContext(ILGenerator code)
{
    public ILGenerator Code { get; } = code;

    public ArgumentElement<TArgument> RetrieveArgument<TArgument>(int index, bool isReference = false)
    {
        return new ArgumentElement<TArgument>(this, index, isReference);
    }
    
    public VariableElement<TVariable> DefineVariable<TVariable>(bool isReference = false)
    {
        return new LocalVariableElement<TVariable>(this, isReference);
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