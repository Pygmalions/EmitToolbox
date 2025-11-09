namespace EmitToolbox.Extensions;

public static class EmitBoxExtensions
{
    extension(ILGenerator code)
    {
        public void Box(Type type)
        {
            code.Emit(OpCodes.Box, type);
        }

        public void Box<TType>()
        {
            code.Emit(OpCodes.Box, typeof(TType));
        }

        public void Unbox(Type type)
        {
            code.Emit(OpCodes.Unbox, type);
        }

        public void UnboxAny(Type type)
        {
            code.Emit(OpCodes.Unbox_Any, type);
        }

        public void Unbox<TType>()
        {
            code.Emit(OpCodes.Unbox, typeof(TType));
        }

        public void UnboxAny<TType>()
        {
            code.Emit(OpCodes.Unbox_Any, typeof(TType));
        }
    }
}