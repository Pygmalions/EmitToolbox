using System.Diagnostics.CodeAnalysis;

namespace EmitToolbox.Framework.Elements;

public class ArgumentElement<TValue>(MethodContext context, int index, bool isReference = false) :
    VariableElement<TValue>(context)
{
    public int Index { get; } = index;

    public override bool IsReference { get; } = isReference;

    protected Action<ILGenerator>? ReferenceLoader { get; } =
        isReference ? ValueElementReferenceLoader.SelectReferenceLoader(typeof(TValue)) : null;

    protected Action<ILGenerator>? ReferenceStorer { get; } =
        isReference ? ValueElementReferenceLoader.SelectReferenceStorer(typeof(TValue)) : null;
    
    [field: MaybeNull] protected VariableElement<TValue> TemporaryVariable => 
        field ??= Context.DefineVariable<TValue>();
    
    protected internal override void EmitLoadAsValue()
    {
        Context.Code.Emit(OpCodes.Ldarg, Index);
        ReferenceLoader?.Invoke(Context.Code);
    }

    protected internal override void EmitLoadAsAddress()
    {
        Context.Code.Emit(IsReference ? OpCodes.Ldarg : OpCodes.Ldarga, Index);
    }

    protected internal override void EmitStoreValue()
    {
        if (ReferenceStorer == null)
        {
            Context.Code.Emit(OpCodes.Starg, Index);
            return;
        }
        
        TemporaryVariable.EmitStoreValue();
        Context.Code.Emit(OpCodes.Ldarg, Index);
        TemporaryVariable.EmitLoadAsValue();
        ReferenceStorer(Context.Code);
    }

    protected internal override void EmitInitializeValue()
    {
        if (ValueType.IsValueType)
        {
            EmitLoadAsAddress();
            Context.Code.Emit(OpCodes.Initobj, ValueType);
        }
        else
        {
            Context.Code.Emit(OpCodes.Ldnull);
            Context.Code.Emit(OpCodes.Starg, Index);
        }
    }
}