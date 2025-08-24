namespace EmitToolbox.Framework.Symbols;

public class ExpressionSymbol<TValue>(MethodBuildingContext context) : 
    ValueSymbol<TValue>(context)
{
    public required Func<ValueSymbol<TValue>> Expression { get; init; }
    
    protected internal override void EmitDirectlyLoadValue()
        => Expression().EmitDirectlyLoadValue();

    protected internal override void EmitDirectlyLoadAddress()
        => Expression().EmitDirectlyLoadAddress();

    protected internal override void EmitLoadAsValue()
        => Expression().EmitLoadAsValue();

    protected internal override void EmitLoadAsAddress()
        => Expression().EmitLoadAsAddress();
}