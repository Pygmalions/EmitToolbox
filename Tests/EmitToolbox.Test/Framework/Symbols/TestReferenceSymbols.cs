using EmitToolbox.Framework;
using EmitToolbox.Framework.Symbols.Extensions;

namespace EmitToolbox.Test.Framework.Symbols;

[TestFixture]
public class TestReferenceSymbols
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Initialize()
    {
        _assembly = DynamicAssembly.DefineExecutable("TestReferenceSymbols");
    }

    private delegate int IntRefLoader(ref int value);
    
    [Test]
    public void ParameterReference_LoadValue()
    {
        var typeContext = _assembly.DefineClass("ParameterReference_LoadValue");
        var methodContext = typeContext.FunctorBuilder.DefineStatic("Test", 
            [ParameterDefinition.Reference<int>()], ResultDefinition.Value<int>());
        var argumentReference = 
            methodContext.Argument<int>(0, ValueModifier.Reference);
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
        var methodContext = typeContext.ActionBuilder.DefineStatic("Test", 
            [ParameterDefinition.Reference<int>(), ParameterDefinition.Value<int>()]);
        var argumentReference = 
            methodContext.Argument<int>(0, ValueModifier.Reference);
        var argumentValue = 
            methodContext.Argument<int>(1);
        argumentReference.AssignFrom(argumentValue);
        methodContext.Return();
        typeContext.Build();
        var value = TestContext.CurrentContext.Random.Next();
        var newValue = TestContext.CurrentContext.Random.Next();
        var method = methodContext.BuildingMethod.CreateDelegate<IntRefAssigner>();
        method(ref value, newValue);
        Assert.That(value, Is.EqualTo(newValue));
    }
}