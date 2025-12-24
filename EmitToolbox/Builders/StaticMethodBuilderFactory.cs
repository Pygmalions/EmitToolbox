using EmitToolbox.Symbols;
using JetBrains.Annotations;

namespace EmitToolbox.Builders;

public class StaticMethodBuilderFactory(DynamicType context)
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
        var builder = MethodBuilderFactory.CreateMethodBuilder(
            context.Builder, name, attributes,
            parameters, typeof(void), Type.EmptyTypes);
        var code = builder.GetILGenerator();
        return new DynamicMethod<Action>(
            builder, MethodBuilderFactory.CreateReturnResultDelegate(code))
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
        var builder = MethodBuilderFactory.CreateMethodBuilder(
            context.Builder, name, attributes,
            parameters, result, resultAttributes ?? Type.EmptyTypes);
        var code = builder.GetILGenerator();
        return new DynamicMethod<Action<ISymbol>>(
            builder, MethodBuilderFactory.CreateReturnResultDelegate<ISymbol>(code, result))
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
        var builder = MethodBuilderFactory.CreateMethodBuilder(
            context.Builder, name, attributes,
            parameters, resultType, resultAttributes ?? Type.EmptyTypes);
        var code = builder.GetILGenerator();
        return new DynamicMethod<Action<ISymbol<TResult>>>(
            builder, MethodBuilderFactory.CreateReturnResultDelegate<ISymbol<TResult>>(code, resultType))
        {
            DeclaringType = context,
            Code = code,
            ParameterTypes = parameters.SelectTypes().ToArray(),
            ReturnType = resultType,
        };
    }
}