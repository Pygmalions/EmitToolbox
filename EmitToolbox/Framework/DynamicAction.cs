using System.Diagnostics.CodeAnalysis;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework;

public abstract class DynamicAction(DynamicType typeContext, MethodBuilder methodBuilder)
    : DynamicMethod(typeContext, methodBuilder.GetILGenerator())
{
    public MethodBuilder MethodBuilder => methodBuilder;

    [field: MaybeNull]
    public MethodInfo BuildingMethod
    {
        get
        {
            if (field != null)
                return field;
            if (TypeContext.IsBuilt)
            {
                return field = TypeContext.BuildingType.GetMethod(
                                  MethodBuilder.Name,
                                  BindingFlags.Public | BindingFlags.NonPublic |
                                  BindingFlags.Instance | BindingFlags.Static,
                                  MethodBuilder.GetParameters()
                                      .Select(parameter => parameter.ParameterType).ToArray())
                              ?? throw new InvalidOperationException("Failed to retrieve the built method.");
            }

            return MethodBuilder;
        }
    }

    public sealed override void MarkAttribute(CustomAttributeBuilder attributeBuilder)
    {
        MethodBuilder.SetCustomAttribute(attributeBuilder);
    }

    public void Return()
    {
        Code.Emit(OpCodes.Ret);
    }
}

public class InstanceDynamicAction(DynamicType typeContext, MethodBuilder methodBuilder)
    : DynamicAction(typeContext, methodBuilder)
{
    [field: MaybeNull]
    public ArgumentSymbol This =>
        field ??= new ArgumentSymbol(this, 0, TypeContext.BuildingType);

    public override bool IsStatic => false;

    public override ArgumentSymbol Argument(int index, Type type)
        => new(this, index + 1, type);

    public override ArgumentSymbol<TValue> Argument<TValue>(int index, ValueModifier modifier = ValueModifier.None)
        => new(this, index + 1, modifier);
}

public class StaticDynamicAction(DynamicType typeContext, MethodBuilder methodBuilder)
    : DynamicAction(typeContext, methodBuilder)
{
    public override bool IsStatic => true;

    public override ArgumentSymbol Argument(int index, Type type)
        => new(this, index, type);

    public override ArgumentSymbol<TValue> Argument<TValue>(int index, ValueModifier modifier = ValueModifier.None)
        => new(this, index, modifier);
}