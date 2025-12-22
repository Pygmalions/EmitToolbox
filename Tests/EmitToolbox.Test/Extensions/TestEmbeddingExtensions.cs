using System.Runtime.CompilerServices;
using EmitToolbox.Extensions;
using EmitToolbox.Symbols;

namespace EmitToolbox.Test.Extensions;

[TestFixture, TestOf(typeof(EmbeddingDelegateExtensions))]
public class TestEmbeddingDelegateExtensions
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    [Test]
    public void Embed_Action()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineAction(nameof(Embed_Action),
            [typeof(StrongBox<int>)]);
        var argument = method.Argument<StrongBox<int>>(0);
        method.EmbedStaticDelegate(argument, static target =>
        {
            target.Value += 1;
        });
        method.Return();
        type.Build();
        
        var functor = method.BuildingMethod.CreateDelegate<Action<StrongBox<int>>>();
        
        var testNumber = TestContext.CurrentContext.Random.Next();
        var testInstance = new StrongBox<int>(testNumber);
        Assert.DoesNotThrow(() => functor(testInstance));
        Assert.That(testInstance.Value, Is.EqualTo(testNumber + 1));
    }

    [Test]
    public void Embed_Func()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int>(nameof(Embed_Func),
            [typeof(int)]);
        var argument = method.Argument<int>(0);
        method.Return(method.EmbedStaticDelegate(argument, static target => target + 1));
        type.Build();
        
        var functor = method.BuildingMethod.CreateDelegate<Func<int, int>>();
        
        var testNumber = TestContext.CurrentContext.Random.Next();
        var testResult = 0;
        Assert.DoesNotThrow(() => testResult = functor(testNumber));
        Assert.That(testResult, Is.EqualTo(testNumber + 1));
    }
    
    [Test]
    public void NonStaticDelegate_Throws()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineAction(nameof(NonStaticDelegate_Throws),
            [typeof(StrongBox<int>)]);
        var argument = method.Argument<StrongBox<int>>(0);
        var captured = 1;
        Assert.Throws<ArgumentException>(() =>
        {
            method.EmbedStaticDelegate(argument, target =>
            {
                target.Value += captured;
            });
        });
    }
}