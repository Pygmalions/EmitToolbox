using EmitToolbox.Extensions;
using EmitToolbox.Symbols;

namespace EmitToolbox.Test.Extensions;

[TestFixture, TestOf(typeof(ListExtensions))]
public class TestListExtensions
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    [Test]
    public void IReadOnlyList_ElementAt_Read_BySymbol_And_ByConst()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var m1 = type.MethodFactory.Static.DefineFunctor<int>(nameof(IReadOnlyList_ElementAt_Read_BySymbol_And_ByConst), [typeof(IReadOnlyList<int>), typeof(int)]);
        var listArg = m1.Argument<IReadOnlyList<int>>(0);
        var indexArg = m1.Argument<int>(1);
        var bySymbol = listArg.ElementAt(indexArg);
        var byConst = listArg.ElementAt(0);
        m1.Return(bySymbol + byConst);
        type.Build();

        var f = m1.BuildingMethod.CreateDelegate<Func<IReadOnlyList<int>, int, int>>();
        var data = Enumerable.Range(1, 10).ToList();
        var idx = TestContext.CurrentContext.Random.Next(0, data.Count);
        Assert.That(f(data, idx), Is.EqualTo(data[idx] + data[0]));
    }

    [Test]
    public void IList_ElementAt_Write_Read_IndexOf_Insert_RemoveAt()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var m = type.MethodFactory.Static.DefineAction(nameof(IList_ElementAt_Write_Read_IndexOf_Insert_RemoveAt), [typeof(IList<int>), typeof(int), typeof(int)]);
        var list = m.Argument<IList<int>>(0);
        var value = m.Argument<int>(1);
        var index = m.Argument<int>(2);
        // write via ItemSymbol
        var item = list.ElementAt(index);
        item.AssignContent(value);
        // insert another value at the same index then remove it to test both APIs
        list.Insert(index, value);
        list.RemoveAt(index);
        m.Return();
        type.Build();

        var f = m.BuildingMethod.CreateDelegate<Action<IList<int>, int, int>>();
        var arr = Enumerable.Repeat(0, 5).ToList();
        var v = TestContext.CurrentContext.Random.Next(-100, 100);
        var idx = TestContext.CurrentContext.Random.Next(0, arr.Count);
        f(arr, v, idx);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(arr[idx], Is.EqualTo(v));
            Assert.That(arr.IndexOf(v), Is.GreaterThanOrEqualTo(0));
        }
    }
}
