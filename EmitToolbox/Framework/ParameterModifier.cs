using System.Runtime.InteropServices;

namespace EmitToolbox.Framework;

public enum ParameterModifier
{
    None,
    In,
    Out
}

public static class ParameterModifierExtensions
{
    private static readonly Type[] AttributeIn = [typeof(InAttribute)];
    private static readonly Type[] AttributeOut = [typeof(OutAttribute)];
    
    public static Type[] ToCustomAttributes(this ParameterModifier modifier)
    {
        return modifier switch
        {
            ParameterModifier.None => Type.EmptyTypes,
            ParameterModifier.In => AttributeIn,
            ParameterModifier.Out => AttributeOut,
            _ => throw new ArgumentOutOfRangeException(nameof(modifier), modifier, null)
        };
    }
}