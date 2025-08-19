namespace EmitToolbox.Framework.Symbols.Literals;

public class LiteralBoolean(MethodBuildingContext context, bool value) : LiteralValueSymbol<bool>(context, value)
{
    protected internal override void EmitDirectlyLoadValue()
    {
        Context.Code.Emit(Value ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
    }
}