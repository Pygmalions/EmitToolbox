namespace EmitToolbox.Framework.Symbols.Literals;

public class LiteralNull<TType>(MethodBuildingContext context) 
    : ValueSymbol<TType?>(context) where TType : class
{
    protected internal override void EmitDirectlyLoadValue()
    {
        Context.Code.Emit(OpCodes.Ldnull);
    }

    protected internal override void EmitDirectlyLoadAddress()
    {
        var variable = Context.Code.DeclareLocal(typeof(TType));
        Context.Code.Emit(OpCodes.Ldnull);
        Context.Code.Emit(OpCodes.Stloc, variable);
        Context.Code.Emit(OpCodes.Ldloca, variable);
    }

    protected internal override void EmitLoadAsValue()
        => EmitDirectlyLoadValue();

    protected internal override void EmitLoadAsAddress()
        => EmitDirectlyLoadAddress();
}