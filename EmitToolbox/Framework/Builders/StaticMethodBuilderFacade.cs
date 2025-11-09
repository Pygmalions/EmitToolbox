using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework.Builders;

public class StaticMethodBuilderFacade(DynamicType context)
{
    public DynamicMethod<MethodBuilder, MethodInfo, Action> DefineAction(
        string name, ParameterDefinition[] parameters,
        VisibilityLevel visibility = VisibilityLevel.Public,
        bool hasSpecialName = false)
    {
        var attributes = visibility.ToMethodAttributes() | MethodAttributes.Static | MethodAttributes.HideBySig;
        if (hasSpecialName)
            attributes |= MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
        var builder = MethodBuilderFacade.CreateMethodBuilder(
            context.Builder, name, attributes,
            parameters, typeof(void), Type.EmptyTypes);
        var code = builder.GetILGenerator();
        return new DynamicMethod<MethodBuilder, MethodInfo, Action>(
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

    public DynamicMethod<MethodBuilder, MethodInfo, Action<ISymbol>> DefineFunctor(
        string name, Type result, ParameterDefinition[] parameters,
        VisibilityLevel visibility = VisibilityLevel.Public,
        bool hasSpecialName = false,
        Type[]? resultAttributes = null)
    {
        var attributes = visibility.ToMethodAttributes() | MethodAttributes.Static | MethodAttributes.HideBySig;
        if (hasSpecialName)
            attributes |= MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
        var builder = MethodBuilderFacade.CreateMethodBuilder(
            context.Builder, name, attributes,
            parameters, result, resultAttributes ?? Type.EmptyTypes);
        var code = builder.GetILGenerator();
        return new DynamicMethod<MethodBuilder, MethodInfo, Action<ISymbol>>(
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

    public DynamicMethod<MethodBuilder, MethodInfo, Action<ISymbol<TResult>>> DefineFunctor<TResult>(
        string name, ParameterDefinition[] parameters,
        ContentModifier? resultModifier = null,
        VisibilityLevel visibility = VisibilityLevel.Public,
        bool hasSpecialName = false,
        Type[]? resultAttributes = null)
    {
        var resultType = resultModifier.Decorate<TResult>();
        var attributes = visibility.ToMethodAttributes() | MethodAttributes.Static | MethodAttributes.HideBySig;
        if (hasSpecialName)
            attributes |= MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
        var builder = MethodBuilderFacade.CreateMethodBuilder(
            context.Builder, name, attributes,
            parameters, resultType, resultAttributes ?? Type.EmptyTypes);
        var code = builder.GetILGenerator();
        return new DynamicMethod<MethodBuilder, MethodInfo, Action<ISymbol<TResult>>>(
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