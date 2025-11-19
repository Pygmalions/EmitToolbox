using System.Linq.Expressions;
using EmitToolbox.Symbols;
using EmitToolbox.Utilities;
using JetBrains.Annotations;

namespace EmitToolbox.Builders;

public class InstanceMethodBuilderFacade(DynamicType context)
{
    [MustUseReturnValue]
    public DynamicMethod<Action> DefineAction(
        string name, ParameterDefinition[]? parameters = null,
        VisibilityLevel visibility = VisibilityLevel.Public,
        MethodModifier methodModifier = MethodModifier.None,
        bool hasSpecialName = false)
    {
        parameters ??= [];
        var builder = MethodBuilderFacade.CreateMethodBuilder(context.Builder, name,
            visibility.ToMethodAttributes() | methodModifier.ToMethodAttributes(hasSpecialName),
            parameters, typeof(void), Type.EmptyTypes);
        var code = builder.GetILGenerator();
        return new DynamicMethod<Action>(builder, MethodBuilderFacade.CreateReturnResultDelegate(code))
        {
            DeclaringType = context,
            Code = code,
            ParameterTypes = parameters.SelectTypes().ToArray(),
            ReturnType = typeof(void),
        };
    }

    [MustUseReturnValue]
    public DynamicMethod<Action> OverrideAction(
        MethodInfo method, string? name = null)
    {
        var builder = MethodBuilderFacade.CreateMethodBuilder(context.Builder, name ?? method.Name, method);
        context.Builder.DefineMethodOverride(builder, method);
        var code = builder.GetILGenerator();
        return new DynamicMethod<Action>(builder, MethodBuilderFacade.CreateReturnResultDelegate(code))
        {
            DeclaringType = context,
            Code = code,
            ParameterTypes = Enumerable.ToArray<Type>(method.GetParameterTypes()),
            ReturnType = typeof(void),
        };
    }

    [MustUseReturnValue]
    public DynamicMethod<Action> OverrideAction<TBase>(
        Expression<Action<TBase>> selector, string? name = null)
    {
        if (selector.Body is not MethodCallExpression expression)
            throw new ArgumentException("Expression is not a method call.", nameof(selector));
        if (!expression.Method.IsVirtual || expression.Method.IsFinal)
            throw new ArgumentException("Method to override is final or not virtual.", nameof(selector));
        return OverrideAction(expression.Method, name);
    }

    [MustUseReturnValue]
    public DynamicMethod<Action<ISymbol>> DefineFunctor(
        string name, Type result, ParameterDefinition[]? parameters = null,
        VisibilityLevel visibility = VisibilityLevel.Public,
        MethodModifier methodModifier = MethodModifier.None,
        bool hasSpecialName = false,
        Type[]? resultAttributes = null)
    {
        parameters ??= [];
        var builder = MethodBuilderFacade.CreateMethodBuilder(context.Builder, name,
            visibility.ToMethodAttributes() | methodModifier.ToMethodAttributes(hasSpecialName),
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
    public DynamicMethod<Action<ISymbol>> OverrideFunctor(MethodInfo method, string? name = null)
    {
        var builder = MethodBuilderFacade.CreateMethodBuilder(
            context.Builder, name ?? method.Name, method);
        context.Builder.DefineMethodOverride(builder, method);
        var code = builder.GetILGenerator();
        return new DynamicMethod<Action<ISymbol>>(
            builder, MethodBuilderFacade.CreateReturnResultDelegate<ISymbol>(code, method.ReturnType))
        {
            DeclaringType = context,
            Code = code,
            ParameterTypes = Enumerable.ToArray<Type>(method.GetParameterTypes()),
            ReturnType = method.ReturnType,
        };
    }

    [MustUseReturnValue]
    public DynamicMethod<Action<ISymbol<TResult>>> DefineFunctor<TResult>(
        string name, ParameterDefinition[]? parameters = null,
        ContentModifier? resultModifier = null,
        VisibilityLevel visibility = VisibilityLevel.Public,
        MethodModifier methodModifier = MethodModifier.None,
        bool hasSpecialName = false,
        Type[]? resultAttributes = null)
    {
        parameters ??= [];
        var resultType = resultModifier.Decorate<TResult>();
        var builder = MethodBuilderFacade.CreateMethodBuilder(context.Builder, name,
            visibility.ToMethodAttributes() | methodModifier.ToMethodAttributes(hasSpecialName),
            parameters, resultType,
            resultAttributes ?? Type.EmptyTypes);
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

    [MustUseReturnValue]
    public DynamicMethod<Action<ISymbol<TResult>>> OverrideFunctor<TResult>(MethodInfo method, string? name = null)
    {
        if (!typeof(TResult).IsAssignableTo(method.ReturnType.BasicType))
            throw new InvalidOperationException(
                "Declared return type is not assignable to the return type of the overridden method.");
        var builder = MethodBuilderFacade.CreateMethodBuilder(
            context.Builder, name ?? method.Name, method);
        context.Builder.DefineMethodOverride(builder, method);
        var code = builder.GetILGenerator();
        return new DynamicMethod<Action<ISymbol<TResult>>>(
            builder, MethodBuilderFacade.CreateReturnResultDelegate<ISymbol<TResult>>(code, method.ReturnType))
        {
            DeclaringType = context,
            Code = code,
            ParameterTypes = Enumerable.ToArray<Type>(method.GetParameterTypes()),
            ReturnType = method.ReturnType
        };
    }

    [MustUseReturnValue]
    public DynamicMethod<Action<ISymbol<TResult>>> OverrideFunctor<TBase, TResult>(
        Expression<Func<TBase, TResult>> selector, string? name = null)
    {
        if (selector.Body is not MethodCallExpression expression)
            throw new ArgumentException("Expression is not a method call.", nameof(selector));
        if (!expression.Method.IsVirtual || expression.Method.IsFinal)
            throw new ArgumentException("Method to override is final or not virtual.", nameof(selector));
        return OverrideFunctor<TResult>(expression.Method, name);
    }
}