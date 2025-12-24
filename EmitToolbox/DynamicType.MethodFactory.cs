using EmitToolbox.Builders;

namespace EmitToolbox;

public class MethodFactory
{
    internal MethodFactory(DynamicType context)
    {
        Instance = new InstanceMethodBuilderFactory(context);
        Static = new StaticMethodBuilderFactory(context);
        Constructor = new ConstructorBuilderFactory(context);
    }
    
    public InstanceMethodBuilderFactory Instance { get; }

    public StaticMethodBuilderFactory Static { get; }

    public ConstructorBuilderFactory Constructor { get; }
}