using System.Numerics;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework.Extensions;

public static class NumberConversionExtensions
{
    private class ConvertingNumberByInstruction<TNumber>(
        ISymbol target, OpCode instruction)
        : OperationSymbol<TNumber>([target])
        where TNumber : unmanaged, INumber<TNumber>
    {
        public override void LoadContent()
        {
            target.LoadAsValue();
            Context.Code.Emit(instruction);
        }
    }

    private class ConvertingNumberByConstructor<TNumber>(
        ISymbol target, ConstructorInfo constructor)
        : OperationSymbol<TNumber>([target])
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
        public OperationSymbol<byte> ToByte()
            => new ConvertingNumberByInstruction<byte>(self, OpCodes.Conv_I1);

        public OperationSymbol<sbyte> ToSByte()
            => new ConvertingNumberByInstruction<sbyte>(self, OpCodes.Conv_I1);

        public OperationSymbol<short> ToInt16()
            => new ConvertingNumberByInstruction<short>(self, OpCodes.Conv_I2);

        public OperationSymbol<ushort> ToUInt16()
            => new ConvertingNumberByInstruction<ushort>(self, OpCodes.Conv_U2);

        public OperationSymbol<int> ToInt32()
            => new ConvertingNumberByInstruction<int>(self, OpCodes.Conv_I4);

        public OperationSymbol<uint> ToUInt32()
            => new ConvertingNumberByInstruction<uint>(self, OpCodes.Conv_U4);

        public OperationSymbol<long> ToInt64()
            => new ConvertingNumberByInstruction<long>(self, OpCodes.Conv_I8);

        public OperationSymbol<ulong> ToUInt64()
            => new ConvertingNumberByInstruction<ulong>(self, OpCodes.Conv_U8);
        
        public OperationSymbol<long> ToIntPtr()
            => new ConvertingNumberByInstruction<long>(self, OpCodes.Conv_I);

        public OperationSymbol<ulong> ToUIntPtr()
            => new ConvertingNumberByInstruction<ulong>(self, OpCodes.Conv_U);

        public OperationSymbol<float> ToSingle()
            => new ConvertingNumberByInstruction<float>(self, OpCodes.Conv_R4);

        public OperationSymbol<double> ToDouble()
            => new ConvertingNumberByInstruction<double>(self, OpCodes.Conv_R8);

        public OperationSymbol<decimal> ToDecimal()
            => new ConvertingNumberByConstructor<decimal>(
                self, typeof(decimal).GetConstructor([typeof(TNumber)])
                      ?? throw new MissingMethodException(
                          $"Cannot find a suitable constructor for 'decimal' that takes a '{typeof(TNumber)}'."));

        public OperationSymbol<BigInteger> ToBigInteger()
            => new ConvertingNumberByConstructor<BigInteger>(
                self, typeof(BigInteger).GetConstructor([typeof(TNumber)])
                      ?? throw new MissingMethodException(
                          $"Cannot find a suitable constructor for 'BigInteger' that takes a '{typeof(TNumber)}'."));
    }
}