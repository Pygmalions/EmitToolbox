using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace EmitToolbox.Extensions;

public static class EmitLogicExtensions
{
    public static void EmitNoOperation(this ILGenerator code)
        => code.Emit(OpCodes.Nop);

    public static void If(this ILGenerator code,
        [InstantHandle] Action<ILGenerator> predicate,
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
    public static void IsTrue(this ILGenerator code)
    {
        code.Emit(OpCodes.Ldc_I4_1);
        code.Emit(OpCodes.Ceq);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IsFalse(this ILGenerator code)
    {
        code.Emit(OpCodes.Ldc_I4_0);
        code.Emit(OpCodes.Ceq);
    }

    /// <inheritdoc cref="OpCodes.Jmp"/>
    public static void JumpToMethod(this ILGenerator code, MethodInfo method)
        => code.Emit(OpCodes.Jmp, method);

    /// <inheritdoc cref="OpCodes.Br"/>
    public static void Goto(this ILGenerator code, Label label)
        => code.Emit(OpCodes.Br, label);

    /// <inheritdoc cref="OpCodes.Brtrue"/>
    public static void GotoIfTrue(this ILGenerator code, Label label)
        => code.Emit(OpCodes.Brtrue, label);

    /// <inheritdoc cref="OpCodes.Brfalse"/>
    public static void GotoIfFalse(this ILGenerator code, Label label)
        => code.Emit(OpCodes.Brfalse, label);

    /// <inheritdoc cref="OpCodes.Beq"/>
    public static void GotoIfEqual(this ILGenerator code, Label label)
        => code.Emit(OpCodes.Beq, label);
    
    public static void GotoIfNotEqual(this ILGenerator code, Label label)
        => code.Emit(OpCodes.Bne_Un, label);
    
    /// <inheritdoc cref="OpCodes.Bgt"/>
    public static void GotoIfGreater(this ILGenerator code, Label label)
        => code.Emit(OpCodes.Bgt, label);

    /// <inheritdoc cref="OpCodes.Bge"/>
    public static void GotoIfGreaterOrEqual(this ILGenerator code, Label label)
        => code.Emit(OpCodes.Bge, label);

    /// <inheritdoc cref="OpCodes.Blt"/>
    public static void GotoIfLess(this ILGenerator code, Label label)
        => code.Emit(OpCodes.Blt, label);

    /// <inheritdoc cref="OpCodes.Ble"/>
    public static void GotoIfLessOrEqual(this ILGenerator code, Label label)
        => code.Emit(OpCodes.Ble, label);
}