namespace EmitToolbox.Framework.Builders;

public class ConstructorBuilderFacade(DynamicType context)
{
    public DynamicConstructor Define(VisibilityLevel visibility = VisibilityLevel.Public)
    {
        var attributes = MethodAttributes.HideBySig | MethodAttributes.SpecialName |
                         MethodAttributes.RTSpecialName | visibility.ToMethodAttributes();
        var builder = context.Builder.DefineDefaultConstructor(attributes);
        var code = builder.GetILGenerator();
        return new DynamicConstructor(builder)
        {
            Context = context,
            Code = code,
            ParameterTypes = Type.EmptyTypes,
            ReturnType = typeof(void),
        };
    }

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
            Context = context,
            Code = code,
            ParameterTypes = parameterTypes,
            ReturnType = typeof(void),
        };
    }
}