namespace EmitToolbox.Framework.Symbols.Literals;

public readonly struct LiteralEnumSymbol<TEnum>(DynamicFunction context, TEnum value) : ILiteralSymbol<TEnum>
    where TEnum : struct, Enum
{
    public DynamicFunction Context => context;
    
    public TEnum Value => value;

    private static Type UnderlyingType => typeof(TEnum).GetEnumUnderlyingType()!;

    public void LoadContent()
    {
        if (UnderlyingType == typeof(byte) || UnderlyingType == typeof(sbyte) ||
            UnderlyingType == typeof(short) || UnderlyingType == typeof(ushort) ||
            UnderlyingType == typeof(int) || UnderlyingType == typeof(uint))
        {
            Context.Code.Emit(OpCodes.Ldc_I4, Convert.ToInt32(Value));
            return;
        }
        if (UnderlyingType == typeof(long) || UnderlyingType == typeof(ulong))
        {
            Context.Code.Emit(OpCodes.Ldc_I8, Convert.ToInt64(Value));
            return;
        }
        throw new Exception($"Unsupported underlying type '{UnderlyingType?.Name ?? "<Unknown>"}' for enum type '{typeof(TEnum).Name}'.");
    }
}