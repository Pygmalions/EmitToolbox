namespace EmitToolbox.Framework.Symbols.Literals;

public class LiteralString(MethodBuildingContext context, string value) 
    : LiteralValueSymbol<string>(context, value)
{
    protected internal override void EmitDirectlyLoadValue()
    {
        Context.Code.Emit(OpCodes.Ldstr, Value);
    }
}