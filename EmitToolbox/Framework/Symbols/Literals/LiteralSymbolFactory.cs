using System.Runtime.CompilerServices;

namespace EmitToolbox.Framework.Symbols.Literals;

public static class LiteralSymbolFactory
{
    public static ISymbol Create(DynamicFunction context, object value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value is bool booleanValue)
            return new LiteralBooleanSymbol(context, booleanValue);
        if (value is string stringValue)
            return new LiteralStringSymbol(context, stringValue);
        
        if (value.GetType().IsEnum)
            return (ISymbol)Activator.CreateInstance(
                typeof(LiteralEnumSymbol<>).MakeGenericType(value.GetType()),
                context, value)!;
        
        if (value is char charValue)
            return new LiteralIntegerCharacterSymbol(context, charValue);
        if (value is sbyte integer8Value)
            return new LiteralInteger8Symbol(context, integer8Value);
        if (value is byte integerU8Value)
            return new LiteralUnsignedInteger8Symbol(context, integerU8Value);
        if (value is short integer16Value)
            return new LiteralInteger16Symbol(context, integer16Value);
        if (value is ushort integerU16Value)
            return new LiteralUnsignedInteger16Symbol(context, integerU16Value);
        if (value is int integer32Value)
            return new LiteralInteger32Symbol(context, integer32Value);
        if (value is uint integerU32Value)
            return new LiteralUnsignedInteger32Symbol(context, integerU32Value);
        if (value is long integer64Value)
            return new LiteralInteger64Symbol(context, integer64Value);
        if (value is ulong integerU64Value)
            return new LiteralUnsignedInteger64Symbol(context, integerU64Value);

        if (value is float float32Value)
            return new LiteralFloat32Symbol(context, float32Value);
        if (value is double float64Value)
            return new LiteralFloat64Symbol(context, float64Value);
        
        if (value is decimal decimalValue)
            return new LiteralDecimalSymbol(context, decimalValue);
        
        if (value is Type typeValue)
            return new LiteralTypeInfoSymbol(context, typeValue);
        if (value is FieldInfo fieldValue)
            return new LiteralFieldInfoSymbol(context, fieldValue);
        if (value is MethodInfo methodValue)
            return new LiteralMethodInfoSymbol(context, methodValue);
        if (value is PropertyInfo propertyValue)
            return new LiteralPropertyInfoSymbol(context, propertyValue);
        if (value is ConstructorInfo constructorValue)
            return new LiteralConstructorInfoSymbol(context, constructorValue);
        
        throw new InvalidOperationException($"Unsupported value type '{value.GetType()}'.");
    }
    
    public static ISymbol<TValue> Create<TValue>(DynamicFunction context, TValue value)
    {
        ArgumentNullException.ThrowIfNull(value);
        
        if (value is bool booleanValue)
            return Unsafe.As<ISymbol<TValue>>(new LiteralBooleanSymbol(context, booleanValue));
        if (value is string stringValue)
            return Unsafe.As<ISymbol<TValue>>(new LiteralStringSymbol(context, stringValue));
        
        if (typeof(TValue).IsEnum)
            return (ISymbol<TValue>)Activator.CreateInstance(
                typeof(LiteralEnumSymbol<>).MakeGenericType(typeof(TValue)),
                context, value)!;
        
        if (value is char charValue)
            return Unsafe.As<ISymbol<TValue>>(new LiteralIntegerCharacterSymbol(context, charValue));
        if (value is sbyte integer8Value)
            return Unsafe.As<ISymbol<TValue>>(new LiteralInteger8Symbol(context, integer8Value));
        if (value is byte integerU8Value)
            return Unsafe.As<ISymbol<TValue>>(new LiteralUnsignedInteger8Symbol(context, integerU8Value));
        if (value is short integer16Value)
            return Unsafe.As<ISymbol<TValue>>(new LiteralInteger16Symbol(context, integer16Value));
        if (value is ushort integerU16Value)
            return Unsafe.As<ISymbol<TValue>>(new LiteralUnsignedInteger16Symbol(context, integerU16Value));
        if (value is int integer32Value)
            return Unsafe.As<ISymbol<TValue>>(new LiteralInteger32Symbol(context, integer32Value));
        if (value is uint integerU32Value)
            return Unsafe.As<ISymbol<TValue>>(new LiteralUnsignedInteger32Symbol(context, integerU32Value));
        if (value is long integer64Value)
            return Unsafe.As<ISymbol<TValue>>(new LiteralInteger64Symbol(context, integer64Value));
        if (value is ulong integerU64Value)
            return Unsafe.As<ISymbol<TValue>>(new LiteralUnsignedInteger64Symbol(context, integerU64Value));
        
        if (value is float float32Value)
            return Unsafe.As<ISymbol<TValue>>(new LiteralFloat32Symbol(context, float32Value));
        if (value is double float64Value)
            return Unsafe.As<ISymbol<TValue>>(new LiteralFloat64Symbol(context, float64Value));
        
        if (value is decimal decimalValue)
            return Unsafe.As<ISymbol<TValue>>(new LiteralDecimalSymbol(context, decimalValue));
        
        if (value is Type typeValue)
            return Unsafe.As<ISymbol<TValue>>(new LiteralTypeInfoSymbol(context, typeValue));
        if (value is FieldInfo fieldValue)
            return Unsafe.As<ISymbol<TValue>>(new LiteralFieldInfoSymbol(context, fieldValue));
        if (value is MethodInfo methodValue)
            return Unsafe.As<ISymbol<TValue>>(new LiteralMethodInfoSymbol(context, methodValue));
        if (value is PropertyInfo propertyValue)
            return Unsafe.As<ISymbol<TValue>>(new LiteralPropertyInfoSymbol(context, propertyValue));
        if (value is ConstructorInfo constructorValue)
            return Unsafe.As<ISymbol<TValue>>(new LiteralConstructorInfoSymbol(context, constructorValue));
        
        throw new InvalidOperationException($"Unsupported value type '{value.GetType()}'.");
    }
}