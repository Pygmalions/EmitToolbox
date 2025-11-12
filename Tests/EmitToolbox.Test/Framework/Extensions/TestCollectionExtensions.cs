using EmitToolbox.Framework;
using EmitToolbox.Framework.Extensions;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Test.Framework.Extensions;

[TestFixture, TestOf(typeof(EmitToolbox.Framework.Extensions.CollectionExtensions))]
public class TestCollectionExtensions
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    [Test]
    public void IReadOnlyCollection_Length()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int>(nameof(IReadOnlyCollection_Length), [typeof(IReadOnlyCollection<int>)]);
        var arg = method.Argument<IReadOnlyCollection<int>>(0);
        method.Return(arg.Length);
        type.Build();

        var functor = method.BuildingMethod.CreateDelegate<Func<IReadOnlyCollection<int>, int>>();
        var data = Enumerable.Range(0, TestContext.CurrentContext.Random.Next(0, 20)).ToArray();
        Assert.That(functor(data), Is.EqualTo(data.Length));
    }

    [Test]
    public void ICollection_Add_Remove()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineAction(
            nameof(ICollection_Add_Remove), [typeof(ICollection<int>), typeof(int), typeof(int)]);
        var collection = method.Argument<ICollection<int>>(0);
        var valueA = method.Argument<int>(1);
        var valueB = method.Argument<int>(2);
        collection.Clear();
        collection.Add(valueA);
        collection.Add(valueB);
        // Remove one and keep another
        collection.Remove(valueA);
        method.Return();
        type.Build();

        var functor = method.BuildingMethod.CreateDelegate<Action<ICollection<int>, int, int>>();
        var list = new List<int>();
        var a = TestContext.CurrentContext.Random.Next(-100, 100);
        var b = TestContext.CurrentContext.Random.Next(-100, 100);
        functor(list, a, b);
        
        Assert.That(list, Has.Count.EqualTo(1));
        using (Assert.EnterMultipleScope())
        {
            Assert.That(list, Does.Contain(b));
            Assert.That(list, Does.Not.Contain(a));
        }
    }

    [Test]
    public void ICollection_Clear()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = 
            type.MethodFactory.Static.DefineAction(nameof(ICollection_Clear), [typeof(ICollection<int>)]);
        var argumentList = method.Argument<ICollection<int>>(0);
        argumentList.Clear();
        method.Return();
        type.Build();
        var testList = new List<int>()
        {
            TestContext.CurrentContext.Random.Next(-100, 100),
            TestContext.CurrentContext.Random.Next(-100, 100)
        };
        var functor = method.BuildingMethod.CreateDelegate<Action<ICollection<int>>>();
        functor(testList);
        Assert.That(testList, Is.Empty);
    }
    
    [Test]
    public void ICollection_Contains()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = 
            type.MethodFactory.Static.DefineFunctor<bool>(nameof(ICollection_Contains), 
                [typeof(ICollection<int>), typeof(int)]);
        var argumentList = method.Argument<ICollection<int>>(0);
        var argumentValue = method.Argument<int>(1);
        method.Return(argumentList.Contains(argumentValue));
        type.Build();
        var testList = new List<int>()
        {
            1, 2, 3
        };
        var functor = method.BuildingMethod.CreateDelegate<Func<ICollection<int>, int, bool>>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(functor(testList, 1), Is.True);
            Assert.That(functor(testList, 2), Is.True);
            Assert.That(functor(testList, 3), Is.True);
            Assert.That(functor(testList, 4), Is.False);
        }
        
    }

    [Test]
    public void ICollection_CopyTo()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = 
            type.MethodFactory.Static.DefineAction(nameof(ICollection_CopyTo), [typeof(ICollection<int>), typeof(int[]), typeof(int)]);
        var collection = method.Argument<ICollection<int>>(0);
        var array = method.Argument<int[]>(1);
        var index = method.Argument<int>(2);
        collection.CopyTo(array, index);
        method.Return();
        type.Build();

        var functor = method.BuildingMethod.CreateDelegate<Action<ICollection<int>, int[], int>>();
        var list = new List<int>(Enumerable.Range(0, TestContext.CurrentContext.Random.Next(3, 8)));
        var offset = TestContext.CurrentContext.Random.Next(0, 3);
        var target = new int[list.Count + offset + 2];
        functor(list, target, offset);
        Assert.That(target.Skip(offset).Take(list.Count), Is.EqualTo(list));
    }
}
