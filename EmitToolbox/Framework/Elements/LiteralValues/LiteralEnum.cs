namespace EmitToolbox.Framework.Elements.LiteralValues;

public class LiteralEnum<TEnum>(MethodContext context, TEnum value) : LiteralValueElement<TEnum>(context, value)
    where TEnum : struct, Enum
{
    protected internal override void EmitLoadAsValue()
    {
        var underlyingType = ValueType.GetEnumUnderlyingType();
        if (underlyingType == typeof(byte) || underlyingType == typeof(sbyte) ||
            underlyingType == typeof(short) || underlyingType == typeof(ushort) ||
            underlyingType == typeof(int) || underlyingType == typeof(uint))
            Context.Code.Emit(OpCodes.Ldc_I4, Convert.ToInt32(Value));
        else if (underlyingType == typeof(long) || underlyingType == typeof(ulong))
            Context.Code.Emit(OpCodes.Ldc_I8, Convert.ToInt64(Value));
        else
            throw new Exception(
                $"Underlying type '{underlyingType.Name}' for enum type '{ValueType.Name}' is not supported.");
    }
}