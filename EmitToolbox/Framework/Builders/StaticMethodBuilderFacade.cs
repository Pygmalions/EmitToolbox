using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework.Builders;

public class StaticMethodBuilderFacade(DynamicType context)
{
    public DynamicFunction<MethodBuilder, MethodInfo, Action> DefineAction(
        string name, ParameterDefinition[]? parameters = null,
        VisibilityLevel visibility = VisibilityLevel.Public,
        bool hasSpecialName = false)
    {
        parameters ??= [];
        var attributes = visibility.ToMethodAttributes() | MethodAttributes.Static | MethodAttributes.HideBySig;
        if (hasSpecialName)
            attributes |= MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
        var builder = MethodBuilderFacade.CreateMethodBuilder(
            context.Builder, name, attributes,
            parameters, typeof(void), Type.EmptyTypes);
        var code = builder.GetILGenerator();
        return new DynamicFunction<MethodBuilder, MethodInfo, Action>(
            builder,
            MethodBuilderFacade.CreateSearchMethodDelegate(builder),
            builder.SetCustomAttribute,
            MethodBuilderFacade.CreateReturnResultDelegate(code))
        {
            Context = context,
            Code = code,
            ParameterTypes = parameters.SelectTypes().ToArray(),
            ReturnType = typeof(void),
        };
    }

    public DynamicFunction<MethodBuilder, MethodInfo, Action<ISymbol>> DefineFunctor(
        string name, Type result, ParameterDefinition[]? parameters = null,
        VisibilityLevel visibility = VisibilityLevel.Public,
        bool hasSpecialName = false,
        Type[]? resultAttributes = null)
    {
        parameters ??= [];
        var attributes = visibility.ToMethodAttributes() | MethodAttributes.Static | MethodAttributes.HideBySig;
        if (hasSpecialName)
            attributes |= MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
        var builder = MethodBuilderFacade.CreateMethodBuilder(
            context.Builder, name, attributes,
            parameters, result, resultAttributes ?? Type.EmptyTypes);
        var code = builder.GetILGenerator();
        return new DynamicFunction<MethodBuilder, MethodInfo, Action<ISymbol>>(
            builder,
            MethodBuilderFacade.CreateSearchMethodDelegate(builder),
            builder.SetCustomAttribute,
            MethodBuilderFacade.CreateReturnResultDelegate<ISymbol>(code, result))
        {
            Context = context,
            Code = code,
            ParameterTypes = parameters.SelectTypes().ToArray(),
            ReturnType = result,
        };
    }

    public DynamicFunction<MethodBuilder, MethodInfo, Action<ISymbol<TResult>>> DefineFunctor<TResult>(
        string name, ParameterDefinition[]? parameters = null,
        ContentModifier? modifier = null,
        VisibilityLevel visibility = VisibilityLevel.Public,
        bool hasSpecialName = false,
        Type[]? resultAttributes = null)
    {
        parameters ??= [];
        var resultType = modifier.Decorate<TResult>();
        var attributes = visibility.ToMethodAttributes() | MethodAttributes.Static | MethodAttributes.HideBySig;
        if (hasSpecialName)
            attributes |= MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
        var builder = MethodBuilderFacade.CreateMethodBuilder(
            context.Builder, name, attributes,
            parameters, resultType, resultAttributes ?? Type.EmptyTypes);
        var code = builder.GetILGenerator();
        return new DynamicFunction<MethodBuilder, MethodInfo, Action<ISymbol<TResult>>>(
            builder,
            MethodBuilderFacade.CreateSearchMethodDelegate(builder),
            builder.SetCustomAttribute,
            MethodBuilderFacade.CreateReturnResultDelegate<ISymbol<TResult>>(code, resultType))
        {
            Context = context,
            Code = code,
            ParameterTypes = parameters.SelectTypes().ToArray(),
            ReturnType = resultType,
        };
    }
}