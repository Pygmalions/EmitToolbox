using EmitToolbox.Builders;

namespace EmitToolbox;

public class MethodFactory
{
    internal MethodFactory(DynamicType context)
    {
        Instance = new InstanceMethodBuilderFacade(context);
        Static = new StaticMethodBuilderFacade(context);
        Constructor = new ConstructorBuilderFacade(context);
    }
    
    public InstanceMethodBuilderFacade Instance { get; }

    public StaticMethodBuilderFacade Static { get; }

    public ConstructorBuilderFacade Constructor { get; }
}