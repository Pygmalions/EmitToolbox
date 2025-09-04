namespace EmitToolbox.Framework.Symbols;

public class ExpressionSymbol<TValue>(MethodBuildingContext context) : 
    ValueSymbol<TValue>(context)
{
    public required Func<ValueSymbol<TValue>> Expression { get; init; }
    
    public override void EmitDirectlyLoadValue()
        => Expression().EmitDirectlyLoadValue();

    public override void EmitDirectlyLoadAddress()
        => Expression().EmitDirectlyLoadAddress();

    public override void EmitLoadAsValue()
        => Expression().EmitLoadAsValue();

    public override void EmitLoadAsAddress()
        => Expression().EmitLoadAsAddress();
}