using System.Diagnostics.CodeAnalysis;

namespace EmitToolbox.Framework.Elements;

public class LocalVariableElement<TValue>(MethodContext context, bool isReference = false) : 
    VariableElement<TValue>(context)
{
    private readonly LocalBuilder _variable =
        context.Code.DeclareLocal(isReference ? typeof(TValue).MakeByRefType() : typeof(TValue));

    public override bool IsReference { get; } = isReference;

    protected Action<ILGenerator>? ReferenceLoader { get; } =
        isReference ? ValueElementReferenceLoader.SelectReferenceLoader(typeof(TValue)) : null;

    protected Action<ILGenerator>? ReferenceStorer { get; } =
        isReference ? ValueElementReferenceLoader.SelectReferenceStorer(typeof(TValue)) : null;
    
    [field: MaybeNull] protected VariableElement<TValue> TemporaryVariable => 
        field ??= Context.DefineVariable<TValue>();
    
    protected internal override void EmitLoadAsValue()
    {
        Context.Code.Emit(OpCodes.Ldloc, _variable);
        ReferenceLoader?.Invoke(Context.Code);
    }

    protected internal override void EmitLoadAsAddress()
    {
        Context.Code.Emit(IsReference ? OpCodes.Ldloc : OpCodes.Ldloca, _variable);
    }

    protected internal override void EmitStoreValue()
    {
        if (ReferenceStorer == null)
        {
            Context.Code.Emit(OpCodes.Stloc, _variable);
            return;
        }
        
        TemporaryVariable.EmitStoreValue();
        Context.Code.Emit(OpCodes.Ldloc, _variable);
        TemporaryVariable.EmitLoadAsValue();
        ReferenceStorer(Context.Code);
    }
}