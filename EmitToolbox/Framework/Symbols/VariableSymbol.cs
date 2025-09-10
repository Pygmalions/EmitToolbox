namespace EmitToolbox.Framework.Symbols;

public class VariableSymbol(DynamicMethod context, LocalBuilder variable) : IAddressableSymbol, IAssignableSymbol
{
    public DynamicMethod Context { get; } = context;

    private LocalBuilder Variable { get; } = variable;

    public Type ContentType { get; } = variable.LocalType;

    public void EmitLoadContent()
    {
        Context.Code.Emit(OpCodes.Ldloc, Variable);
    }

    public void EmitLoadAddress()
    {
        Context.Code.Emit(OpCodes.Ldloca, Variable);
    }

    public void EmitStoreContent()
    {
        Context.Code.Emit(OpCodes.Stloc, Variable);
    }
}

public class VariableSymbol<TValue>(
    DynamicMethod context,
    ValueModifier modifier = ValueModifier.None,
    bool isPinned = false)
    : VariableSymbol(context, context.Code.DeclareLocal(typeof(TValue).WithModifier(modifier), isPinned)), 
        IAddressableSymbol<TValue>, IAssignableSymbol<TValue>
{
}