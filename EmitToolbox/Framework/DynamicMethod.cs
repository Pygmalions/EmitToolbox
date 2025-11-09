using System.Diagnostics.CodeAnalysis;

namespace EmitToolbox.Framework;

public abstract class DynamicMethod(MethodBase builder) : IAttributeMarker<DynamicMethod>
{
    /// <summary>
    /// IL instructions stream of this method.
    /// </summary>
    public required ILGenerator Code { get; init; }

    public required DynamicType Context { get; init;  }

    /// <summary>
    /// Types of parameters.
    /// </summary>
    public required Type[] ParameterTypes { get; init; }
    
    /// <summary>
    /// Type of the return value.
    /// </summary>
    public required Type ReturnType { get; init; }
    
    /// <summary>
    /// Refer to the method builder when <see cref="Context"/> is not built;
    /// and refer to the built method when <see cref="Context"/> is built.
    /// </summary>
    [field: MaybeNull]
    public MethodBase BuildingMethod
    {
        get
        {
            if (field is not null)
                return field;
            if (!Context.IsBuilt)
                return builder;
            field = SearchBuiltMethod(Context.BuildingType);
            return field;
        }
    } = null!;

    public abstract DynamicMethod MarkAttribute(CustomAttributeBuilder attributeBuilder);

    protected abstract MethodBase SearchBuiltMethod(Type type);
}