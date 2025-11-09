using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace EmitToolbox.Extensions;

public static class EmitLogicExtensions
{
    extension(ILGenerator code)
    {
        public void EmitNoOperation()
            => code.Emit(OpCodes.Nop);

        public void If([InstantHandle] Action<ILGenerator> predicate,
            [InstantHandle] Action<ILGenerator>? whenTrue = null,
            [InstantHandle] Action<ILGenerator>? whenFalse = null)
        {
            whenTrue ??= EmitNoOperation;
            whenFalse ??= EmitNoOperation;

            predicate(code);

            var labelFalse = code.DefineLabel();
            var labelEnd = code.DefineLabel();

            code.Emit(OpCodes.Brfalse, labelFalse);
            whenTrue.Invoke(code);
            code.Emit(OpCodes.Br, labelEnd);
            code.MarkLabel(labelFalse);
            whenFalse.Invoke(code);
            code.MarkLabel(labelEnd);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void IsTrue()
        {
            code.Emit(OpCodes.Ldc_I4_1);
            code.Emit(OpCodes.Ceq);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void IsFalse()
        {
            code.Emit(OpCodes.Ldc_I4_0);
            code.Emit(OpCodes.Ceq);
        }

        /// <inheritdoc cref="OpCodes.Jmp"/>
        public void JumpToMethod(MethodInfo method)
            => code.Emit(OpCodes.Jmp, method);

        /// <inheritdoc cref="OpCodes.Br"/>
        public void Goto(Label label)
            => code.Emit(OpCodes.Br, label);

        /// <inheritdoc cref="OpCodes.Brtrue"/>
        public void GotoIfTrue(Label label)
            => code.Emit(OpCodes.Brtrue, label);

        /// <inheritdoc cref="OpCodes.Brfalse"/>
        public void GotoIfFalse(Label label)
            => code.Emit(OpCodes.Brfalse, label);

        /// <inheritdoc cref="OpCodes.Beq"/>
        public void GotoIfEqual(Label label)
            => code.Emit(OpCodes.Beq, label);

        public void GotoIfNotEqual(Label label)
            => code.Emit(OpCodes.Bne_Un, label);

        /// <inheritdoc cref="OpCodes.Bgt"/>
        public void GotoIfGreater(Label label)
            => code.Emit(OpCodes.Bgt, label);

        /// <inheritdoc cref="OpCodes.Bge"/>
        public void GotoIfGreaterOrEqual(Label label)
            => code.Emit(OpCodes.Bge, label);

        /// <inheritdoc cref="OpCodes.Blt"/>
        public void GotoIfLess(Label label)
            => code.Emit(OpCodes.Blt, label);

        /// <inheritdoc cref="OpCodes.Ble"/>
        public void GotoIfLessOrEqual(Label label)
            => code.Emit(OpCodes.Ble, label);
    }
}