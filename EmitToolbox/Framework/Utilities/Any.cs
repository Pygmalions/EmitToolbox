namespace EmitToolbox.Framework.Utilities;

// The placeholder type for parameters. This class should only be used in expressions and never be instantiated.
public static class Any<TValue>
{
    public static TValue Value
        => throw new InvalidOperationException("This placeholder should only be used in expressions.");
    
    public static ref TValue ByRef
        => throw new InvalidOperationException("This placeholder should only be used in expressions.");
    
    public static TValue? Nullable 
        => throw new InvalidOperationException("This placeholder should only be used in expressions.");
}