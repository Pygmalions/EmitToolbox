using EmitToolbox.Framework.Symbols;
using JetBrains.Annotations;

namespace EmitToolbox.Framework.Builders;

public class StaticMethodBuilderFacade(DynamicType context)
{
    [MustUseReturnValue]
    public DynamicMethod<Action> DefineAction(
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
        return new DynamicMethod<Action>(
            builder, MethodBuilderFacade.CreateReturnResultDelegate(code))
        {
            DeclaringType = context,
            Code = code,
            ParameterTypes = parameters.SelectTypes().ToArray(),
            ReturnType = typeof(void),
        };
    }

    [MustUseReturnValue]
    public DynamicMethod<Action<ISymbol>> DefineFunctor(
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
        return new DynamicMethod<Action<ISymbol>>(
            builder, MethodBuilderFacade.CreateReturnResultDelegate<ISymbol>(code, result))
        {
            DeclaringType = context,
            Code = code,
            ParameterTypes = parameters.SelectTypes().ToArray(),
            ReturnType = result,
        };
    }

    [MustUseReturnValue]
    public DynamicMethod<Action<ISymbol<TResult>>> DefineFunctor<TResult>(
        string name, ParameterDefinition[]? parameters = null,
        ContentModifier? resultModifier = null,
        VisibilityLevel visibility = VisibilityLevel.Public,
        bool hasSpecialName = false,
        Type[]? resultAttributes = null)
    {
        parameters ??= [];
        var resultType = resultModifier.Decorate<TResult>();
        var attributes = visibility.ToMethodAttributes() | MethodAttributes.Static | MethodAttributes.HideBySig;
        if (hasSpecialName)
            attributes |= MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
        var builder = MethodBuilderFacade.CreateMethodBuilder(
            context.Builder, name, attributes,
            parameters, resultType, resultAttributes ?? Type.EmptyTypes);
        var code = builder.GetILGenerator();
        return new DynamicMethod<Action<ISymbol<TResult>>>(
            builder, MethodBuilderFacade.CreateReturnResultDelegate<ISymbol<TResult>>(code, resultType))
        {
            DeclaringType = context,
            Code = code,
            ParameterTypes = parameters.SelectTypes().ToArray(),
            ReturnType = resultType,
        };
    }
}