using System.Numerics;
using EmitToolbox.Extensions;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework.Extensions;

public static class NumberConversionExtensions
{
    private class NumberConvertByInstruction<TNumber>(
        ISymbol target, OpCode instruction)
        : OperationSymbol<TNumber>([target])
        where TNumber : unmanaged, INumber<TNumber>
    {
        public override void EmitContent()
        {
            target.EmitAsValue();
            Context.Code.Emit(instruction);
        }
    }

    private class NumberConvertByConstructor<TNumber>(
        ISymbol target, ConstructorInfo constructor)
        : OperationSymbol<TNumber>([target])
        where TNumber : struct, INumber<TNumber>
    {
        public override void EmitContent()
        {
            var code = Context.Code;
            var variable = code.DeclareLocal(typeof(TNumber));
            code.Emit(OpCodes.Ldloca, variable);
            target.EmitAsValue();
            code.Call(constructor);
            code.Emit(OpCodes.Ldloc, variable);
        }
    }
    
    extension<TNumber>(ISymbol<TNumber> self) where TNumber : struct, INumber<TNumber>
    {
        public OperationSymbol<byte> ToByte()
            => new NumberConvertByInstruction<byte>(self, OpCodes.Conv_I1);

        public OperationSymbol<sbyte> ToSByte()
            => new NumberConvertByInstruction<sbyte>(self, OpCodes.Conv_I1);

        public OperationSymbol<short> ToInt16()
            => new NumberConvertByInstruction<short>(self, OpCodes.Conv_I2);

        public OperationSymbol<ushort> ToUInt16()
            => new NumberConvertByInstruction<ushort>(self, OpCodes.Conv_U2);

        public OperationSymbol<int> ToInt32()
            => new NumberConvertByInstruction<int>(self, OpCodes.Conv_I4);

        public OperationSymbol<uint> ToUInt32()
            => new NumberConvertByInstruction<uint>(self, OpCodes.Conv_U4);

        public OperationSymbol<long> ToInt64()
            => new NumberConvertByInstruction<long>(self, OpCodes.Conv_I8);

        public OperationSymbol<ulong> ToUInt64()
            => new NumberConvertByInstruction<ulong>(self, OpCodes.Conv_U8);

        public OperationSymbol<float> ToSingle()
            => new NumberConvertByInstruction<float>(self, OpCodes.Conv_R4);

        public OperationSymbol<double> ToDouble()
            => new NumberConvertByInstruction<double>(self, OpCodes.Conv_R8);

        public OperationSymbol<decimal> ToDecimal()
            => new NumberConvertByConstructor<decimal>(
                self, typeof(decimal).GetConstructor([typeof(TNumber)])
                      ?? throw new MissingMethodException(
                          $"Cannot find a suitable constructor for 'decimal' that takes a '{typeof(TNumber)}'."));

        public OperationSymbol<BigInteger> ToBigInteger()
            => new NumberConvertByConstructor<BigInteger>(
                self, typeof(BigInteger).GetConstructor([typeof(TNumber)])
                      ?? throw new MissingMethodException(
                          $"Cannot find a suitable constructor for 'BigInteger' that takes a '{typeof(TNumber)}'."));
    }
}