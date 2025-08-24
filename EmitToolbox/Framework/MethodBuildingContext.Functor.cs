using System.Diagnostics.CodeAnalysis;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework;

public class FunctorMethodBuildingContext(TypeBuildingContext typeContext, MethodBuilder methodBuilder)
    : MethodBuildingContext(typeContext, methodBuilder.GetILGenerator())
{
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

    public override bool IsStatic { get; } = methodBuilder.IsStatic;

    internal MethodBuilder MethodBuilder => methodBuilder;
    
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