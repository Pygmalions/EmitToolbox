namespace EmitToolbox.Framework.Elements;

public abstract class VariableElement<TValue>(MethodContext context) : ValueElement<TValue>(context)
{
    protected internal abstract void EmitStoreValue();
    
    protected internal virtual void EmitInitializeValue()
    {
        if (ValueType.IsValueType)
        {
            EmitLoadAsAddress();
            Context.Code.Emit(OpCodes.Initobj, ValueType);
        }
        else
        {
            Context.Code.Emit(OpCodes.Ldnull);
            EmitStoreValue();
        }
    }

    public void Initialize()
    {
        EmitInitializeValue();
    }
    
    public void Assign<TTarget>(ValueElement<TTarget> value) where TTarget : TValue
    {
        value.EmitLoadAsValue();
        EmitStoreValue();
    }
}