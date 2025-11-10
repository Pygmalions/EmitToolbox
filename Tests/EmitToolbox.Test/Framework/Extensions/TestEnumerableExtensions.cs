using EmitToolbox.Framework;
using EmitToolbox.Framework.Extensions;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Test.Framework.Extensions;

[TestFixture, TestOf(typeof(EnumerableExtensions))]
public class TestEnumerableExtensions
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    [Test]
    public void ForEach()
    {
        var data = new int[TestContext.CurrentContext.Random.Next(5, 10)];
        for (var index = 0; index < data.Length; index++)
            data[index] = TestContext.CurrentContext.Random.Next(0, 100);
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int>(
            "TestForEach", [typeof(IEnumerable<int>)]);
        var argument = method.Argument<IEnumerable<int>>(0);
        var sum = method.Variable<int>();
        argument.ForEach(element =>
        {
            sum.AssignContent(sum + element);
        });
        method.Return(sum);
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<IEnumerable<int>, int>>();
        Assert.That(functor(data), Is.EqualTo(data.Sum()));
    }
}