namespace EmitToolbox.Framework.Elements.LiteralValues;

public class LiteralFloat(MethodContext context, float value) : LiteralValueElement<float>(context, value)
{
    protected internal override void EmitLoadAsValue()
    {
        Context.Code.Emit(OpCodes.Ldc_R4, Value);
    }
}

public class LiteralDouble(MethodContext context, double value) : LiteralValueElement<double>(context, value)
{
    protected internal override void EmitLoadAsValue()
    {
        Context.Code.Emit(OpCodes.Ldc_R8, Value);
    }
}
