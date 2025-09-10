namespace EmitToolbox.Framework.Symbols.Literals;

public class LiteralNull(DynamicMethod context, Type type) : ISymbol
{
    public DynamicMethod Context { get; } = context;

    public Type ContentType { get; } = type;

    public void EmitLoadContent() => Context.Code.Emit(OpCodes.Ldnull);
}

public class LiteralNull<TValue>(DynamicMethod context) :
    LiteralNull(context, typeof(TValue)), ISymbol<TValue?>
    where TValue : notnull
{
}