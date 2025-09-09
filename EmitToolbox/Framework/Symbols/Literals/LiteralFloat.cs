using EmitToolbox.Framework.Symbols.Traits;

namespace EmitToolbox.Framework.Symbols.Literals;

public class LiteralFloat(DynamicMethod context, float value) : LiteralSymbol<float>(context, value), INumberSymbol
{
    public INumberSymbol.RepresentationKind Representation => INumberSymbol.RepresentationKind.FloatingPoint32;

    public override void EmitLoadContent()
        => Context.Code.Emit(OpCodes.Ldc_R4, Value);
}

public class LiteralDouble(DynamicMethod context, double value) : LiteralSymbol<double>(context, value), INumberSymbol
{
    public INumberSymbol.RepresentationKind Representation => INumberSymbol.RepresentationKind.FloatingPoint32;
    
    public override void EmitLoadContent()
        => Context.Code.Emit(OpCodes.Ldc_R8, Value);
}
