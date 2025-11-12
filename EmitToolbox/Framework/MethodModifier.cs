namespace EmitToolbox.Framework;

public enum MethodModifier
{
    None,
    Virtual,
    Abstract,
    New
}

public static class MethodModifierExtensions
{
    public static MethodAttributes ToMethodAttributes(this MethodModifier modifier, 
        bool hasSpecialName = false)
    {
        var attributes = modifier switch
        {
            MethodModifier.None => MethodAttributes.HideBySig,
            MethodModifier.Virtual => MethodAttributes.Virtual,
            MethodModifier.Abstract => MethodAttributes.Abstract,
            MethodModifier.New => MethodAttributes.NewSlot,
            _ => throw new ArgumentOutOfRangeException(nameof(modifier), modifier, null)
        };
        if (hasSpecialName)
            attributes |= MethodAttributes.SpecialName;
        return attributes;
    }
}