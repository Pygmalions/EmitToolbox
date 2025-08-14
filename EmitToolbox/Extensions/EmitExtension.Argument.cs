using System.Reflection.Emit;

namespace EmitToolbox.Extensions;

public static class EmitArgumentExtension
{
    public static void LoadArgument(this ILGenerator code, int index)
        => code.Emit(OpCodes.Ldarg, index);

    public static void LoadArgumentAddress(this ILGenerator code, int index)
        => code.Emit(OpCodes.Ldarga, index);

    public static void LoadArgument0(this ILGenerator code)
        => code.Emit(OpCodes.Ldarg_0);

    public static void LoadArgument1(this ILGenerator code)
        => code.Emit(OpCodes.Ldarg_1);

    public static void LoadArgument2(this ILGenerator code)
        => code.Emit(OpCodes.Ldarg_2);

    public static void LoadArgument3(this ILGenerator code)
        => code.Emit(OpCodes.Ldarg_3);
}