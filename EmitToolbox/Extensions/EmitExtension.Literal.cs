namespace EmitToolbox.Extensions;

public static class EmitLiteralExtension
{
    public static void LoadNull(this ILGenerator code)
        => code.Emit(OpCodes.Ldnull);

    public static void LoadLiteral(this ILGenerator code, object? boxedLiteral)
    {
        if (boxedLiteral is null)
        {
            code.LoadNull();
            return;
        }

        if (boxedLiteral.GetType().IsEnum)
        {
            code.LoadEnum(boxedLiteral.GetType(), boxedLiteral);
            return;
        }

        switch (boxedLiteral)
        {
            case sbyte value:
                code.LoadLiteral(value);
                break;
            case byte value:
                code.LoadLiteral(value);
                break;
            case short value:
                code.LoadLiteral(value);
                break;
            case ushort value:
                code.LoadLiteral(value);
                break;
            case int value:
                code.LoadLiteral(value);
                break;
            case uint value:
                code.LoadLiteral(value);
                break;
            case long value:
                code.LoadLiteral(value);
                break;
            case ulong value:
                code.LoadLiteral(value);
                break;
            case float value:
                code.LoadLiteral(value);
                break;
            case double value:
                code.LoadLiteral(value);
                break;
            case decimal value:
                code.LoadLiteral(value);
                break;
            case char value:
                code.LoadLiteral(value);
                break;
            case string value:
                code.LoadLiteral(value);
                break;
            case bool value:
                code.LoadLiteral(value);
                break;
        }
    }

    public static void LoadBoxedLiteral(this ILGenerator code, object? boxedLiteral)
    {
        if (boxedLiteral is null)
        {
            code.LoadNull();
            return;
        }

        if (boxedLiteral.GetType().IsEnum)
        {
            var enumType = boxedLiteral.GetType();
            code.LoadEnum(enumType, boxedLiteral);
            code.Box(enumType);
            return;
        }

        switch (boxedLiteral)
        {
            case sbyte value:
                code.LoadLiteral(value);
                code.Box<sbyte>();
                break;
            case byte value:
                code.LoadLiteral(value);
                code.Box<byte>();
                break;
            case short value:
                code.LoadLiteral(value);
                code.Box<short>();
                break;
            case ushort value:
                code.LoadLiteral(value);
                code.Box<ushort>();
                break;
            case int value:
                code.LoadLiteral(value);
                code.Box<int>();
                break;
            case uint value:
                code.LoadLiteral(value);
                code.Box<uint>();
                break;
            case long value:
                code.LoadLiteral(value);
                code.Box<long>();
                break;
            case ulong value:
                code.LoadLiteral(value);
                code.Box<ulong>();
                break;
            case float value:
                code.LoadLiteral(value);
                code.Box<float>();
                break;
            case double value:
                code.LoadLiteral(value);
                code.Box<double>();
                break;
            case decimal value:
                code.LoadLiteral(value);
                code.Box<decimal>();
                break;
            case char value:
                code.LoadLiteral(value);
                code.Box<char>();
                break;
            case string value:
                code.LoadLiteral(value);
                break;
            case bool value:
                code.LoadLiteral(value);
                code.Box<bool>();
                break;
        }
    }

    public static void LoadLiteral(this ILGenerator code, sbyte value)
        => code.Emit(OpCodes.Ldc_I4, value);

    public static void LoadLiteral(this ILGenerator code, byte value)
        => code.Emit(OpCodes.Ldc_I4, value);

    public static void LoadLiteral(this ILGenerator code, short value)
        => code.Emit(OpCodes.Ldc_I4, value);

    public static void LoadLiteral(this ILGenerator code, ushort value)
        => code.Emit(OpCodes.Ldc_I4, value);

    public static void LoadLiteral(this ILGenerator code, int value)
        => code.Emit(OpCodes.Ldc_I4, value);

    public static void LoadLiteral(this ILGenerator code, uint value)
        => code.Emit(OpCodes.Ldc_I4, value);

    public static void LoadLiteral(this ILGenerator code, long value)
        => code.Emit(OpCodes.Ldc_I8, value);

    public static void LoadLiteral(this ILGenerator code, ulong value)
        => code.Emit(OpCodes.Ldc_I8, value);

    public static void LoadLiteral(this ILGenerator code, float value)
        => code.Emit(OpCodes.Ldc_R4, value);

    public static void LoadLiteral(this ILGenerator code, double value)
        => code.Emit(OpCodes.Ldc_R8, value);

    public static void LoadLiteral(this ILGenerator code, decimal value)
    {
        // Allocate a Span on stack and store it in a local variable.
        var variableBitsSpan = code.DeclareLocal(typeof(Span<int>));
        code.AllocateSpanOnStack<int>(4);
        code.Emit(OpCodes.Stloc, variableBitsSpan);
        // decimal.GetBits(...) only takes 4 integers.
        Span<int> bits = stackalloc int[4];
        decimal.GetBits(value, bits);

        // Store the bits into the Span.
        for (var bitIndex = 0; bitIndex < 4; ++bitIndex)
        {
            code.LoadLocalAddress(variableBitsSpan);
            code.GetSpanItemReference<int>(bitIndex);
            code.Emit(OpCodes.Ldc_I4, bits[bitIndex]);
            code.Emit(OpCodes.Stind_I4);
        }

        // Convert it into a read-only Span.
        var variableDecimal = code.DeclareLocal(typeof(decimal));
        code.LoadLocalAddress(variableDecimal);
        code.LoadLocal(variableBitsSpan);
        code.ConvertSpanToReadOnlySpan<int>();
        code.Call(typeof(decimal).GetConstructor([typeof(ReadOnlySpan<int>)])!);
        code.LoadLocal(variableDecimal);
    }

    public static void LoadLiteral(this ILGenerator code, char value)
        => code.Emit(OpCodes.Ldc_I4, value);

    public static void LoadLiteral(this ILGenerator code, string value)
        => code.Emit(OpCodes.Ldstr, value);

    public static void LoadLiteral(this ILGenerator code, bool value)
        => code.Emit(value ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);

    /// <summary>
    /// This method has the same function to `default(T)`.
    /// </summary>
    public static void LoadDefault(this ILGenerator code, Type type)
    {
        if (type.IsValueType)
        {
            var localVariable = code.DeclareLocal(type);
            code.Emit(OpCodes.Ldloca, localVariable);
            code.Emit(OpCodes.Initobj, type);
        }
        else
            code.Emit(OpCodes.Ldnull);
    }

    public static void LoadParameterDefaultValue(this ILGenerator code, ParameterInfo parameter)
    {
        var parameterType = parameter.ParameterType;
        switch (parameter.DefaultValue)
        {
            case sbyte value:
                code.LoadLiteral(value);
                break;
            case byte value:
                code.LoadLiteral(value);
                break;
            case short value:
                code.LoadLiteral(value);
                break;
            case ushort value:
                code.LoadLiteral(value);
                break;
            case int value:
                code.LoadLiteral(value);
                break;
            case uint value:
                code.LoadLiteral(value);
                break;
            case long value:
                code.LoadLiteral(value);
                break;
            case ulong value:
                code.LoadLiteral(value);
                break;
            case float value:
                code.LoadLiteral(value);
                break;
            case double value:
                code.LoadLiteral(value);
                break;
            case decimal value:
                code.LoadLiteral(value);
                break;
            case char value:
                code.LoadLiteral(value);
                break;
            case string value:
                code.LoadLiteral(value);
                break;
            case bool value:
                code.LoadLiteral(value);
                break;
            case null:
                code.LoadDefault(parameterType);
                break;
        }
    }
}