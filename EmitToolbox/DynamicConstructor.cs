using EmitToolbox.Extensions;
using EmitToolbox.Symbols;

namespace EmitToolbox;

public class DynamicConstructor(ConstructorBuilder builder) : DynamicFunction(builder)
{
    public ConstructorBuilder Builder { get; } = builder;

    public new ConstructorInfo BuildingMethod => (ConstructorInfo)base.BuildingMethod;

    public void Return()
    {
        Code.Emit(OpCodes.Ret);
    }

    /// <summary>
    /// Try to search for an accessible constructor of the base type and invoke it.
    /// </summary>
    public void InvokeBaseTypeConstructor(params ISymbol[] parameters)
    {
        var baseType = DeclaringType.Builder.BaseType;
        if (baseType is null)
            throw new InvalidOperationException(
                "Cannot invoke the base type constructor: this type does not have a base type.");
        var baseConstructor = baseType.GetConstructor(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            parameters.Select(symbol => symbol.BasicType).ToArray());
        if (baseConstructor is null)
            throw new Exception(
                "No base constructor was found that matches the types of the provided symbols.");
        this.This().Invoke(baseConstructor, parameters);
    }

    public override DynamicFunction MarkAttribute(CustomAttributeBuilder attributeBuilder)
    {
        Builder.SetCustomAttribute(attributeBuilder);
        return this;
    }

    protected override MethodBase SearchBuiltMethod(Type type)
    {
        var flags = (Builder.IsStatic ? BindingFlags.Static : BindingFlags.Instance) |
                    (Builder.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic);
        return type.GetConstructor(flags,
            Builder.GetParameters().Select(parameter => parameter.ParameterType).ToArray())!;
    }
}