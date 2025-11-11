namespace EmitToolbox.Framework.Builders;

public class ConstructorBuilderFacade(DynamicType context)
{
    public DynamicFunction<ConstructorBuilder, ConstructorInfo, Action> Define(
        VisibilityLevel visibility = VisibilityLevel.Public)
    {
        var attributes = MethodAttributes.HideBySig | MethodAttributes.SpecialName |
                         MethodAttributes.RTSpecialName | visibility.ToMethodAttributes();
        var builder = context.Builder.DefineDefaultConstructor(attributes);
        var code = builder.GetILGenerator();
        return new DynamicFunction<ConstructorBuilder, ConstructorInfo, Action>(
            builder,
            MethodBuilderFacade.CreateSearchMethodDelegate(builder),
            builder.SetCustomAttribute,
            MethodBuilderFacade.CreateReturnResultDelegate(code))
        {
            Context = context,
            Code = code,
            ParameterTypes = Type.EmptyTypes,
            ReturnType = typeof(void),
        };
    }

    public DynamicFunction<ConstructorBuilder, ConstructorInfo, Action> Define(
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
        return new DynamicFunction<ConstructorBuilder, ConstructorInfo, Action>(
            builder,
            MethodBuilderFacade.CreateSearchMethodDelegate(builder),
            builder.SetCustomAttribute,
            MethodBuilderFacade.CreateReturnResultDelegate(code))
        {
            Context = context,
            Code = code,
            ParameterTypes = parameterTypes,
            ReturnType = typeof(void),
        };
    }
}