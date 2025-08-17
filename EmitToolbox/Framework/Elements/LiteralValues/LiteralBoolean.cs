namespace EmitToolbox.Framework.Elements.LiteralValues;

public class LiteralBoolean(MethodContext context, bool value) : LiteralValueElement<bool>(context, value)
{
    protected internal override void EmitLoadAsValue()
    {
        Context.Code.Emit(Value ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
    }
}