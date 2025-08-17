using EmitToolbox.Framework.Elements.LiteralValues;

namespace EmitToolbox.Framework;

public partial class MethodContext
{
    public LiteralNull<TType> EmplaceNull<TType>() where TType : class => new(this);
    
    public LiteralBoolean EmplaceValue(bool value) => new (this, value);
    
    public LiteralInteger8 EmplaceValue(sbyte value) => new(this, value);
    
    public LiteralIntegerU8 EmplaceValue(byte value) => new(this, value);
    
    public LiteralInteger16 EmplaceValue(short value) => new(this, value);
    
    public LiteralIntegerU16 EmplaceValue(ushort value) => new(this, value);
    
    public LiteralInteger32 EmplaceValue(int value) => new(this, value);
    
    public LiteralIntegerU32 EmplaceValue(uint value) => new(this, value);
    
    public LiteralInteger64 EmplaceValue(long value) => new(this, value);
    
    public LiteralIntegerU64 EmplaceValue(ulong value) => new(this, value);
    
    public LiteralFloat EmplaceValue(float value) => new(this, value);
    
    public LiteralDouble EmplaceValue(double value) => new(this, value);
    
    public LiteralDecimal EmplaceValue(decimal value) => new(this, value);
    
    public LiteralIntegerCharacter EmplaceValue(char value) => new(this, value);
    
    public LiteralString EmplaceValue(string value) => new(this, value);
    
    public LiteralEnum<TEnum> EmplaceValue<TEnum>(TEnum value) where TEnum : struct, Enum 
        => new(this, value);
}