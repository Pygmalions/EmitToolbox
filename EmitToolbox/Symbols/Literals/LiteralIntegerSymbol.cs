namespace EmitToolbox.Symbols.Literals;

public readonly struct LiteralInteger8Symbol(DynamicFunction context, sbyte value) : ILiteralSymbol<sbyte>
{
    public DynamicFunction Context => context;

    public sbyte Value => value;

    public void LoadContent() => Context.Code.Emit(OpCodes.Ldc_I4, (int)Value);
}

public readonly struct LiteralUnsignedInteger8Symbol(DynamicFunction context, byte value) : ILiteralSymbol<byte>
{
    public DynamicFunction Context => context;

    public byte Value => value;

    public void LoadContent() => Context.Code.Emit(OpCodes.Ldc_I4, (int)Value);
}

public readonly struct LiteralIntegerCharacterSymbol(DynamicFunction context, char value) : ILiteralSymbol<char>
{
    public DynamicFunction Context => context;

    public char Value => value;

    public void LoadContent() => Context.Code.Emit(OpCodes.Ldc_I4, (int)Value);
}

public readonly struct LiteralInteger16Symbol(DynamicFunction context, short value) : ILiteralSymbol<short>
{
    public DynamicFunction Context => context;

    public short Value => value;

    public void LoadContent() => Context.Code.Emit(OpCodes.Ldc_I4, (int)Value);
}

public readonly struct LiteralUnsignedInteger16Symbol(DynamicFunction context, ushort value) : ILiteralSymbol<ushort>
{
    public DynamicFunction Context => context;

    public ushort Value => value;

    public void LoadContent() => Context.Code.Emit(OpCodes.Ldc_I4, (int)Value);
}

public readonly struct LiteralInteger32Symbol(DynamicFunction context, int value) : ILiteralSymbol<int>
{
    public DynamicFunction Context => context;

    public int Value => value;

    public void LoadContent() => Context.Code.Emit(OpCodes.Ldc_I4, Value);
}

public readonly struct LiteralUnsignedInteger32Symbol(DynamicFunction context, uint value) : ILiteralSymbol<uint>
{
    public DynamicFunction Context => context;

    public uint Value => value;

    public void LoadContent() => Context.Code.Emit(OpCodes.Ldc_I4, (int)Value);
}

public readonly struct LiteralInteger64Symbol(DynamicFunction context, long value) : ILiteralSymbol<long>
{
    public DynamicFunction Context => context;

    public long Value => value;

    public void LoadContent() => Context.Code.Emit(OpCodes.Ldc_I8, Value);
}

public readonly struct LiteralUnsignedInteger64Symbol(DynamicFunction context, ulong value) : ILiteralSymbol<ulong>
{
    public DynamicFunction Context => context;

    public ulong Value => value;

    public void LoadContent() => Context.Code.Emit(OpCodes.Ldc_I8, (long)Value);
}