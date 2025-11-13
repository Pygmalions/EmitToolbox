using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using EmitToolbox.Framework.Symbols;
using EmitToolbox.Framework.Symbols.Literals;
using EmitToolbox.Framework.Symbols.Operations;
using EmitToolbox.Framework.Utilities;

namespace EmitToolbox.Framework.Extensions;

public static class ConversionExtensions
{
    private class CastingClass<TTarget>(ISymbol target) 
        : OperationSymbol<TTarget>(target.Context)
        where TTarget : class
    {
        public override void LoadContent()
        {
            target.LoadAsValue();
            Context.Code.Emit(OpCodes.Castclass, typeof(TTarget));
        }
    }

    private class TryCastingClass<TTarget>(ISymbol target)
        : OperationSymbol<TTarget?>([target])
        where TTarget : class?
    {
        public override void LoadContent()
        {
            target.LoadAsValue();
            Context.Code.Emit(OpCodes.Isinst, typeof(TTarget));
        }
    }

    private class IsInstanceOfType(ISymbol target, Type type)
        : OperationSymbol<bool>([target])
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
        public ISymbol<bool> IsInstanceOf(Type type)
        {
            var basicType = self.BasicType;
            if (basicType.IsValueType)
                return basicType == type
                    ? new LiteralBooleanSymbol(self.Context, true)
                    : new LiteralBooleanSymbol(self.Context, false);
            return new IsInstanceOfType(self, type);
        }

        /// <summary>
        /// Check if this symbol is an instance of the specified type.
        /// If this symbol is a value type, then the checking result will be a literal symbol.
        /// </summary>
        /// <typeparam name="TTarget">Type to check.</typeparam>
        /// <returns>An operation symbol or a literal symbol of the checking result</returns>
        [Pure]
        public ISymbol<bool> IsInstanceOf<TTarget>()
            => self.IsInstanceOf(typeof(TTarget));

        /// <summary>
        /// Cast this symbol to the specified type using 'OpCodes.Castclass',
        /// which has the same effect as the 'as' operator.
        /// </summary>
        /// <typeparam name="TTarget">Target type to cast this symbol to.</typeparam>
        /// <returns>Operation of this casting.</returns>
        [Pure]
        public OperationSymbol<TTarget> CastTo<TTarget>() where TTarget : class
            => new CastingClass<TTarget>(self);

        /// <summary>
        /// Try to cast this symbol to the specified type using 'OpCodes.Isinst',
        /// which has the same effect as the 'as' operator.
        /// If the cast fails, then the result will be null.
        /// </summary>
        /// <typeparam name="TTarget">Target type to cast this symbol to.</typeparam>
        /// <returns>Operation of this casting.</returns>
        [Pure]
        public OperationSymbol<TTarget?> TryCastTo<TTarget>() where TTarget : class
            => new TryCastingClass<TTarget?>(self);
        
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
        public OperationSymbol<TTarget> ConvertTo<TTarget>() where TTarget : allows ref struct
        {
            var basicType = self.BasicType;
            var targetType = typeof(TTarget);

            // Check for assignment.
            if ((basicType.IsValueType && basicType == targetType) || 
                (!basicType.IsValueType && basicType.IsAssignableTo(targetType)))
                return new NoOperation<TTarget>(self);

            // Target type is object, then use conditional boxing.
            if (targetType == typeof(object))
                return Unsafe.As<OperationSymbol<TTarget>>(self.ToObject());

            // Target type is an object, and the source type is a value type, then use unboxing.
            if (basicType == typeof(object) && targetType.IsValueType)
                return Unsafe.As<OperationSymbol<TTarget>>(
                    Activator.CreateInstance(
                        typeof(BoxingExtensions.UnboxingAsValue<>)
                            .MakeGenericType(targetType), self)!);
            
            // Check for conversion operators on the source type.
            if (basicType.GetMethodByReturnType(
                    "op_Explicit", targetType, [basicType],
                    BindingFlags.Public | BindingFlags.Static, true) is
                { } explicitConversionMethod)
                return new InvocationOperation<TTarget>(explicitConversionMethod, null, [self]);
            if (basicType.GetMethodByReturnType(
                    "op_Implicit", targetType, [basicType],
                    BindingFlags.Public | BindingFlags.Static, true) is
                { } implicitConversionMethod)
                return new InvocationOperation<TTarget>(
                    implicitConversionMethod, null, [self]);

            // Check for conversion operators on the target type.
            if (targetType.GetMethodByReturnType(
                    "op_Explicit", targetType, [basicType],
                    BindingFlags.Public | BindingFlags.Static, true) is
                { } targetExplicitConversionMethod)
                return new InvocationOperation<TTarget>(targetExplicitConversionMethod, null, [self]);
            if (targetType.GetMethodByReturnType(
                    "op_Implicit", targetType, [basicType],
                    BindingFlags.Public | BindingFlags.Static, true) is
                { } targetImplicitConversionMethod)
                return new InvocationOperation<TTarget>(targetImplicitConversionMethod, null, [self]);

            // Check for public constructors.
            if (targetType.GetConstructor([targetType]) is { } constructor)
            {
                if (!targetType.IsValueType)
                    return new InstantiationExtensions.InstantiatingClass<TTarget>(
                        self.Context, constructor, [self]);

                var variable = self.Context.Variable<TTarget>();
                return new InstantiationExtensions.InstantiatingStruct<TTarget>(
                    variable, constructor, [self]);
            }

            if (!targetType.IsValueType && !basicType.IsValueType)
                return (OperationSymbol<TTarget>)Activator.CreateInstance(
                    typeof(CastingClass<>).MakeGenericType(targetType), self)!;
            
            throw new InvalidCastException(
                $"Cannot convert '{self.BasicType}' to '{targetType.BasicType}'");
        }
    }
}