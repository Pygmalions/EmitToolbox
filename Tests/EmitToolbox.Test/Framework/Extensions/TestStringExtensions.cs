using EmitToolbox.Extensions;
using EmitToolbox.Symbols;

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

    [Test]
    public void Format_TwoArguments_MixedTypes()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<string>(nameof(Format_TwoArguments_MixedTypes),
            [typeof(string), typeof(int), typeof(string)]);

        var fmt = method.Argument<string>(0);
        var a = method.Argument<int>(1);
        var b = method.Argument<string>(2);

        // Use ToObject() to convert arguments to ISymbol<object>
        method.Return(fmt.Format([a.ToObject(), b.ToObject()]));

        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<string, int, string, string>>();

        var x = TestContext.CurrentContext.Random.Next();
        var y = TestContext.CurrentContext.Random.GetString(8);
        const string pattern = "{0}-{1}";
        Assert.That(functor(pattern, x, y), Is.EqualTo(string.Format(pattern, x, y)));
    }

    [Test]
    public void Format_EmptyArguments_ReturnsOriginal()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<string>(nameof(Format_EmptyArguments_ReturnsOriginal),
            [typeof(string)]);

        var fmt = method.Argument<string>(0);

        // No placeholders and no arguments
        method.Return(fmt.Format([]));

        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<string, string>>();

        var pattern = TestContext.CurrentContext.Random.GetString(12);
        Assert.That(functor(pattern), Is.EqualTo(string.Format(pattern)));
    }

    [Test]
    public void Format_WithNullArgument()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<string>(nameof(Format_WithNullArgument),
            [typeof(string), typeof(object)]);

        var fmt = method.Argument<string>(0);
        var obj = method.Argument<object>(1);

        method.Return(fmt.Format([obj.ToObject()]));

        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<string, object?, string>>();

        const string pattern = "Value:{0}";
        object? value = null;
        Assert.That(functor(pattern, value), Is.EqualTo(string.Format(pattern, value)));
    }
}