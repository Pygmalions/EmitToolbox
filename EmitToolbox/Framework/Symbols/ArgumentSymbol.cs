namespace EmitToolbox.Framework.Symbols;

public class ArgumentSymbol(DynamicMethod context, int index, Type type) : IAddressableSymbol, IAssignableSymbol
{
    public DynamicMethod Context { get; } = context;

    public Type ContentType { get; } = type;

    public void EmitLoadContent()
    {
        Context.Code.Emit(OpCodes.Ldarg, index);
    }

    public void EmitLoadAddress()
    {
        Context.Code.Emit(OpCodes.Ldarga, index);
    }

    public void EmitStoreContent()
    {
        Context.Code.Emit(OpCodes.Starg, index);
    }
}

public class ArgumentSymbol<TValue>(
    DynamicMethod context, int index,
    ValueModifier modifier = ValueModifier.None)
    : ArgumentSymbol(context, index, typeof(TValue).WithModifier(modifier)), 
        IAddressableSymbol<TValue>, IAssignableSymbol<TValue>
{
}