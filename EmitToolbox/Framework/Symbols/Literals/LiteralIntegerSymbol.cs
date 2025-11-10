namespace EmitToolbox.Framework.Symbols.Literals;

public readonly struct LiteralInteger8Symbol(DynamicMethod context, sbyte value) : ILiteralSymbol<sbyte>
{
    public DynamicMethod Context => context;

    public sbyte Value => value;

    public void LoadContent() => Context.Code.Emit(OpCodes.Ldc_I4, (int)Value);
}

public readonly struct LiteralUnsignedInteger8Symbol(DynamicMethod context, byte value) : ILiteralSymbol<byte>
{
    public DynamicMethod Context => context;

    public byte Value => value;

    public void LoadContent() => Context.Code.Emit(OpCodes.Ldc_I4, (int)Value);
}

public readonly struct LiteralIntegerCharacterSymbol(DynamicMethod context, char value) : ILiteralSymbol<char>
{
    public DynamicMethod Context => context;

    public char Value => value;

    public void LoadContent() => Context.Code.Emit(OpCodes.Ldc_I4, (int)Value);
}

public readonly struct LiteralInteger16Symbol(DynamicMethod context, short value) : ILiteralSymbol<short>
{
    public DynamicMethod Context => context;

    public short Value => value;

    public void LoadContent() => Context.Code.Emit(OpCodes.Ldc_I4, (int)Value);
}

public readonly struct LiteralUnsignedInteger16Symbol(DynamicMethod context, ushort value) : ILiteralSymbol<ushort>
{
    public DynamicMethod Context => context;

    public ushort Value => value;

    public void LoadContent() => Context.Code.Emit(OpCodes.Ldc_I4, (int)Value);
}

public readonly struct LiteralInteger32Symbol(DynamicMethod context, int value) : ILiteralSymbol<int>
{
    public DynamicMethod Context => context;

    public int Value => value;

    public void LoadContent() => Context.Code.Emit(OpCodes.Ldc_I4, Value);
}

public readonly struct LiteralUnsignedInteger32Symbol(DynamicMethod context, uint value) : ILiteralSymbol<uint>
{
    public DynamicMethod Context => context;

    public uint Value => value;

    public void LoadContent() => Context.Code.Emit(OpCodes.Ldc_I4, (int)Value);
}

public readonly struct LiteralInteger64Symbol(DynamicMethod context, long value) : ILiteralSymbol<long>
{
    public DynamicMethod Context => context;

    public long Value => value;

    public void LoadContent() => Context.Code.Emit(OpCodes.Ldc_I8, Value);
}

public readonly struct LiteralUnsignedInteger64Symbol(DynamicMethod context, ulong value) : ILiteralSymbol<ulong>
{
    public DynamicMethod Context => context;

    public ulong Value => value;

    public void LoadContent() => Context.Code.Emit(OpCodes.Ldc_I8, (long)Value);
}