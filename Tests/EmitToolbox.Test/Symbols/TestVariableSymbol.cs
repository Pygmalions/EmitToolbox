using EmitToolbox.Symbols;

namespace EmitToolbox.Test.Symbols;

[TestFixture,
 TestOf(typeof(VariableSymbol)),
 TestOf(typeof(VariableSymbol<>))]
public class TestVariableSymbol
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }
    
    private Func<TResult, TResult> CreateTestMethod<TResult>(bool byReference = false)
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<TResult>(
            "Execute", [ParameterDefinition.Value<TResult>()]);
        var argument = method.Argument<TResult>(0);
        var variable = method.Variable<TResult>(byReference ? ContentModifier.Reference : null);
        variable.AssignContent(argument);
        method.Return(variable);
        type.Build();
        return method.BuildingMethod.CreateDelegate<Func<TResult, TResult>>();
    }
    
    [Test]
    public void Value_GetAndSet()
    {
        var method = CreateTestMethod<int>();
        var value = TestContext.CurrentContext.Random.Next();
        var result = method(value);
        Assert.That(result, Is.EqualTo(value));
    }
    
    [Test]
    public void Object_GetAndSet()
    {
        var method = CreateTestMethod<string>();
        var value = TestContext.CurrentContext.Random.GetString(10);
        var result = method(value);
        Assert.That(result, Is.EqualTo(value));
    }
    
    [Test]
    public void Value_ByRef_GetAndSet()
    {
        var method = CreateTestMethod<int>(true);
        var value = TestContext.CurrentContext.Random.Next();
        var result = method(value);
        Assert.That(result, Is.EqualTo(value));
    }
    
    [Test]
    public void Object_ByRef_GetAndSet()
    {
        var method = CreateTestMethod<string>(true);
        var value = TestContext.CurrentContext.Random.GetString(10);
        var result = method(value);
        Assert.That(result, Is.EqualTo(value));
    }
}