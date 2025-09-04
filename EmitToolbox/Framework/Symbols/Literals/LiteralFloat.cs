namespace EmitToolbox.Framework.Symbols.Literals;

public class LiteralFloat(MethodBuildingContext context, float value) : LiteralValueSymbol<float>(context, value)
{
    public override void EmitDirectlyLoadValue()
    {
        Context.Code.Emit(OpCodes.Ldc_R4, Value);
    }
}

public class LiteralDouble(MethodBuildingContext context, double value) : LiteralValueSymbol<double>(context, value)
{
    public override void EmitDirectlyLoadValue()
    {
        Context.Code.Emit(OpCodes.Ldc_R8, Value);
    }
}
