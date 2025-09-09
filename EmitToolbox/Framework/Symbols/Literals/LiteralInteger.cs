using EmitToolbox.Framework.Symbols.Traits;

namespace EmitToolbox.Framework.Symbols.Literals;

public class LiteralInteger8(DynamicMethod context, sbyte value)
    : LiteralSymbol<sbyte>(context, value), INumberSymbol
{
    public INumberSymbol.RepresentationKind Representation 
        => INumberSymbol.RepresentationKind.Integer32;

    public override void EmitLoadContent()
        => Context.Code.Emit(OpCodes.Ldc_I4, (int)Value);
}

public class LiteralUnsignedInteger8(DynamicMethod context, byte value)
    : LiteralSymbol<byte>(context, value), INumberSymbol
{
    public INumberSymbol.RepresentationKind Representation 
        => INumberSymbol.RepresentationKind.Integer32;
    
    public override void EmitLoadContent()
        => Context.Code.Emit(OpCodes.Ldc_I4, (int)Value);
}

public class LiteralIntegerCharacter(DynamicMethod context, char value)
    : LiteralSymbol<char>(context, value), INumberSymbol
{
    public INumberSymbol.RepresentationKind Representation 
        => INumberSymbol.RepresentationKind.Integer32;
    
    public override void EmitLoadContent()
        => Context.Code.Emit(OpCodes.Ldc_I4, Value);
}

public class LiteralInteger16(DynamicMethod context, short value)
    : LiteralSymbol<short>(context, value), INumberSymbol
{
    public INumberSymbol.RepresentationKind Representation 
        => INumberSymbol.RepresentationKind.Integer32;
    
    public override void EmitLoadContent()
        => Context.Code.Emit(OpCodes.Ldc_I4, (int)Value);
}

public class LiteralUnsignedInteger16(DynamicMethod context, ushort value)
    : LiteralSymbol<ushort>(context, value), INumberSymbol
{
    public INumberSymbol.RepresentationKind Representation 
        => INumberSymbol.RepresentationKind.Integer32;
    
    public override void EmitLoadContent()
        => Context.Code.Emit(OpCodes.Ldc_I4, Value);
}

public class LiteralInteger32(DynamicMethod context, int value) 
    : LiteralSymbol<int>(context, value), INumberSymbol
{
    public INumberSymbol.RepresentationKind Representation 
        => INumberSymbol.RepresentationKind.Integer32;
    
    public override void EmitLoadContent()
        => Context.Code.Emit(OpCodes.Ldc_I4, Value);
}

public class LiteralUnsignedInteger32(DynamicMethod context, uint value)
    : LiteralSymbol<uint>(context, value), INumberSymbol
{
    public INumberSymbol.RepresentationKind Representation 
        => INumberSymbol.RepresentationKind.Integer32;
    
    public override void EmitLoadContent()
        => Context.Code.Emit(OpCodes.Ldc_I4, (int)Value);
}

public class LiteralInteger64(DynamicMethod context, long value)
    : LiteralSymbol<long>(context, value), INumberSymbol
{
    public INumberSymbol.RepresentationKind Representation 
        => INumberSymbol.RepresentationKind.Integer64;
    
    public override void EmitLoadContent()
        => Context.Code.Emit(OpCodes.Ldc_I8, Value);
}

public class LiteralUnsignedInteger64(DynamicMethod context, ulong value)
    : LiteralSymbol<ulong>(context, value), INumberSymbol
{
    public INumberSymbol.RepresentationKind Representation 
        => INumberSymbol.RepresentationKind.Integer64;
    
    public override void EmitLoadContent()
        => Context.Code.Emit(OpCodes.Ldc_I8, (long)Value);
}