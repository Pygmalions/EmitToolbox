using EmitToolbox.Framework;
using EmitToolbox.Framework.Extensions;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Test.Framework.Extensions;

[TestFixture, TestOf(typeof(StringExtensions))]
public class TestStringExtensions
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }
    
    [Test]
    public void Concat()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<string>("Concat", 
            [typeof(string), typeof(string)]);
        var argumentA = method.Argument<string>(0);
        var argumentB = method.Argument<string>(1);
        method.Return(argumentA + argumentB);
        
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<string, string, string>>();
        var testStringA = TestContext.CurrentContext.Random.GetString(10);
        var testStringB = TestContext.CurrentContext.Random.GetString(10);
        Assert.That(functor(testStringA, testStringB), Is.EqualTo(testStringA + testStringB));
    }
    
    [Test]
    public void IsEqualTo()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<bool>("IsEqualTo", 
            [typeof(string), typeof(string)]);
        var argumentA = method.Argument<string>(0);
        var argumentB = method.Argument<string>(1);
        method.Return(argumentA.IsEqualTo(argumentB));
        
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<string, string, bool>>();
        var testStringA = TestContext.CurrentContext.Random.GetString(10);
        var testStringB = TestContext.CurrentContext.Random.GetString(10);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(functor(testStringA, testStringB), Is.False);
            Assert.That(functor(testStringA, testStringA), Is.True);
        }
    }
    
    [Test]
    public void IsNotEqualTo()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<bool>("IsEqualTo", 
            [typeof(string), typeof(string)]);
        var argumentA = method.Argument<string>(0);
        var argumentB = method.Argument<string>(1);
        method.Return(argumentA.IsNotEqualTo(argumentB));
        
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<string, string, bool>>();
        var testStringA = TestContext.CurrentContext.Random.GetString(10);
        var testStringB = TestContext.CurrentContext.Random.GetString(10);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(functor(testStringA, testStringB), Is.True);
            Assert.That(functor(testStringA, testStringA), Is.False);
        }
    }
}