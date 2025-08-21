using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework;

public class FunctorMethodBuildingContext(MethodBuilder methodBuilder)
    : MethodBuildingContext(methodBuilder.GetILGenerator())
{
    public MethodInfo BuildingMethod { get; } = methodBuilder;

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