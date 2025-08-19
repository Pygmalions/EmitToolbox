namespace EmitToolbox.Framework.Symbols.Literals;

public class LiteralInteger8(MethodBuildingContext context, sbyte value)
    : LiteralValueSymbol<sbyte>(context, value)
{
    protected internal override void EmitDirectlyLoadValue()
    {
        Context.Code.Emit(OpCodes.Ldc_I4, Value);
    }
}

public class LiteralIntegerU8(MethodBuildingContext context, byte value)
    : LiteralValueSymbol<byte>(context, value)
{
    protected internal override void EmitDirectlyLoadValue()
    {
        Context.Code.Emit(OpCodes.Ldc_I4, Value);
    }
}

public class LiteralIntegerCharacter(MethodBuildingContext context, char value)
    : LiteralValueSymbol<char>(context, value)
{
    protected internal override void EmitDirectlyLoadValue()
    {
        Context.Code.Emit(OpCodes.Ldc_I4, Value);
    }
}

public class LiteralInteger16(MethodBuildingContext context, short value)
    : LiteralValueSymbol<short>(context, value)
{
    protected internal override void EmitDirectlyLoadValue()
    {
        Context.Code.Emit(OpCodes.Ldc_I4, Value);
    }
}

public class LiteralIntegerU16(MethodBuildingContext context, ushort value)
    : LiteralValueSymbol<ushort>(context, value)
{
    protected internal override void EmitDirectlyLoadValue()
    {
        Context.Code.Emit(OpCodes.Ldc_I4, Value);
    }
}

public class LiteralInteger32(MethodBuildingContext context, int value) 
    : LiteralValueSymbol<int>(context, value)
{
    protected internal override void EmitDirectlyLoadValue()
    {
        Context.Code.Emit(OpCodes.Ldc_I4, Value);
    }
}

public class LiteralIntegerU32(MethodBuildingContext context, uint value)
    : LiteralValueSymbol<uint>(context, value)
{
    protected internal override void EmitDirectlyLoadValue()
    {
        Context.Code.Emit(OpCodes.Ldc_I4, Value);
    }
}

public class LiteralInteger64(MethodBuildingContext context, long value)
    : LiteralValueSymbol<long>(context, value)
{
    protected internal override void EmitDirectlyLoadValue()
    {
        Context.Code.Emit(OpCodes.Ldc_I8, Value);
    }
}

public class LiteralIntegerU64(MethodBuildingContext context, ulong value)
    : LiteralValueSymbol<ulong>(context, value)
{
    protected internal override void EmitDirectlyLoadValue()
    {
        Context.Code.Emit(OpCodes.Ldc_I8, Value);
    }
}