using System.Diagnostics.CodeAnalysis;

namespace EmitToolbox.Framework;

public class ActionMethodBuildingContext(TypeBuildingContext typeContext, MethodBuilder methodBuilder)
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
                    methodBuilder.GetParameters()
                        .Select(parameter => parameter.ParameterType).ToArray())!;
            return field ?? methodBuilder;
        }
    }

    public override void MarkAttribute(CustomAttributeBuilder attributeBuilder)
    {
        methodBuilder.SetCustomAttribute(attributeBuilder);
    }

    public void Return()
    {
        Code.Emit(OpCodes.Ret);
    }
}