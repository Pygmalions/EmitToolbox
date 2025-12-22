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
        public LiteralBooleanSymbol Literal(bool value)
            => new(self, value);

        public LiteralDecimalSymbol Literal(decimal value) 
            => new(self, value);

        public LiteralEnumSymbol<TEnum> Literal<TEnum>(TEnum value) where TEnum : struct, Enum 
            => new(self, value);
        
        public LiteralStringSymbol Literal(string value) => new(self, value);
        
        public LiteralFloat32Symbol Literal(float value) => new(self, value);
        
        public LiteralFloat64Symbol Literal(double value) => new(self, value);
        
        public LiteralInteger8Symbol Literal(sbyte value) => new(self, value);

        public LiteralUnsignedInteger8Symbol Literal(byte value) => new(self, value);

        public LiteralIntegerCharacterSymbol Literal(char value) => new(self, value);

        public LiteralInteger16Symbol Literal(short value) => new(self, value);

        public LiteralUnsignedInteger16Symbol Literal(ushort value) => new(self, value);

        public LiteralInteger32Symbol Literal(int value) => new(self, value);

        public LiteralUnsignedInteger32Symbol Literal(uint value) => new(self, value);

        public LiteralInteger64Symbol Literal(long value) => new(self, value);

        public LiteralUnsignedInteger64Symbol Literal(ulong value) => new(self, value);
        
        public LiteralNullSymbol Null(Type type) => new(self, type);
        
        public LiteralNullSymbol<TContent> Null<TContent>() where TContent : class => new(self);
        
        public LiteralTypeInfoSymbol Literal(Type type) => new(self, type);
        
        public LiteralFieldInfoSymbol Literal(FieldInfo field) => new(self, field);
        
        public LiteralPropertyInfoSymbol Literal(PropertyInfo property) => new(self, property);
        
        public LiteralMethodInfoSymbol Literal(MethodInfo method) => new(self, method);
       
        public LiteralConstructorInfoSymbol Literal(ConstructorInfo constructor) => new(self, constructor);
    }
}