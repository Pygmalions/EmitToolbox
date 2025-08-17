namespace EmitToolbox.Framework.Elements;

public class ArgumentElement<TValue>(MethodContext context, int index) : VariableElement<TValue>(context)
{
    public int Index { get; } = index;
    
    protected internal override void EmitLoadAsValue()
    {
        Context.Code.Emit(OpCodes.Ldarg, Index);
    }

    protected internal override void EmitLoadAsAddress()
    {
        Context.Code.Emit(OpCodes.Ldarga, Index);
    }

    protected internal override void EmitStoreValue()
    {
        Context.Code.Emit(OpCodes.Starg, Index);
    }
    
    protected internal override void EmitInitializeValue()
    {
        if (ValueType.IsValueType)
        {
            EmitLoadAsAddress();
            Context.Code.Emit(OpCodes.Initobj, ValueType);
        }
        else
        {
            Context.Code.Emit(OpCodes.Ldnull);
            Context.Code.Emit(OpCodes.Starg, Index);
        }
    }
}