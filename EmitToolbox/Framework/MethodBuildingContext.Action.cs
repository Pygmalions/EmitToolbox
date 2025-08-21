namespace EmitToolbox.Framework;

public class ActionMethodBuildingContext(MethodBuilder methodBuilder)
    : MethodBuildingContext(methodBuilder.GetILGenerator())
{
    public MethodInfo BuildingMethod { get; } = methodBuilder;

    public override void MarkAttribute(CustomAttributeBuilder attributeBuilder)
    {
        methodBuilder.SetCustomAttribute(attributeBuilder);
    }

    public void Return()
    {
        Code.Emit(OpCodes.Ret);
    }
}