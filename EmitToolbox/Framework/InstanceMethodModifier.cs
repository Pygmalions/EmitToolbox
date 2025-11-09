namespace EmitToolbox.Framework;

public enum InstanceMethodModifier
{
    None,
    Virtual,
    Abstract,
    New
}

public static class MethodModifierExtensions
{
    public static MethodAttributes ToMethodAttributes(this InstanceMethodModifier modifier, 
        bool hasSpecialName = false)
    {
        var attributes = modifier switch
        {
            InstanceMethodModifier.None => MethodAttributes.HideBySig,
            InstanceMethodModifier.Virtual => MethodAttributes.Virtual,
            InstanceMethodModifier.Abstract => MethodAttributes.Abstract,
            InstanceMethodModifier.New => MethodAttributes.NewSlot,
            _ => throw new ArgumentOutOfRangeException(nameof(modifier), modifier, null)
        };
        if (hasSpecialName)
            attributes |= MethodAttributes.SpecialName;
        return attributes;
    }
}