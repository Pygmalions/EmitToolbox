using System.Runtime.CompilerServices;

namespace EmitToolbox.Extensions;

public static class EmitSpanExtensions
{
    extension(ILGenerator code)
    {
        /// <summary>
        /// This method will push a Span onto the stack.
        /// </summary>
        public void AllocateSpanOnStack<TItem>(int size)
        {
            code.Emit(OpCodes.Ldc_I4, size * Unsafe.SizeOf<TItem>());
            code.Emit(OpCodes.Conv_U);
            code.Emit(OpCodes.Localloc);
            code.Emit(OpCodes.Ldc_I4, size);
            code.NewObject(typeof(Span<TItem>).GetConstructor([typeof(void*), typeof(int)])!);
        }

        /// <summary>
        /// This method takes a Span and pushes a ReadOnlySpan onto the stack.
        /// </summary>
        public void ConvertSpanToReadOnlySpan<TItem>()
        {
            // Currently, there is only one implicit conversion operator of Span<T>, which is to ReadOnlySpan<T>.
            code.Call(typeof(Span<TItem>).GetMethod("op_Implicit",
                BindingFlags.Public | BindingFlags.Static,
                [typeof(Span<TItem>)])!);
        }

        /// <summary>
        /// This method takes a reference to a Span,
        /// and pushes the reference to the item at the index onto the stack.
        /// </summary>
        public void GetSpanItemReference<TItem>(int index)
        {
            code.LoadLiteral(index);
            code.Call(typeof(Span<TItem>).GetMethod("get_Item",
                BindingFlags.Public | BindingFlags.Instance,
                [typeof(int)])!);
        }
    }
}