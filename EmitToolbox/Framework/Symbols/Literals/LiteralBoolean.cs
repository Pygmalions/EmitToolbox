namespace EmitToolbox.Framework.Symbols.Literals;

public class LiteralBoolean(DynamicMethod context, bool value) : LiteralSymbol<bool>(context, value)
{
    public override void EmitLoadContent()
        => Context.Code.Emit(Value ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
}