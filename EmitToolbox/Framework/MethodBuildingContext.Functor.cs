namespace EmitToolbox.Framework;

public class FunctorMethodBuildingContext(MethodBuilder methodBuilder)
    : MethodBuildingContext(methodBuilder.GetILGenerator())
{
    public MethodInfo BuildingMethod { get; } = methodBuilder;
    
    public void Return()
    {
        throw new NotImplementedException();
    }
}