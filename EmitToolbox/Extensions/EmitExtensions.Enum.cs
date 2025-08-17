namespace EmitToolbox.Extensions;

public static class EmitEnumExtensions
{
    public static void LoadEnum(this ILGenerator code, Type enumType, object enumValue)
    {
        if (!enumType.IsEnum)
            throw new ArgumentException($"Type '{enumType.Name}' is not an enum type.", nameof(enumType));
        var underlyingType = enumType.GetEnumUnderlyingType();
        if (underlyingType == typeof(byte) || underlyingType == typeof(sbyte) ||
            underlyingType == typeof(short) || underlyingType == typeof(ushort) ||
            underlyingType == typeof(int) || underlyingType == typeof(uint))
            code.Emit(OpCodes.Ldc_I4, (int)enumValue);
        else if (underlyingType == typeof(long) || underlyingType == typeof(ulong))
            code.Emit(OpCodes.Ldc_I8, (int)enumValue);
        else
            throw new ArgumentException(
                $"Underlying type '{underlyingType.Name}' for enum type '{enumType.Name}' is not supported.",
                nameof(enumType));
    }

    public static void ParseEnum(this ILGenerator code, Type enumType)
    {
        if (!enumType.IsEnum)
            throw new ArgumentException($"Type '{enumType}' is not an enum.", nameof(enumType));
        var underlyingType = enumType.GetEnumUnderlyingType();
        if (underlyingType == typeof(byte) || underlyingType == typeof(sbyte) ||
            underlyingType == typeof(short) || underlyingType == typeof(ushort) ||
            underlyingType == typeof(int) || underlyingType == typeof(uint))
            code.Emit(OpCodes.Conv_I4);
        else if (underlyingType == typeof(long) || underlyingType == typeof(ulong))
            code.Emit(OpCodes.Conv_I8);
        else
            throw new ArgumentException(
                $"Underlying type '{underlyingType.Name}' for enum type '{enumType.Name}' is not supported.",
                nameof(enumType));
    }
}