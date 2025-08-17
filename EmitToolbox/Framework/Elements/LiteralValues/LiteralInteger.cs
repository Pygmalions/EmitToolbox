namespace EmitToolbox.Framework.Elements.LiteralValues;

public class LiteralInteger8(MethodContext context, sbyte value) : LiteralValueElement<sbyte>(context, value)
{
    protected internal override void EmitLoadAsValue()
    {
        Context.Code.Emit(OpCodes.Ldc_I4, Value);
    }
}

public class LiteralIntegerU8(MethodContext context, byte value) : LiteralValueElement<byte>(context, value)
{
    protected internal override void EmitLoadAsValue()
    {
        Context.Code.Emit(OpCodes.Ldc_I4, Value);
    }
}

public class LiteralIntegerCharacter(MethodContext context, char value) : LiteralValueElement<char>(context, value)
{
    protected internal override void EmitLoadAsValue()
    {
        Context.Code.Emit(OpCodes.Ldc_I4, Value);
    }
}

public class LiteralInteger16(MethodContext context, short value) : LiteralValueElement<short>(context, value)
{
    protected internal override void EmitLoadAsValue()
    {
        Context.Code.Emit(OpCodes.Ldc_I4, Value);
    }
}

public class LiteralIntegerU16(MethodContext context, ushort value) : LiteralValueElement<ushort>(context, value)
{
    protected internal override void EmitLoadAsValue()
    {
        Context.Code.Emit(OpCodes.Ldc_I4, Value);
    }
}

public class LiteralInteger32(MethodContext context, int value) : LiteralValueElement<int>(context, value)
{
    protected internal override void EmitLoadAsValue()
    {
        Context.Code.Emit(OpCodes.Ldc_I4, Value);
    }
}

public class LiteralIntegerU32(MethodContext context, uint value) : LiteralValueElement<uint>(context, value)
{
    protected internal override void EmitLoadAsValue()
    {
        Context.Code.Emit(OpCodes.Ldc_I4, Value);
    }
}

public class LiteralInteger64(MethodContext context, long value) : LiteralValueElement<long>(context, value)
{
    protected internal override void EmitLoadAsValue()
    {
        Context.Code.Emit(OpCodes.Ldc_I8, Value);
    }
}

public class LiteralIntegerU64(MethodContext context, ulong value) : LiteralValueElement<ulong>(context, value)
{
    protected internal override void EmitLoadAsValue()
    {
        Context.Code.Emit(OpCodes.Ldc_I8, Value);
    }
}