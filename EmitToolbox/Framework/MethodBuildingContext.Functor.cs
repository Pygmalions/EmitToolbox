using System.Diagnostics.CodeAnalysis;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework;

public abstract class FunctorBuildingContext(TypeBuildingContext typeContext, MethodBuilder methodBuilder)
    : MethodBuildingContext(typeContext, methodBuilder.GetILGenerator())
{
    public MethodBuilder MethodBuilder => methodBuilder;
    
    [field: MaybeNull]
    public MethodInfo BuildingMethod
    {
        get
        {
            if (TypeContext.IsBuilt)
                field ??= TypeContext.BuildingType.GetMethod(
                              methodBuilder.Name,
                              BindingFlags.Public | BindingFlags.NonPublic |
                              (methodBuilder.IsStatic ? BindingFlags.Static : BindingFlags.Instance),
                              methodBuilder.GetParameters()
                                  .Select(parameter => parameter.ParameterType).ToArray())
                          ?? throw new InvalidOperationException("Failed to retrieve the built method.");
            return field ?? methodBuilder;
        }
    }
    
    public override void MarkAttribute(CustomAttributeBuilder attributeBuilder)
    {
        methodBuilder.SetCustomAttribute(attributeBuilder);
    }

    public void Return(ValueSymbol result)
    {
        if (!BuildingMethod.ReturnType.IsByRef)
            result.EmitLoadAsValue();
        else
            result.EmitLoadAsAddress();
        Code.Emit(OpCodes.Ret);
    }
}

public class InstanceFunctorBuildingContext(TypeBuildingContext typeContext, MethodBuilder methodBuilder)
    : FunctorBuildingContext(typeContext, methodBuilder)
{
    [field: MaybeNull]
    public ThisSymbol This =>
        field ??= new ThisSymbol(this, TypeContext.BuildingType);

    public override bool IsStatic => false;

    public override ArgumentSymbol<TValue> Argument<TValue>(int index, bool isReference = false)
        => new(this, index + 1, isReference);
}

public class StaticFunctorBuildingContext(TypeBuildingContext typeContext, MethodBuilder methodBuilder)
    : FunctorBuildingContext(typeContext, methodBuilder)
{
    public override bool IsStatic => true;

    public override ArgumentSymbol<TValue> Argument<TValue>(int index, bool isReference = false)
        => new(this, index, isReference);
}