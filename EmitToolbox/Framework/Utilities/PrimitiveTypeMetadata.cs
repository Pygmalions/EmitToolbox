namespace EmitToolbox.Framework.Utilities;

public static class PrimitiveTypeMetadata
{
    public static bool GetIsUnsigned(Type type)
    {
        if (type == typeof(byte))
            return true;
        if (type == typeof(ushort))
            return true;
        if (type == typeof(uint))
            return true;
        if (type == typeof(ulong))
            return true;
        return false;
    }

    public static OpCode GetInstructionForIndirectLoading(Type type)
    {
        if (type == typeof(sbyte))
            return OpCodes.Ldind_I1;
        if (type == typeof(short))
            return OpCodes.Ldind_I2;
        if (type == typeof(int))
            return OpCodes.Ldind_I4;
        if (type == typeof(long) || type == typeof(ulong))
            return OpCodes.Ldind_I8;
        if (type == typeof(byte))
            return OpCodes.Ldind_U1;
        if (type == typeof(ushort) || type == typeof(char))
            return OpCodes.Ldind_U2;
        if (type == typeof(uint))
            return OpCodes.Ldind_U4;
        if (type == typeof(nint) || type == typeof(nuint))
            return OpCodes.Ldind_I;
        if (type == typeof(float))
            return OpCodes.Ldind_R4;
        if (type == typeof(double))
            return OpCodes.Ldind_R8;
        throw new Exception($"Unsupported type '{type.Name}'.");
    }
    
    public static OpCode GetInstructionForIndirectStoring(Type type)
    {
        if (type == typeof(sbyte)  || type == typeof(byte) || type == typeof(bool))
            return OpCodes.Stind_I1;
        if (type == typeof(short) || type == typeof(ushort) || type == typeof(char))
            return OpCodes.Stind_I2;
        if (type == typeof(int) || type == typeof(uint))
            return OpCodes.Stind_I4;
        if (type == typeof(long) || type == typeof(ulong))
            return OpCodes.Stind_I8;
        if (type == typeof(nint) || type == typeof(nuint))
            return OpCodes.Stind_I;
        if (type == typeof(float))
            return OpCodes.Stind_R4;
        if (type == typeof(double))
            return OpCodes.Stind_R8;
        throw new Exception($"Unsupported type '{type.Name}'.");
    }
}

public static class PrimitiveTypeMetadata<TPrimitive> where TPrimitive : allows ref struct
{
    public static readonly Lazy<bool> IsUnsigned = 
        new(PrimitiveTypeMetadata.GetIsUnsigned(typeof(TPrimitive)));

    public static readonly Lazy<OpCode> InstructionForIndirectLoading = new(
        () => PrimitiveTypeMetadata.GetInstructionForIndirectLoading(typeof(TPrimitive)));
    
    public static readonly Lazy<OpCode> InstructionForIndirectStoring = new(
        () => PrimitiveTypeMetadata.GetInstructionForIndirectStoring(typeof(TPrimitive)));
}