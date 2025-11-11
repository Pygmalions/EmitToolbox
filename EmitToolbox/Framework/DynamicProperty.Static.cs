using EmitToolbox.Framework.Builders;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework;

public class StaticDynamicProperty<TProperty>(DynamicType context, PropertyBuilder builder)
    : DynamicProperty(context, builder)
{
    DynamicFunction<MethodBuilder, MethodInfo, Action<ISymbol<TProperty>>> DefineGetter(
        string? name = null, VisibilityLevel visibility = VisibilityLevel.Public)
    {
        var builder = MethodBuilderFacade.CreateMethodBuilder(
            Context.Builder, name ?? $"get_{Builder.Name}",
            visibility.ToMethodAttributes() | MethodAttributes.Static | MethodAttributes.HideBySig,
            [], Builder.PropertyType, Type.EmptyTypes);
        var code = builder.GetILGenerator();
        var methodContext = new DynamicFunction<
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

    DynamicFunction<MethodBuilder, MethodInfo, Action> DefineSetter(
        string? name = null, VisibilityLevel visibility = VisibilityLevel.Public)
    {
        var builder = MethodBuilderFacade.CreateMethodBuilder(Context.Builder, name ?? $"set_{Builder.Name}",
            visibility.ToMethodAttributes() | MethodAttributes.Static | MethodAttributes.HideBySig,
            [new ParameterDefinition(Builder.PropertyType)], typeof(void), Type.EmptyTypes);
        var code = builder.GetILGenerator();
        var methodContext = new DynamicFunction<MethodBuilder, MethodInfo, Action>(
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