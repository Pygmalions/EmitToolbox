using System.Diagnostics.CodeAnalysis;
using Mono.Reflection;

namespace EmitToolbox;

public abstract class DynamicFunction(MethodBase builder) : IAttributeMarker
{
    /// <summary>
    /// IL instructions stream of this method.
    /// </summary>
    public required ILGenerator Code { get; init; }

    /// <summary>
    /// Dynamic type declaring this method.
    /// </summary>
    public required DynamicType DeclaringType { get; init;  }

    /// <summary>
    /// Types of parameters.
    /// </summary>
    public required Type[] ParameterTypes { get; init; }
    
    /// <summary>
    /// Type of the return value.
    /// </summary>
    public required Type ReturnType { get; init; }
    
    /// <summary>
    /// Refer to the method builder when <see cref="DeclaringType"/> is not built;
    /// and refer to the built method when <see cref="DeclaringType"/> is built.
    /// </summary>
    [field: MaybeNull]
    public MethodBase BuildingMethod
    {
        get
        {
            if (field is not null)
                return field;
            if (!DeclaringType.IsBuilt)
                return builder;
            field = SearchBuiltMethod(DeclaringType.BuildingType);
            return field;
        }
    } = null!;

    public abstract IAttributeMarker MarkAttribute(CustomAttributeBuilder attributeBuilder);

    protected abstract MethodBase SearchBuiltMethod(Type type);

    public void IgnoreVisibilityChecksToAssembly(Assembly assembly)
        => DeclaringType.DeclaringAssembly.IgnoreVisibilityChecksToAssembly(assembly);
    
    public void IgnoreVisibilityChecksToAssembly(string assemblyName)
        => DeclaringType.DeclaringAssembly.IgnoreVisibilityChecksToAssembly(assemblyName);

    public override string ToString()
    {
        return !DeclaringType.IsBuilt 
            ? $"Dynamic Method: {BuildingMethod.Name}" 
            : string.Join('\n', BuildingMethod.GetInstructions().Select(target => target.ToString()));
    }
}