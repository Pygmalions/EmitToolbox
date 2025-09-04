namespace EmitToolbox.Framework.Symbols.Literals;

public class LiteralNull<TType>(MethodBuildingContext context) 
    : ValueSymbol<TType?>(context) where TType : class
{
    public override void EmitDirectlyLoadValue()
    {
        Context.Code.Emit(OpCodes.Ldnull);
    }

    public override void EmitDirectlyLoadAddress()
    {
        var variable = Context.Code.DeclareLocal(typeof(TType));
        Context.Code.Emit(OpCodes.Ldnull);
        Context.Code.Emit(OpCodes.Stloc, variable);
        Context.Code.Emit(OpCodes.Ldloca, variable);
    }

    public override void EmitLoadAsValue()
        => EmitDirectlyLoadValue();

    public override void EmitLoadAsAddress()
        => EmitDirectlyLoadAddress();
}