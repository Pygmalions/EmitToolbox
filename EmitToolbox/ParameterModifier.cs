using System.Runtime.InteropServices;

namespace EmitToolbox;

public enum ParameterModifier
{
    None,
    In,
    Out
}

public static class ParameterModifierExtensions
{
    private static readonly Type[] AttributeIn = [typeof(InAttribute)];
    
    public static Type[] ToCustomAttributes(this ParameterModifier modifier)
    {
        return modifier switch
        {
            ParameterModifier.None => Type.EmptyTypes,
            ParameterModifier.In => AttributeIn,
            // Marking 'OutAttribute' to 'out' parameters will cause signature mismatching exceptions at runtime.
            ParameterModifier.Out => Type.EmptyTypes,
            _ => throw new ArgumentOutOfRangeException(nameof(modifier), modifier, null)
        };
    }
}