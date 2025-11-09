namespace EmitToolbox.Framework;

public enum VisibilityLevel
{
    Private = 1,
    Protected = 2,
    Internal = 3,
    ProtectedInternal = 4,
    Public = 5,
}

public static class VisibilityLevelExtensions
{
    public static TypeAttributes ToTypeAttributes(this VisibilityLevel visibility)
    {
        return visibility switch
        {
            VisibilityLevel.Public => TypeAttributes.Public,
            VisibilityLevel.Internal => TypeAttributes.NotPublic | TypeAttributes.NestedAssembly,
            VisibilityLevel.Protected => TypeAttributes.NestedFamily,
            VisibilityLevel.Private => TypeAttributes.NestedPrivate,
            VisibilityLevel.ProtectedInternal => TypeAttributes.NestedFamORAssem,
            _ => throw new ArgumentOutOfRangeException(nameof(visibility), visibility, null)
        };
    }
    
    public static MethodAttributes ToMethodAttributes(this VisibilityLevel visibility)
    {
        return visibility switch
        {
            VisibilityLevel.Public => MethodAttributes.Public,
            VisibilityLevel.Internal => MethodAttributes.Assembly,
            VisibilityLevel.Protected => MethodAttributes.Family,
            VisibilityLevel.Private => MethodAttributes.Private,
            VisibilityLevel.ProtectedInternal => MethodAttributes.FamORAssem,
            _ => throw new ArgumentOutOfRangeException(nameof(visibility), visibility, null)
        };
    }
    
    public static FieldAttributes ToFieldAttributes(this VisibilityLevel visibility)
    {
        return visibility switch
        {
            VisibilityLevel.Public => FieldAttributes.Public,
            VisibilityLevel.Internal => FieldAttributes.Assembly,
            VisibilityLevel.Protected => FieldAttributes.Family,
            VisibilityLevel.Private => FieldAttributes.Private,
            VisibilityLevel.ProtectedInternal => FieldAttributes.FamORAssem,
            _ => throw new ArgumentOutOfRangeException(nameof(visibility), visibility, null)
        };
    }
}