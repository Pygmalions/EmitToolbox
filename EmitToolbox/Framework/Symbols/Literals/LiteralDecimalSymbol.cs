using EmitToolbox.Extensions;

namespace EmitToolbox.Framework.Symbols.Literals;

public readonly struct LiteralDecimalSymbol(DynamicMethod context, decimal value) : ILiteralSymbol<decimal>
{
    public DynamicMethod Context => context;
    
    public decimal Value => value;

    public void EmitContent()
    {
        var code = Context.Code;
        
        // Allocate a Span on stack and store it in a local variable.
        var variableBitsSpan = code.DeclareLocal(typeof(Span<int>));
        
        // Allocate a Span of 4 integers on the heap.
        code.Emit(OpCodes.Ldc_I4, 4 * sizeof(int));
        code.Emit(OpCodes.Conv_U);
        code.Emit(OpCodes.Localloc);
        code.Emit(OpCodes.Ldc_I4, 4);
        code.NewObject(typeof(Span<int>).GetConstructor([typeof(void*), typeof(int)])!);
        
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