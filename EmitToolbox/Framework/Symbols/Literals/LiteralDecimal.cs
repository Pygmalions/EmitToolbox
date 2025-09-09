using EmitToolbox.Extensions;

namespace EmitToolbox.Framework.Symbols.Literals;

public class LiteralDecimal(DynamicMethod context, decimal value) : LiteralSymbol<decimal>(context, value)
{
    public override void EmitLoadContent()
    {
        var code = Context.Code;
        
        // Allocate a Span on stack and store it in a local variable.
        var variableBitsSpan = code.DeclareLocal(typeof(Span<int>));
        code.AllocateSpanOnStack<int>(4);
        code.Emit(OpCodes.Stloc, variableBitsSpan);
        // decimal.GetBits(...) only takes 4 integers.
        Span<int> bits = stackalloc int[4];
        decimal.GetBits(Value, bits);

        // Store the bits into the Span.
        for (var bitIndex = 0; bitIndex < 4; ++bitIndex)
        {
            code.LoadLocalAddress(variableBitsSpan);
            code.GetSpanItemReference<int>(bitIndex);
            code.Emit(OpCodes.Ldc_I4, bits[bitIndex]);
            code.Emit(OpCodes.Stind_I4);
        }

        // Convert it into a read-only Span.
        var variableDecimal = code.DeclareLocal(typeof(decimal));
        code.LoadLocalAddress(variableDecimal);
        code.LoadLocal(variableBitsSpan);
        code.ConvertSpanToReadOnlySpan<int>();
        code.Call(typeof(decimal).GetConstructor([typeof(ReadOnlySpan<int>)])!);
        code.LoadLocal(variableDecimal);
    }
}