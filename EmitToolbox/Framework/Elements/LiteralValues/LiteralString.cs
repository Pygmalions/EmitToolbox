namespace EmitToolbox.Framework.Elements.LiteralValues;

public class LiteralString(MethodContext context, string value) : LiteralValueElement<string>(context, value)
{
    protected internal override void EmitLoadAsValue()
    {
        Context.Code.Emit(OpCodes.Ldstr, Value);
    }
}