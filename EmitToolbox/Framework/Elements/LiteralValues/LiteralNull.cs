namespace EmitToolbox.Framework.Elements.LiteralValues;

public class LiteralNull<TType>(MethodContext context) : ValueElement<TType?>(context) where TType : class
{
    protected internal override void EmitLoadAsValue()
    {
        Context.Code.Emit(OpCodes.Ldnull);
    }

    protected internal override void EmitLoadAsAddress()
    {
        var variable = Context.Code.DeclareLocal(typeof(TType));
        Context.Code.Emit(OpCodes.Ldnull);
        Context.Code.Emit(OpCodes.Stloc, variable);
        Context.Code.Emit(OpCodes.Ldloca, variable);
    }
}