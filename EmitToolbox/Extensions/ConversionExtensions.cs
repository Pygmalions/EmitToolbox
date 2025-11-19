using System.Diagnostics.Contracts;
using EmitToolbox.Symbols;
using EmitToolbox.Symbols.Literals;
using EmitToolbox.Symbols.Operations;
using EmitToolbox.Utilities;

namespace EmitToolbox.Extensions;

public static class ConversionExtensions
{
    private class CastingClass(ISymbol target, Type type) : OperationSymbol(target.Context, type)
    {
        public override void LoadContent()
        {
            target.LoadAsValue();
            Context.Code.Emit(OpCodes.Castclass, ContentType);
        }
    }

    private class TryCastingClass(ISymbol target, Type type) : OperationSymbol(target.Context, type)
    {
        public override void LoadContent()
        {
            target.LoadAsValue();
            Context.Code.Emit(OpCodes.Isinst, ContentType);
        }
    }

    private class IsInstanceOfType(ISymbol target, Type type)
        : OperationSymbol<bool>(target.Context)
    {
        public override void LoadContent()
        {
            target.LoadAsValue();
            Context.Code.Emit(OpCodes.Isinst, type);
            Context.Code.Emit(OpCodes.Ldnull);
            Context.Code.Emit(OpCodes.Cgt_Un);
        }
    }

    extension(ISymbol self)
    {
        /// <summary>
        /// Check if this symbol is an instance of the specified type.
        /// If this symbol is a value type, then the checking result will be a literal symbol.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>An operation symbol or a literal symbol of the checking result</returns>
        [Pure]
        public IOperationSymbol<bool> IsInstanceOf(Type type)
        {
            var basicType = self.BasicType;
            if (basicType.IsValueType)
                return new LiteralBooleanSymbol(self.Context, basicType == type).AsSymbol<bool>();
            return new IsInstanceOfType(self, type);
        }

        /// <summary>
        /// Check if this symbol is an instance of the specified type.
        /// If this symbol is a value type, then the checking result will be a literal symbol.
        /// </summary>
        /// <typeparam name="TTarget">Type to check.</typeparam>
        /// <returns>An operation symbol or a literal symbol of the checking result</returns>
        [Pure]
        public IOperationSymbol<bool> IsInstanceOf<TTarget>()
            => self.IsInstanceOf(typeof(TTarget));

        /// <summary>
        /// Cast this symbol to the specified type using 'OpCodes.Castclass',
        /// which has the same effect as the 'as' operator.
        /// </summary>
        [Pure]
        public IOperationSymbol CastTo(Type type)
        {
            if (self.BasicType.IsValueType)
                throw new InvalidOperationException(
                    $"Cannot cast a symbol of a value type '{self.BasicType}' to another type.");
            type = type.BasicType;
            return type.IsValueType 
                ? throw new InvalidOperationException($"Cannot cast this symbol to a value type '{type}'.") 
                : new CastingClass(self, type);
        }

        /// <summary>
        /// Cast this symbol to the specified type using 'OpCodes.Castclass',
        /// which has the same effect as the 'as' operator.
        /// </summary>
        [Pure]
        public IOperationSymbol TryCastTo(Type type)
        {
            if (self.BasicType.IsValueType)
                throw new InvalidOperationException(
                    $"Cannot cast a symbol of a value type '{self.BasicType}' to another type.");
            type = type.BasicType;
            return type.IsValueType 
                ? throw new InvalidOperationException($"Cannot cast this symbol to a value type '{type}'.") 
                : new TryCastingClass(self, type);
        }
        
        /// <summary>
        /// Cast this symbol to the specified type using 'OpCodes.Castclass',
        /// which has the same effect as the 'as' operator.
        /// </summary>
        /// <typeparam name="TTarget">Target type to cast this symbol to.</typeparam>
        /// <returns>Operation of this casting.</returns>
        [Pure]
        public IOperationSymbol<TTarget> CastTo<TTarget>() where TTarget : class
            => new CastingClass(self, typeof(TTarget)).AsSymbol<TTarget>();

        /// <summary>
        /// Try to cast this symbol to the specified type using 'OpCodes.Isinst',
        /// which has the same effect as the 'as' operator.
        /// If the cast fails, then the result will be null.
        /// </summary>
        /// <typeparam name="TTarget">Target type to cast this symbol to.</typeparam>
        /// <returns>Operation of this casting.</returns>
        [Pure]
        public IOperationSymbol<TTarget?> TryCastTo<TTarget>() where TTarget : class
            => new TryCastingClass(self, typeof(TTarget)).AsSymbol<TTarget?>();

        /// <summary>
        /// Convert this symbol to the specified type by trying following rules in sequence:
        /// <br/> 1. If the source type is assignable to the target type, then no conversion is needed.
        /// <br/> 2. If any symbol is an object, then use conditional boxing or unboxing.
        /// <br/> 3. If the source type has an explicit conversion operator to the target type, then use it.
        /// <br/> 4. If the target type has an explicit conversion operator to the source type, then use it.
        /// <br/> 5. If the target type has a public constructor that takes the target type as a parameter,
        /// then instantiate the target type.
        /// </summary>
        /// <param name="toType">Target type for this type to convert to.</param>
        /// <returns>Conversion operation.</returns>
        /// <exception cref="InvalidCastException">
        /// Thrown when the conversion is not possible through mentioned rules.
        /// </exception>
        public IOperationSymbol ConvertTo(Type toType)
        {
            var fromType = self.BasicType;

            // Check for assignment.
            if ((fromType.IsValueType && fromType == toType) ||
                (!fromType.IsValueType && fromType.IsAssignableTo(toType)))
                return new NoOperation(self);

            // Target type is object, then use conditional boxing.
            if (toType == typeof(object))
                return self.ToObject();

            // Target type is an object, and the source type is a value type, then use unboxing.
            if (fromType == typeof(object) && toType.IsValueType)
                return self.Unbox(toType);
            
            // Check for conversion operators on the source type.
            if (fromType.GetMethodByReturnType(
                    "op_Explicit", toType, [fromType],
                    BindingFlags.Public | BindingFlags.Static, true) is
                { } explicitConversionMethod)
                return new InvocationOperation(explicitConversionMethod, null, [self]);
            if (fromType.GetMethodByReturnType(
                    "op_Implicit", toType, [fromType],
                    BindingFlags.Public | BindingFlags.Static, true) is
                { } implicitConversionMethod)
                return new InvocationOperation(implicitConversionMethod, null, [self]);

            // Check for conversion operators on the target type.
            if (toType.GetMethodByReturnType(
                    "op_Explicit", toType, [fromType],
                    BindingFlags.Public | BindingFlags.Static, true) is
                { } targetExplicitConversionMethod)
                return new InvocationOperation(targetExplicitConversionMethod, null, [self]);
            if (toType.GetMethodByReturnType(
                    "op_Implicit", toType, [fromType],
                    BindingFlags.Public | BindingFlags.Static, true) is
                { } targetImplicitConversionMethod)
                return new InvocationOperation(targetImplicitConversionMethod, null, [self]);

            // Check for public constructors.
            if (toType.GetConstructor([toType]) is { } constructor)
                return new NoOperation(self.Context.New(constructor, [self]));

            if (!toType.IsValueType && !fromType.IsValueType)
                return new CastingClass(self, toType);
            
            throw new InvalidCastException(
                $"Cannot convert '{self.BasicType}' to '{toType.BasicType}'");
        }
        
        /// <summary>
        /// Convert this symbol to the specified type:
        /// <br/> 1. If the source type is assignable to the target type, then no conversion is needed.
        /// <br/> 2. If any symbol is an object, then use conditional boxing or unboxing.
        /// <br/> 3. If the source type has an explicit conversion operator to the target type, then use it.
        /// <br/> 4. If the target type has an explicit conversion operator to the source type, then use it.
        /// <br/> 5. If the target type has a public constructor that takes the target type as a parameter,
        /// then instantiate the target type.
        /// </summary>
        /// <typeparam name="TTarget">Target type for this type to convert to.</typeparam>
        /// <returns>Conversion operation.</returns>
        /// <exception cref="InvalidCastException">
        /// Thrown when the conversion is not possible through mentioned rules.
        /// </exception>
        [Pure]
        public IOperationSymbol<TTarget> ConvertTo<TTarget>() where TTarget : allows ref struct
            => self.ConvertTo(typeof(TTarget)).AsSymbol<TTarget>();
    }
}