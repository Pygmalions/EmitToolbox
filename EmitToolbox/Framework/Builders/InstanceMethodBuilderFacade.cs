using System.Linq.Expressions;
using EmitToolbox.Framework.Symbols;
using EmitToolbox.Framework.Utilities;

namespace EmitToolbox.Framework.Builders;

public class InstanceMethodBuilderFacade(DynamicType context)
{
    public DynamicMethod<MethodBuilder, MethodInfo, Action> DefineAction(
        string name, ParameterDefinition[] parameters,
        VisibilityLevel visibility = VisibilityLevel.Public,
        InstanceMethodModifier methodModifier = InstanceMethodModifier.None,
        bool hasSpecialName = false)
    {
        var builder = MethodBuilderFacade.CreateMethodBuilder(context.Builder, name,
            visibility.ToMethodAttributes() | methodModifier.ToMethodAttributes(hasSpecialName),
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

    public DynamicMethod<MethodBuilder, MethodInfo, Action> OverrideAction(
        MethodInfo method, string? name = null)
    {
        var builder = MethodBuilderFacade.CreateMethodBuilder(context.Builder, name ?? method.Name, method);
        context.Builder.DefineMethodOverride(builder, method);
        var code = builder.GetILGenerator();
        return new DynamicMethod<MethodBuilder, MethodInfo, Action>(
            builder,
            MethodBuilderFacade.CreateSearchMethodDelegate(builder),
            builder.SetCustomAttribute,
            MethodBuilderFacade.CreateReturnResultDelegate(code))
        {
            Context = context,
            Code = code,
            ParameterTypes = method.GetParameterTypes().ToArray(),
            ReturnType = typeof(void),
        };
    }

    public DynamicMethod<MethodBuilder, MethodInfo, Action> OverrideAction<TBase>(
        Expression<Action<TBase>> selector, string? name = null)
    {
        if (selector.Body is not MethodCallExpression expression)
            throw new ArgumentException("Expression is not a method call.", nameof(selector));
        if (!expression.Method.IsVirtual || expression.Method.IsFinal)
            throw new ArgumentException("Method to override is final or not virtual.", nameof(selector));
        return OverrideAction(expression.Method, name);
    }

    public DynamicMethod<MethodBuilder, MethodInfo, Action<ISymbol>> DefineFunctor(
        string name, Type result, ParameterDefinition[] parameters,
        VisibilityLevel visibility = VisibilityLevel.Public,
        InstanceMethodModifier methodModifier = InstanceMethodModifier.None,
        bool hasSpecialName = false,
        Type[]? resultAttributes = null)
    {
        var builder = MethodBuilderFacade.CreateMethodBuilder(context.Builder, name,
            visibility.ToMethodAttributes() | methodModifier.ToMethodAttributes(hasSpecialName),
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

    public DynamicMethod<MethodBuilder, MethodInfo, Action<ISymbol>> OverrideFunctor(
        MethodInfo method, string? name = null)
    {
        var builder = MethodBuilderFacade.CreateMethodBuilder(
            context.Builder, name ?? method.Name, method);
        context.Builder.DefineMethodOverride(builder, method);
        var code = builder.GetILGenerator();
        return new DynamicMethod<MethodBuilder, MethodInfo, Action<ISymbol>>(
            builder,
            MethodBuilderFacade.CreateSearchMethodDelegate(builder),
            builder.SetCustomAttribute,
            MethodBuilderFacade.CreateReturnResultDelegate<ISymbol>(code, method.ReturnType))
        {
            Context = context,
            Code = code,
            ParameterTypes = method.GetParameterTypes().ToArray(),
            ReturnType = method.ReturnType,
        };
    }

    public DynamicMethod<MethodBuilder, MethodInfo, Action<ISymbol<TResult>>> DefineFunctor<TResult>(
        string name, ParameterDefinition[] parameters,
        ContentModifier? resultModifier = null,
        VisibilityLevel visibility = VisibilityLevel.Public,
        InstanceMethodModifier methodModifier = InstanceMethodModifier.None,
        bool hasSpecialName = false,
        Type[]? resultAttributes = null)
    {
        var resultType = resultModifier.Decorate<TResult>();
        var builder = MethodBuilderFacade.CreateMethodBuilder(context.Builder, name,
            visibility.ToMethodAttributes() | methodModifier.ToMethodAttributes(hasSpecialName),
            parameters, resultType,
            resultAttributes ?? Type.EmptyTypes);
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

    public DynamicMethod<MethodBuilder, MethodInfo, Action<ISymbol<TResult>>> OverrideFunctor<TResult>(
        MethodInfo method, string? name = null)
    {
        if (!typeof(TResult).IsAssignableTo(method.ReturnType.BasicType))
            throw new InvalidOperationException(
                "Declared return type is not assignable to the return type of the overridden method.");
        var builder = MethodBuilderFacade.CreateMethodBuilder(
            context.Builder, name ?? method.Name, method);
        context.Builder.DefineMethodOverride(builder, method);
        var code = builder.GetILGenerator();
        return new DynamicMethod<MethodBuilder, MethodInfo, Action<ISymbol<TResult>>>(
            builder,
            MethodBuilderFacade.CreateSearchMethodDelegate(builder),
            builder.SetCustomAttribute,
            MethodBuilderFacade.CreateReturnResultDelegate<ISymbol<TResult>>(code, method.ReturnType))
        {
            Context = context,
            Code = code,
            ParameterTypes = method.GetParameterTypes().ToArray(),
            ReturnType = method.ReturnType
        };
    }
    
    public DynamicMethod<MethodBuilder, MethodInfo, Action<ISymbol<TResult>>> OverrideFunctor<TBase, TResult>(
        Expression<Func<TBase, TResult>> selector, string? name = null)
    {
        if (selector.Body is not MethodCallExpression expression)
            throw new ArgumentException("Expression is not a method call.", nameof(selector));
        if (!expression.Method.IsVirtual || expression.Method.IsFinal)
            throw new ArgumentException("Method to override is final or not virtual.", nameof(selector));
        return OverrideFunctor<TResult>(expression.Method, name);
    }
}