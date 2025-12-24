using JetBrains.Annotations;

namespace EmitToolbox.Builders;

public class ConstructorBuilderFacade(DynamicType context)
{
    [MustUseReturnValue]
    public DynamicConstructor Define(VisibilityLevel visibility = VisibilityLevel.Public)
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
        var code = builder.GetILGenerator();
        return new DynamicConstructor(builder)
        {
            DeclaringType = context,
            Code = code,
            ParameterTypes = Type.EmptyTypes,
            ReturnType = typeof(void),
        };
        configure?.Invoke(builder);
    }

    [MustUseReturnValue]
    public DynamicConstructor Define(
        ParameterDefinition[] parameters, VisibilityLevel visibility = VisibilityLevel.Public)
    {
        var attributes = MethodAttributes.HideBySig | MethodAttributes.SpecialName |
                         MethodAttributes.RTSpecialName | visibility.ToMethodAttributes();
        var parameterTypes = parameters.SelectTypes().ToArray();
        var builder = context.Builder.DefineConstructor(
            attributes, CallingConventions.Standard,
            parameterTypes,
            parameters.ToRequiredCustomModifiers().ToArray(),
            parameters.ToOptionalCustomModifiers().ToArray());
        var code = builder.GetILGenerator();
        return new DynamicConstructor(builder)
        {
            DeclaringType = context,
            Code = code,
            ParameterTypes = parameterTypes,
            ReturnType = typeof(void),
        };
    }
}