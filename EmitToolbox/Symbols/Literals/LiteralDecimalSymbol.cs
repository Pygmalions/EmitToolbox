using EmitToolbox.Extensions;

namespace EmitToolbox.Symbols.Literals;

public readonly struct LiteralDecimalSymbol(DynamicFunction context, decimal value) : ILiteralSymbol<decimal>
{
    public DynamicFunction Context => context;
    
    public decimal Value => value;

    public void LoadContent()
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
        
        variableBits.ConvertTo<ReadOnlySpan<int>>().LoadContent();
        Context.Code.Emit(OpCodes.Newobj, 
            typeof(decimal).GetConstructor([typeof(ReadOnlySpan<int>)])!);
    }
}