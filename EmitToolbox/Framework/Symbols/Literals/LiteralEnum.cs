using EmitToolbox.Framework.Symbols.Traits;

namespace EmitToolbox.Framework.Symbols.Literals;

public class LiteralEnum<TEnum> : LiteralSymbol<TEnum>, INumberSymbol
    where TEnum : struct, Enum
{
    public INumberSymbol.RepresentationKind Representation { get; }

    public LiteralEnum(DynamicMethod context, TEnum value) : base(context, value)
    {
        var underlyingType = ValueType.GetEnumUnderlyingType();
        Representation = underlyingType switch
        {
            not null when underlyingType == typeof(byte) || underlyingType == typeof(sbyte) ||
                     underlyingType == typeof(short) || underlyingType == typeof(ushort) ||
                     underlyingType == typeof(int) || underlyingType == typeof(uint) =>
                INumberSymbol.RepresentationKind.Integer32,
            not null when underlyingType == typeof(long) || underlyingType == typeof(ulong) =>
                INumberSymbol.RepresentationKind.Integer64,
            _ => throw new Exception(
                $"Unsupported underlying type '{underlyingType?.Name ?? "<Unknown>"}' " +
                $"for enum type '{ValueType.Name}'.")
        };
    }

    public override void EmitLoadContent()
    {
        switch (Representation)
        {
            case INumberSymbol.RepresentationKind.Integer32:
                Context.Code.Emit(OpCodes.Ldc_I4, Convert.ToInt32(Value));
                break;
            case INumberSymbol.RepresentationKind.Integer64:
                Context.Code.Emit(OpCodes.Ldc_I8, Convert.ToInt64(Value));
                break;
            case INumberSymbol.RepresentationKind.Native:
            case INumberSymbol.RepresentationKind.FloatingPoint32:
            case INumberSymbol.RepresentationKind.FloatingPoint64:
            default:
                throw new Exception($"Unsupported representation '{Representation}' for enum type '{ValueType.Name}'.");
        }
    }
}