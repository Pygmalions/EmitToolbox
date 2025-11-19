using EmitToolbox.Symbols.Literals;

namespace EmitToolbox.Symbols;

public interface ILiteralSymbol : ISymbol
{
    object Value { get; }
}

public interface ILiteralSymbol<out TContent> : ILiteralSymbol, ISymbol<TContent>
{
    new TContent Value { get; }

    Type ISymbol.ContentType => typeof(TContent);

    object ILiteralSymbol.Value => Value!;
}

public static class LiteralSymbolExtensions
{
    extension(DynamicFunction self)
    {
        public LiteralBooleanSymbol Value(bool value)
            => new(self, value);

        public LiteralDecimalSymbol Value(decimal value) 
            => new(self, value);

        public LiteralEnumSymbol<TEnum> Value<TEnum>(TEnum value) where TEnum : struct, Enum 
            => new(self, value);
        
        public LiteralStringSymbol Value(string value) => new(self, value);
        
        public LiteralFloat32Symbol Value(float value) => new(self, value);
        
        public LiteralFloat64Symbol Value(double value) => new(self, value);
        
        public LiteralInteger8Symbol Value(sbyte value) => new(self, value);

        public LiteralUnsignedInteger8Symbol Value(byte value) => new(self, value);

        public LiteralIntegerCharacterSymbol Value(char value) => new(self, value);

        public LiteralInteger16Symbol Value(short value) => new(self, value);

        public LiteralUnsignedInteger16Symbol Value(ushort value) => new(self, value);

        public LiteralInteger32Symbol Value(int value) => new(self, value);

        public LiteralUnsignedInteger32Symbol Value(uint value) => new(self, value);

        public LiteralInteger64Symbol Value(long value) => new(self, value);

        public LiteralUnsignedInteger64Symbol Value(ulong value) => new(self, value);
        
        public LiteralNullSymbol Null(Type type) => new(self, type);
        
        public LiteralNullSymbol<TContent> Null<TContent>() where TContent : class => new(self);
        
        public LiteralTypeInfoSymbol Value(Type type) => new(self, type);
        
        public LiteralFieldInfoSymbol Value(FieldInfo field) => new(self, field);
        
        public LiteralPropertyInfoSymbol Value(PropertyInfo property) => new(self, property);
        
        public LiteralMethodInfoSymbol Value(MethodInfo method) => new(self, method);
       
        public LiteralConstructorInfoSymbol Value(ConstructorInfo constructor) => new(self, constructor);
    }
}