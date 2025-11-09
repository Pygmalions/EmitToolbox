using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework.Extensions;

public static class ReferenceExtensions
{
    extension(ISymbol self)
    {
        /// <summary>
        /// Check if this symbol can be loaded as a reference without wrapping it into a temporary variable:
        /// <br/> - when it is of a by-ref type,
        /// <br/> - or when it is addressable.
        /// </summary>
        public bool CanLoadAsReference
            => self
                is { ContentType.IsByRef: true }
                or IAddressableSymbol { ContentType.IsByRef: false };
    }

    extension(ISymbol self)
    {
        /// <summary>
        /// Emit this symbol as a value:
        /// <br/> - if it is not a by-ref type,
        /// then its content will be emitted as calling <see cref="ISymbol.EmitContent"/>;
        /// <br/> - if it is a by-ref type,
        /// then it will be dereferenced, and the value will be emitted.
        /// </summary>
        /// <exception cref="Exception">Throw if it is of an supported primitive type.</exception>
        public void EmitAsValue()
        {
            if (!self.ContentType.IsByRef)
            {
                self.EmitContent();
                return;
            }

            var basicType = self.BasicType;

            var code = self.Context.Code;

            // Handle class types.
            if (!basicType.IsValueType)
            {
                self.EmitContent();
                code.Emit(OpCodes.Ldind_Ref);
                return;
            }

            // Handle struct types.
            if (!basicType.IsPrimitive)
            {
                self.EmitContent();
                code.Emit(OpCodes.Ldobj, basicType);
                return;
            }

            // Handle primitive types.
            self.EmitContent();

            if (basicType == typeof(bool) || basicType == typeof(sbyte))
                code.Emit(OpCodes.Ldind_I1);
            else if (basicType == typeof(byte))
                code.Emit(OpCodes.Ldind_U1);
            else if (basicType == typeof(short) || basicType == typeof(char))
                code.Emit(OpCodes.Ldind_I2);
            else if (basicType == typeof(ushort))
                code.Emit(OpCodes.Ldind_U2);
            else if (basicType == typeof(int))
                code.Emit(OpCodes.Ldind_I4);
            else if (basicType == typeof(uint))
                code.Emit(OpCodes.Ldind_U4);
            else if (basicType == typeof(long) || basicType == typeof(ulong))
                code.Emit(OpCodes.Ldind_I8);
            else if (basicType == typeof(float))
                code.Emit(OpCodes.Ldind_R4);
            else if (basicType == typeof(double))
                code.Emit(OpCodes.Ldind_R8);
            else if (basicType == typeof(nint) || basicType == typeof(nuint))
                code.Emit(OpCodes.Ldind_I);
            else
                throw new Exception($"Unrecognized primitive type: '{basicType}'.");
        }

        /// <summary>
        /// Emit this symbol as a reference:
        /// <br/> 1. if it is of a by-ref type,
        /// then its content will be emitted as calling <see cref="ISymbol.EmitContent"/>;
        /// <br/> 2. if it is <see cref="IAddressableSymbol"/>,
        /// it will be emitted as calling <see cref="IAddressableSymbol.EmitAddress"/>.
        /// <br/> 3. if <paramref name="allowTemporary"/> is true,
        /// it will be stored in a local variable whose address will be emitted.
        /// </summary>
        /// <param name="allowTemporary">
        /// If true, the symbol will be stored in a temporary variable if it is not addressable nor of a by-ref type.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Throw when a symbol is not addressable nor of a by-ref type and <paramref name="allowTemporary"/> is false.
        /// </exception>
        public void EmitAsReference(bool allowTemporary = false)
        {
            if (self.ContentType.IsByRef)
            {
                self.EmitContent();
                return;
            }

            if (self is IAddressableSymbol addressable)
            {
                addressable.EmitAddress();
                return;
            }
            
            if (!allowTemporary)
                throw new InvalidOperationException(
                    "Cannot emit this symbol as a reference: it is not addressable nor of a by-ref type.");

            var code = self.Context.Code;
            var temporary = code.DeclareLocal(self.ContentType);
            self.EmitContent();
            code.Emit(OpCodes.Stloc, temporary);
            code.Emit(OpCodes.Ldloca, temporary);
        }

        /// <summary>
        /// Emit this symbol as a target symbol for calls of instance methods.
        /// </summary>
        public void EmitAsTarget()
        {
            if (!self.ContentType.IsValueType)
                self.EmitAsValue();
            else
                self.EmitAsReference(true);
        }

        /// <summary>
        /// Emit this symbol for filling in the specified parameter:
        /// <br/> - if the parameter has a by-ref modifier, including 'in', 'out' or 'ref',
        /// the symbol will be emitted as a reference;
        /// <br/> - otherwise, the symbol will be emitted as a value.
        /// </summary>
        /// <param name="parameter">The parameter for this symbol to suit.</param>
        public void EmitForParameter(ParameterInfo parameter)
        {
            if (parameter is { IsIn: false, IsOut: false, ParameterType.IsByRef: false })
                self.EmitAsValue();
            else
                self.EmitAsReference(true);
        }

        /// <summary>
        /// Emit this symbol for filling in the specified type:
        /// <br/> - if the type is a by-ref type, then it will be emitted as a reference;
        /// <br/> - otherwise, it will be emitted as a value.
        /// </summary>
        /// <param name="type">The type for this symbol to suit.</param>
        public void EmitForType(Type type)
        {
            if (!type.IsByRef)
                self.EmitAsValue();
            else
                self.EmitAsReference(true);
        }

        /// <summary>
        /// Emit this symbol for filling in the specified symbol:
        /// <br/> - if the symbol is of a by-ref type, then it will be emitted as a reference;
        /// <br/> - otherwise, it will be emitted as a value.
        /// </summary>
        /// <param name="symbol">The symbol for this symbol to suit.</param>
        public void EmitForSymbol(ISymbol symbol)
        {
            if (symbol.ContentType.IsByRef)
                self.EmitAsReference();
            else
                self.EmitAsValue();
        }
    }
}