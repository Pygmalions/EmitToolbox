using System.Reflection.Emit;

namespace EmitToolbox.Extensions;

public static class EmitBoxExtensions
{
    public static void Box(this ILGenerator code, Type type)
    {
        code.Emit(OpCodes.Box, type);
    }

    public static void Box<TType>(this ILGenerator code)
    {
        code.Emit(OpCodes.Box, typeof(TType));
    }

    public static void Unbox(this ILGenerator code, Type type)
    {
        code.Emit(OpCodes.Unbox, type);
    }

    public static void UnboxAny(this ILGenerator code, Type type)
    {
        code.Emit(OpCodes.Unbox_Any, type);
    }

    public static void Unbox<TType>(this ILGenerator code)
    {
        code.Emit(OpCodes.Unbox, typeof(TType));
    }

    public static void UnboxAny<TType>(this ILGenerator code)
    {
        code.Emit(OpCodes.Unbox_Any, typeof(TType));
    }
}