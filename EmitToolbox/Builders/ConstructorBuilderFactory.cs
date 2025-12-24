using JetBrains.Annotations;

namespace EmitToolbox.Builders;

public class ConstructorBuilderFactory(DynamicType context)
{
    private readonly List<ConstructorBuilder> _constructors = [];

    public IReadOnlyCollection<ConstructorBuilder> DefinedConstructors => _constructors;
    
    /// <summary>
    /// Define a parameterless constructor that simply calls the parent constructor.
    /// Note that its <see cref="ConstructorBuilder.GetILGenerator()"/> method is not allowed to access.
    /// </summary>
    /// <param name="visibility">
    /// Visibility of this constructor.
    /// Note that a struct must have a public parameterless constructor.
    /// </param>
    /// <param name="configure">Functor that further configures the constructor builder.</param>
    public void DefineDefaultConstructor(VisibilityLevel visibility = VisibilityLevel.Public,
        Action<ConstructorBuilder>? configure = null)
    {
        if (context.Builder.BaseType == typeof(ValueType) && visibility != VisibilityLevel.Public)
            throw new ArgumentException(
                "Failed to define the default constructor: " +
                "the parameterless constructor of a struct cannot be non-public.");
        var attributes = MethodAttributes.HideBySig | MethodAttributes.SpecialName |
                         MethodAttributes.RTSpecialName | visibility.ToMethodAttributes();
        var builder = context.Builder.DefineDefaultConstructor(attributes);
        configure?.Invoke(builder);
        _constructors.Add(builder);
    }

    [MustUseReturnValue]
    public DynamicConstructor Define(
        ParameterDefinition[] parameters, VisibilityLevel visibility = VisibilityLevel.Public)
    {
        if (context.Builder.BaseType == typeof(ValueType) && 
            parameters.Length == 0 &&
            visibility != VisibilityLevel.Public)
            throw new ArgumentException(
                "Failed to define the constructor: " +
                "the parameterless constructor of a struct cannot be non-public.");
        var attributes = MethodAttributes.HideBySig | MethodAttributes.SpecialName |
                         MethodAttributes.RTSpecialName | visibility.ToMethodAttributes();
        var parameterTypes = parameters.SelectTypes().ToArray();
        var builder = context.Builder.DefineConstructor(
            attributes, CallingConventions.Standard,
            parameterTypes,
            parameters.ToRequiredCustomModifiers().ToArray(),
            parameters.ToOptionalCustomModifiers().ToArray());
        var code = builder.GetILGenerator();
        _constructors.Add(builder);
        return new DynamicConstructor(builder)
        {
            DeclaringType = context,
            Code = code,
            ParameterTypes = parameterTypes,
            ReturnType = typeof(void),
        };
    }
}