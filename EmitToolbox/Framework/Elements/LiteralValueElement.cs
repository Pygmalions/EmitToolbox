namespace EmitToolbox.Framework.Elements;

public abstract class LiteralValueElement<TValue>(MethodContext context, TValue value) : ValueElement<TValue>(context)
{
    public sealed override Type ValueType { get; } = typeof(TValue);

    public TValue Value { get; } = value;
    
    protected internal override void EmitLoadAsAddress()
    {
        var variable = Context.Code.DeclareLocal(typeof(TValue));
        EmitLoadAsValue();
        Context.Code.Emit(OpCodes.Stloc, variable);
        Context.Code.Emit(OpCodes.Ldloca, variable);
    }
}
