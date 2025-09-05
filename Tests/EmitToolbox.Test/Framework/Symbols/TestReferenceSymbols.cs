using EmitToolbox.Framework;

namespace EmitToolbox.Test.Framework.Symbols;

[TestFixture]
public class TestReferenceSymbols
{
    private AssemblyBuildingContext _assembly;

    [SetUp]
    public void Initialize()
    {
        _assembly = AssemblyBuildingContext
            .CreateExecutableContextBuilder("TestReferenceSymbols")
            .Build();
    }

    private delegate int IntRefLoader(ref int value);
    
    [Test]
    public void ParameterReference_LoadValue()
    {
        var typeContext = _assembly.DefineClass("ParameterReference_LoadValue");
        var methodContext = typeContext.Functors.Static("Test", 
            [ParameterDefinition.Reference<int>()],
            ResultDefinition.Value<int>());
        var argumentReference = 
            methodContext.Argument<int>(0, true);
        methodContext.Return(argumentReference);
        typeContext.Build();
        var value = TestContext.CurrentContext.Random.Next();
        var method = methodContext.BuildingMethod.CreateDelegate<IntRefLoader>();
        Assert.That(method(ref value), Is.EqualTo(value));
    }
    
    private delegate void IntRefAssigner(ref int target, int value);
    
    [Test]
    public void ParameterReference_StoreValue()
    {
        var typeContext = _assembly.DefineClass("ParameterReference_StoreValue");
        var methodContext = typeContext.Actions.Static("Test", 
            [ParameterDefinition.Reference<int>(), ParameterDefinition.Value<int>()]);
        var argumentReference = 
            methodContext.Argument<int>(0, true);
        var argumentValue = 
            methodContext.Argument<int>(1);
        argumentReference.Assign(argumentValue);
        methodContext.Return();
        typeContext.Build();
        var value = TestContext.CurrentContext.Random.Next();
        var newValue = TestContext.CurrentContext.Random.Next();
        var method = methodContext.BuildingMethod.CreateDelegate<IntRefAssigner>();
        method(ref value, newValue);
        Assert.That(value, Is.EqualTo(newValue));
    }
}