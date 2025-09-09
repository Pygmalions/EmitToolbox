namespace EmitToolbox.Framework;

public enum ValueModifier
{
    None,
    Reference,
    Pointer
}

public static class ValueModifierExtensions
{
    public static Type WithModifier(this Type type, ValueModifier modifier) => modifier switch
    {
        ValueModifier.None => type,
        ValueModifier.Reference => type.MakeByRefType(),
        ValueModifier.Pointer => type.MakePointerType(),
        _ => throw new ArgumentOutOfRangeException(nameof(modifier), modifier, null)
    };
    
    public static Type WithoutModifier(this Type type)
    {
        return type.IsByRef || type.IsPointer ? type.GetElementType()! : type;
    }
}