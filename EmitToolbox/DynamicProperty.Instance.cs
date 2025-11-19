using EmitToolbox.Builders;
using EmitToolbox.Symbols;
using JetBrains.Annotations;

namespace EmitToolbox;

public class InstanceDynamicProperty<TProperty>(DynamicType context, PropertyBuilder builder)
    : DynamicProperty(context, builder)
{
    [MustUseReturnValue("Returned getter is initially empty and its function body needs to be defined.")]
    DynamicMethod<Action<ISymbol<TProperty>>> DefineGetter(
        string? name = null,
        VisibilityLevel visibility = VisibilityLevel.Public,
        MethodModifier methodModifier = MethodModifier.None)
    {
        var builder = MethodBuilderFacade.CreateMethodBuilder(Context.Builder, name ?? $"get_{Builder.Name}",
            visibility.ToMethodAttributes() | methodModifier.ToMethodAttributes(true),
            [], Builder.PropertyType, Type.EmptyTypes);
        var code = builder.GetILGenerator();
        var method = new DynamicMethod<Action<ISymbol<TProperty>>>(
            builder,
            MethodBuilderFacade.CreateReturnResultDelegate<ISymbol<TProperty>>(code, Builder.PropertyType))
        {
            DeclaringType = Context,
            Code = code,
            ParameterTypes = Type.EmptyTypes,
            ReturnType = Builder.PropertyType,
        };
        BindGetter(method);
        return method;
    }

    [MustUseReturnValue("Returned setter is initially empty and its function body needs to be defined.")]
    DynamicMethod<Action> DefineSetter(
        string? name = null,
        VisibilityLevel visibility = VisibilityLevel.Public,
        MethodModifier methodModifier = MethodModifier.None)
    {
        var builder = MethodBuilderFacade.CreateMethodBuilder(Context.Builder, name ?? $"set_{Builder.Name}",
            visibility.ToMethodAttributes() | methodModifier.ToMethodAttributes(true),
            [new ParameterDefinition(Builder.PropertyType)], typeof(void), Type.EmptyTypes);
        var code = builder.GetILGenerator();
        var methodContext = new DynamicMethod<Action>(
            builder,
            MethodBuilderFacade.CreateReturnResultDelegate(code))
        {
            DeclaringType = Context,
            Code = code,
            ParameterTypes = [Builder.PropertyType],
            ReturnType = typeof(void),
        };
        BindSetter(methodContext);
        return methodContext;
    }
}