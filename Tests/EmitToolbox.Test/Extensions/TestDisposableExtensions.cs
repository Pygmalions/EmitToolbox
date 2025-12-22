using EmitToolbox.Extensions;
using EmitToolbox.Symbols;

namespace EmitToolbox.Test.Extensions;

[TestFixture, TestOf(typeof(DisposableExtensions))]
public class TestDisposableExtensions
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    private class SampleDisposable : IDisposable
    {
        public int DisposedCount { get; private set; }

        public void Dispose()
        {
            DisposedCount++;
        }
    }

    [Test]
    public void Dispose_Once()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineAction("DisposeOnce", [typeof(IDisposable)]);
        var argument = method.Argument<IDisposable>(0);
        DisposableExtensions.InvokeDispose<IDisposable>(argument);
        method.Return();
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Action<IDisposable>>();
        var sample = new SampleDisposable();
        Assert.DoesNotThrow(() => functor(sample));
        Assert.That(sample.DisposedCount, Is.EqualTo(1));
    }
}