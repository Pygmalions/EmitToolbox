using EmitToolbox.Framework;
using EmitToolbox.Framework.Symbols.Extensions;

namespace EmitToolbox.Test.Framework.Extensions;

[TestFixture]
public class TestObjectExtension
{
    private static AssemblyBuildingContext _assembly;

    [SetUp]
    public void Initialize()
    {
        _assembly = AssemblyBuildingContext
            .CreateExecutableContextBuilder("TestObjectExtension")
            .Build();
    }
    
    [Test]
    public void TestObjectExtension_Box()
    {
        var typeContext = _assembly.DefineClass("TestObjectExtension_Box");
        var methodContext = typeContext.Functors.Static("Test", 
            [ParameterDefinition.Value<int>()], ResultDefinition.Value<object>());
        var argument = methodContext.Argument<int>(0);
        methodContext.Return(argument.Box());
        typeContext.Build();
        var value = TestContext.CurrentContext.Random.Next();
        Assert.That(methodContext.BuildingMethod.Invoke(null, [value]),
            Is.EqualTo(value));
    }
    
    [Test]
    public void TestObjectExtension_Unbox()
    {
        var typeContext = _assembly.DefineClass("TestObjectExtension_Unbox");
        var methodContext = typeContext.Functors.Static("Test", 
            [ParameterDefinition.Value<object>()], ResultDefinition.Value<int>());
        var argument = methodContext.Argument<object>(0);
        methodContext.Return(argument.Unbox<int>());
        typeContext.Build();
        object value = TestContext.CurrentContext.Random.Next();
        Assert.That(methodContext.BuildingMethod.Invoke(null, [value]), 
            Is.EqualTo(value));
    }
}