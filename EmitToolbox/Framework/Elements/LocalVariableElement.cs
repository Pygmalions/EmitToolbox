namespace EmitToolbox.Framework.Elements;

public class LocalVariableElement<TValue>(MethodContext context) : VariableElement<TValue>(context)
{
    private readonly LocalBuilder _variable = context.Code.DeclareLocal(typeof(TValue));

    protected internal override void EmitLoadAsValue()
    {
        Context.Code.Emit(OpCodes.Ldloc, _variable);
    }

    protected internal override void EmitLoadAsAddress()
    {
        Context.Code.Emit(OpCodes.Ldloca, _variable);
    }

    protected internal override void EmitStoreValue()
    {
        Context.Code.Emit(OpCodes.Stloc, _variable);
    }
}
