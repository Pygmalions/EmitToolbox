using System.Diagnostics.CodeAnalysis;

namespace EmitToolbox.Framework.Symbols;

public class LocalVariableSymbol<TValue>(MethodBuildingContext context, bool isReference = false) 
    : VariableSymbol<TValue>(context, isReference)
{
    [field: MaybeNull]
    private LocalBuilder Variable
    {
        get
        {
            if (field != null)
                return field;
            var type = typeof(TValue);
            if (IsReference)
                type = type.MakeByRefType();
            return field = Context.Code.DeclareLocal(type);
        }
    } = null!;

    protected override void EmitDirectlyStoreValue()
    {
        Context.Code.Emit(OpCodes.Stloc, Variable);
    }

    protected internal override void EmitDirectlyLoadValue()
    {
        Context.Code.Emit(OpCodes.Ldloc, Variable);
    }

    protected internal override void EmitDirectlyLoadAddress()
    {
        Context.Code.Emit(OpCodes.Ldloca, Variable);
    }
}