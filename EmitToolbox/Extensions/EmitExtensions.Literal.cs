namespace EmitToolbox.Extensions;

public static class EmitLiteralExtensions
{
    extension(ILGenerator code)
    {
        public void LoadNull()
            => code.Emit(OpCodes.Ldnull);

        public void LoadLiteral(object? boxedLiteral)
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

        public void LoadBoxedLiteral(object? boxedLiteral)
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

        public void LoadLiteral(sbyte value)
            => code.Emit(OpCodes.Ldc_I4, value);

        public void LoadLiteral(byte value)
            => code.Emit(OpCodes.Ldc_I4, value);

        public void LoadLiteral(short value)
            => code.Emit(OpCodes.Ldc_I4, value);

        public void LoadLiteral(ushort value)
            => code.Emit(OpCodes.Ldc_I4, value);

        public void LoadLiteral(int value)
            => code.Emit(OpCodes.Ldc_I4, value);

        public void LoadLiteral(uint value)
            => code.Emit(OpCodes.Ldc_I4, value);

        public void LoadLiteral(long value)
            => code.Emit(OpCodes.Ldc_I8, value);

        public void LoadLiteral(ulong value)
            => code.Emit(OpCodes.Ldc_I8, value);

        public void LoadLiteral(float value)
            => code.Emit(OpCodes.Ldc_R4, value);

        public void LoadLiteral(double value)
            => code.Emit(OpCodes.Ldc_R8, value);

        public void LoadLiteral(decimal value)
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

        public void LoadLiteral(char value)
            => code.Emit(OpCodes.Ldc_I4, value);

        public void LoadLiteral(string value)
            => code.Emit(OpCodes.Ldstr, value);

        public void LoadLiteral(bool value)
            => code.Emit(value ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);

        /// <summary>
        /// This method has the same function to `default(T)`.
        /// </summary>
        public void LoadDefault(Type type)
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

        public void LoadParameterDefaultValue(ParameterInfo parameter)
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
}