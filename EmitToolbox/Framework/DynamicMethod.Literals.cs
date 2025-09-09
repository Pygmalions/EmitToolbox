using EmitToolbox.Framework.Symbols.Literals;

namespace EmitToolbox.Framework;

public partial class DynamicMethod
{
    public LiteralNull<TType> Null<TType>() where TType : class => new(this);
    
    public LiteralBoolean Value(bool value) => new (this, value);
    
    public LiteralInteger8 Value(sbyte value) => new(this, value);
    
    public LiteralUnsignedInteger8 Value(byte value) => new(this, value);
    
    public LiteralInteger16 Value(short value) => new(this, value);
    
    public LiteralUnsignedInteger16 Value(ushort value) => new(this, value);
    
    public LiteralInteger32 Value(int value) => new(this, value);
    
    public LiteralUnsignedInteger32 Value(uint value) => new(this, value);
    
    public LiteralInteger64 Value(long value) => new(this, value);
    
    public LiteralUnsignedInteger64 Value(ulong value) => new(this, value);
    
    public LiteralFloat Value(float value) => new(this, value);
    
    public LiteralDouble Value(double value) => new(this, value);
    
    public LiteralDecimal Value(decimal value) => new(this, value);
    
    public LiteralIntegerCharacter Value(char value) => new(this, value);
    
    public LiteralString Value(string value) => new(this, value);
    
    public LiteralEnum<TEnum> Value<TEnum>(TEnum value) where TEnum : struct, Enum 
        => new(this, value);
}