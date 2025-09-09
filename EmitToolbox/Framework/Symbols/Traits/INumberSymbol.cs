namespace EmitToolbox.Framework.Symbols.Traits;

/// <summary>
/// Symbols implemented this interface has a symbol representation.
/// </summary>
public interface INumberSymbol : ISymbol
{
    public enum RepresentationKind
    {
        Native,
        Integer32,
        Integer64,
        FloatingPoint32,
        FloatingPoint64
    }
    
    /// <summary>
    /// Type of the value that is actually loaded 
    /// </summary>
    RepresentationKind Representation { get; }
}

public static class NumberValueExtensions
{
    public static void EmitLoadAsNativeInteger(this INumberSymbol symbol)
    {
        symbol.EmitLoadContent();
        symbol.Context.Code.Emit(OpCodes.Conv_I);
    }
    
    public static void EmitLoadAsUnsignedNativeInteger(this INumberSymbol symbol)
    {
        symbol.EmitLoadContent();
        symbol.Context.Code.Emit(OpCodes.Conv_U);
    }
    
    public static void EmitLoadAsInteger8(this INumberSymbol symbol)
    {
        symbol.EmitLoadContent();
        symbol.Context.Code.Emit(OpCodes.Conv_I1);
    }
    
    public static void EmitLoadAsUnsignedInteger8(this INumberSymbol symbol)
    {
        symbol.EmitLoadContent();
        symbol.Context.Code.Emit(OpCodes.Conv_U1);
    }
    
    public static void EmitLoadAsInteger16(this INumberSymbol symbol)
    {
        symbol.EmitLoadContent();
        symbol.Context.Code.Emit(OpCodes.Conv_I2);
    }
    
    public static void EmitLoadAsUnsignedInteger16(this INumberSymbol symbol)
    {
        symbol.EmitLoadContent();
        symbol.Context.Code.Emit(OpCodes.Conv_U2);
    }
    
    public static void EmitLoadAsInteger32(this INumberSymbol symbol)
    {
        symbol.EmitLoadContent();
        symbol.Context.Code.Emit(OpCodes.Conv_I4);
    }
    
    public static void EmitLoadAsUnsignedInteger32(this INumberSymbol symbol)
    {
        symbol.EmitLoadContent();
        symbol.Context.Code.Emit(OpCodes.Conv_U4);
    }
    
    public static void EmitLoadAsInteger64(this INumberSymbol symbol)
    {
        symbol.EmitLoadContent();
        symbol.Context.Code.Emit(OpCodes.Conv_I8);
    }
    
    public static void EmitLoadAsUnsignedInteger64(this INumberSymbol symbol)
    {
        symbol.EmitLoadContent();
        symbol.Context.Code.Emit(OpCodes.Conv_U8);
    }
}