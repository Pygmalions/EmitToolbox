using EmitToolbox.Framework;
using EmitToolbox.Framework.Symbols.Extensions;

namespace EmitToolbox.Test.Framework.Extensions;

[TestFixture]
public class TestObjectExtensions
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Initialize()
    {
        _assembly = DynamicAssembly.DefineExecutable("TestObjectExtensions");
    }
    
    [Test]
    public void TestObjectExtension_Box()
    {
        var typeContext = _assembly.DefineClass("TestObjectExtensions_Box");
        var methodContext = typeContext.FunctorBuilder.DefineStatic("Test", 
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
        var typeContext = _assembly.DefineClass("TestObjectExtensions_Unbox");
        var methodContext = typeContext.FunctorBuilder.DefineStatic("Test", 
            [ParameterDefinition.Value<object>()], ResultDefinition.Value<int>());
        var argument = methodContext.Argument<object>(0);
        methodContext.Return(argument.Unbox<int>());
        typeContext.Build();
        object value = TestContext.CurrentContext.Random.Next();
        Assert.That(methodContext.BuildingMethod.Invoke(null, [value]), 
            Is.EqualTo(value));
    }
}