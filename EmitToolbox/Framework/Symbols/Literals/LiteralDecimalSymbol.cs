using EmitToolbox.Framework.Extensions;

namespace EmitToolbox.Framework.Symbols.Literals;

public readonly struct LiteralDecimalSymbol(DynamicMethod context, decimal value) : ILiteralSymbol<decimal>
{
    public DynamicMethod Context => context;
    
    public decimal Value => value;

    public void EmitContent()
    {
        var variableBits = 
            Context.StackAllocate<int>(Context.Value(4));
        
        // decimal.GetBits(...) only takes 4 integers.
        Span<int> bits = stackalloc int[4];
        decimal.GetBits(Value, bits);
        
        // Store the bits into the Span.
        for (var bitIndex = 0; bitIndex < 4; ++bitIndex)
        {
            variableBits
                .ElementAt(Context.Value(bitIndex))
                .CopyValueFrom(Context.Value(bits[bitIndex]));
        }
        
        Context.New<decimal>(typeof(decimal).GetConstructor([typeof(ReadOnlySpan<int>)])!,
                variableBits.ConvertTo<ReadOnlySpan<int>>())
            .EmitContent();
    }
}