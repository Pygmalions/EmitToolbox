namespace EmitToolbox.Extensions;

public static class EmitArgumentExtensions
{
    extension(ILGenerator code)
    {
        public void LoadArgument(int index)
            => code.Emit(OpCodes.Ldarg, index);

        public void LoadArgumentAddress(int index)
            => code.Emit(OpCodes.Ldarga, index);

        public void LoadArgument_0()
            => code.Emit(OpCodes.Ldarg_0);

        public void LoadArgument_1()
            => code.Emit(OpCodes.Ldarg_1);

        public void LoadArgument_2()
            => code.Emit(OpCodes.Ldarg_2);

        public void LoadArgument_3()
            => code.Emit(OpCodes.Ldarg_3);
    }
}