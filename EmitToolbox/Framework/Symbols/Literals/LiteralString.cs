namespace EmitToolbox.Framework.Symbols.Literals;

public class LiteralString(DynamicMethod context, string value) : LiteralSymbol<string>(context, value)
{
    public override void EmitLoadContent()
        => Context.Code.Emit(OpCodes.Ldstr, Value);
}