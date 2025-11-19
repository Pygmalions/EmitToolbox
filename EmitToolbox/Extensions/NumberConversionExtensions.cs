using System.Diagnostics.Contracts;
using System.Numerics;
using EmitToolbox.Symbols;

namespace EmitToolbox.Extensions;

public static class NumberConversionExtensions
{
    private class ConvertingNumberByInstruction<TNumber>(ISymbol target, OpCode instruction)
        : OperationSymbol<TNumber>(target.Context)
        where TNumber : unmanaged, INumber<TNumber>
    {
        public override void LoadContent()
        {
            target.LoadAsValue();
            Context.Code.Emit(instruction);
        }
    }

    private class ConvertingNumberByConstructor<TNumber>(ISymbol target, ConstructorInfo constructor)
        : OperationSymbol<TNumber>(target.Context)
        where TNumber : struct, INumber<TNumber>
    {
        public override void LoadContent()
        {
            var code = Context.Code;
            var variable = code.DeclareLocal(typeof(TNumber));
            code.Emit(OpCodes.Ldloca, variable);
            target.LoadAsValue();
            code.Emit(OpCodes.Call, constructor);
            code.Emit(OpCodes.Ldloc, variable);
        }
    }

    extension<TNumber>(ISymbol<TNumber> self) where TNumber : struct, INumber<TNumber>
    {
        [Pure]
        public IOperationSymbol<byte> ToByte()
            => new ConvertingNumberByInstruction<byte>(self, OpCodes.Conv_I1);

        [Pure]
        public IOperationSymbol<sbyte> ToSByte()
            => new ConvertingNumberByInstruction<sbyte>(self, OpCodes.Conv_I1);

        [Pure]
        public IOperationSymbol<short> ToInt16()
            => new ConvertingNumberByInstruction<short>(self, OpCodes.Conv_I2);

        [Pure]
        public IOperationSymbol<ushort> ToUInt16()
            => new ConvertingNumberByInstruction<ushort>(self, OpCodes.Conv_U2);

        [Pure]
        public IOperationSymbol<int> ToInt32()
            => new ConvertingNumberByInstruction<int>(self, OpCodes.Conv_I4);

        [Pure]
        public IOperationSymbol<uint> ToUInt32()
            => new ConvertingNumberByInstruction<uint>(self, OpCodes.Conv_U4);

        [Pure]
        public IOperationSymbol<long> ToInt64()
            => new ConvertingNumberByInstruction<long>(self, OpCodes.Conv_I8);

        [Pure]
        public IOperationSymbol<ulong> ToUInt64()
            => new ConvertingNumberByInstruction<ulong>(self, OpCodes.Conv_U8);

        [Pure]
        public IOperationSymbol<long> ToIntPtr()
            => new ConvertingNumberByInstruction<long>(self, OpCodes.Conv_I);

        [Pure]
        public IOperationSymbol<ulong> ToUIntPtr()
            => new ConvertingNumberByInstruction<ulong>(self, OpCodes.Conv_U);

        [Pure]
        public IOperationSymbol<float> ToSingle()
            => new ConvertingNumberByInstruction<float>(self, OpCodes.Conv_R4);

        [Pure]
        public IOperationSymbol<double> ToDouble()
            => new ConvertingNumberByInstruction<double>(self, OpCodes.Conv_R8);

        [Pure]
        public IOperationSymbol<decimal> ToDecimal()
            => new ConvertingNumberByConstructor<decimal>(
                self, typeof(decimal).GetConstructor([typeof(TNumber)])
                      ?? throw new MissingMethodException(
                          $"Cannot find a suitable constructor for 'decimal' that takes a '{typeof(TNumber)}'."));

        [Pure]
        public IOperationSymbol<BigInteger> ToBigInteger()
            => new ConvertingNumberByConstructor<BigInteger>(
                self, typeof(BigInteger).GetConstructor([typeof(TNumber)])
                      ?? throw new MissingMethodException(
                          $"Cannot find a suitable constructor for 'BigInteger' that takes a '{typeof(TNumber)}'."));
    }
}