using EmitToolbox.Framework.Builders;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework;

public class InstanceDynamicProperty<TProperty>(DynamicType context, PropertyBuilder builder)
    : DynamicProperty(context, builder)
{
    DynamicMethod<MethodBuilder, MethodInfo, Action<ISymbol<TProperty>>>
        DefineGetter(
            string? name = null,
            VisibilityLevel visibility = VisibilityLevel.Public,
            InstanceMethodModifier methodModifier = InstanceMethodModifier.None)
    {
        var builder = MethodBuilderFacade.CreateMethodBuilder(Context.Builder, name ?? $"get_{Builder.Name}",
            visibility.ToMethodAttributes() | methodModifier.ToMethodAttributes(true),
            [], Builder.PropertyType, Type.EmptyTypes);
        var code = builder.GetILGenerator();
        var methodContext = new DynamicMethod<
            MethodBuilder, MethodInfo, Action<ISymbol<TProperty>>>(
            builder,
            MethodBuilderFacade.CreateSearchMethodDelegate(builder),
            builder.SetCustomAttribute,
            MethodBuilderFacade.CreateReturnResultDelegate<ISymbol<TProperty>>(code, Builder.PropertyType))
        {
            Context = Context,
            Code = code,
            ParameterTypes = Type.EmptyTypes,
            ReturnType = Builder.PropertyType,
        };
        BindGetter(methodContext);
        return methodContext;
    }

    DynamicMethod<MethodBuilder, MethodInfo, Action>
        DefineSetter(
            string? name = null,
            VisibilityLevel visibility = VisibilityLevel.Public,
            InstanceMethodModifier methodModifier = InstanceMethodModifier.None)
    {
        var builder = MethodBuilderFacade.CreateMethodBuilder(Context.Builder, name ?? $"set_{Builder.Name}",
            visibility.ToMethodAttributes() | methodModifier.ToMethodAttributes(true),
            [new ParameterDefinition(Builder.PropertyType)], typeof(void), Type.EmptyTypes);
        var code = builder.GetILGenerator();
        var methodContext = new DynamicMethod<MethodBuilder, MethodInfo, Action>(
            builder,
            MethodBuilderFacade.CreateSearchMethodDelegate(builder),
            builder.SetCustomAttribute,
            MethodBuilderFacade.CreateReturnResultDelegate(code))
        {
            Context = Context,
            Code = code,
            ParameterTypes = [Builder.PropertyType],
            ReturnType = typeof(void),
        };
        BindSetter(methodContext);
        return methodContext;
    }
}