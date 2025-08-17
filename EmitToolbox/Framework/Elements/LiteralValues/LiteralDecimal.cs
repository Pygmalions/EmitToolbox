using EmitToolbox.Extensions;

namespace EmitToolbox.Framework.Elements.LiteralValues;

public class LiteralDecimal(MethodContext context, decimal value) : LiteralValueElement<decimal>(context, value)
{
    protected internal override void EmitLoadAsValue()
    {
        // Allocate a Span on stack and store it in a local variable.
        var variableBitsSpan = Context.Code.DeclareLocal(typeof(Span<int>));
        Context.Code.AllocateSpanOnStack<int>(4);
        Context.Code.Emit(OpCodes.Stloc, variableBitsSpan);
        // decimal.GetBits(...) only takes 4 integers.
        Span<int> bits = stackalloc int[4];
        decimal.GetBits(Value, bits);

        // Store the bits into the Span.
        for (var bitIndex = 0; bitIndex < 4; ++bitIndex)
        {
            Context.Code.LoadLocalAddress(variableBitsSpan);
            Context.Code.GetSpanItemReference<int>(bitIndex);
            Context.Code.Emit(OpCodes.Ldc_I4, bits[bitIndex]);
            Context.Code.Emit(OpCodes.Stind_I4);
        }

        // Convert it into a read-only Span.
        var variableDecimal = Context.Code.DeclareLocal(typeof(decimal));
        Context.Code.LoadLocalAddress(variableDecimal);
        Context.Code.LoadLocal(variableBitsSpan);
        Context.Code.ConvertSpanToReadOnlySpan<int>();
        Context.Code.Call(typeof(decimal).GetConstructor([typeof(ReadOnlySpan<int>)])!);
        Context.Code.LoadLocal(variableDecimal);
    }
}