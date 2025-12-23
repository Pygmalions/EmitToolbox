using EmitToolbox.Extensions;
using EmitToolbox.Symbols;

namespace EmitToolbox.Test.Extensions;

[TestFixture, TestOf(typeof(SetExtensions))]
public class TestSetExtensions
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    [Test]
    public void IReadOnlySet_Contains()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var containsMethod = type.MethodFactory.Static.DefineFunctor<bool>("ROSet_Contains", [typeof(IReadOnlySet<int>), typeof(int)]);
        var set = containsMethod.Argument<IReadOnlySet<int>>(0);
        var key = containsMethod.Argument<int>(1);
        containsMethod.Return(set.Contains(key));
        type.Build();

        var containsFunc = containsMethod.BuildingMethod.CreateDelegate<Func<IReadOnlySet<int>, int, bool>>();
        var testSet = new HashSet<int> { 1, 2, 3 };
        using (Assert.EnterMultipleScope())
        {
            Assert.That(containsFunc(testSet, 2), Is.True);
            Assert.That(containsFunc(testSet, 5), Is.False);
        }
    }

    [Test]
    public void IReadOnlySet_IsSubsetOf()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var isSubsetOfMethod = type.MethodFactory.Static.DefineFunctor<bool>("ROSet_IsSubsetOf", [typeof(IReadOnlySet<int>), typeof(IEnumerable<int>)]);
        var set = isSubsetOfMethod.Argument<IReadOnlySet<int>>(0);
        var other = isSubsetOfMethod.Argument<IEnumerable<int>>(1);
        isSubsetOfMethod.Return(set.IsSubsetOf(other));
        type.Build();

        var isSubsetOfFunc = isSubsetOfMethod.BuildingMethod.CreateDelegate<Func<IReadOnlySet<int>, IEnumerable<int>, bool>>();
        var testSet = new HashSet<int> { 1, 2, 3 };
        Assert.That(isSubsetOfFunc(testSet, [1, 2, 3, 4]), Is.True);
    }

    [Test]
    public void IReadOnlySet_Overlaps()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var overlapsMethod = type.MethodFactory.Static.DefineFunctor<bool>("ROSet_Overlaps", [typeof(IReadOnlySet<int>), typeof(IEnumerable<int>)]);
        var set = overlapsMethod.Argument<IReadOnlySet<int>>(0);
        var other = overlapsMethod.Argument<IEnumerable<int>>(1);
        overlapsMethod.Return(set.Overlaps(other));
        type.Build();

        var overlapsFunc = overlapsMethod.BuildingMethod.CreateDelegate<Func<IReadOnlySet<int>, IEnumerable<int>, bool>>();
        var testSet = new HashSet<int> { 1, 2, 3 };
        Assert.That(overlapsFunc(testSet, [4, 5, 3]), Is.True);
    }

    [Test]
    public void IReadOnlySet_SetEquals()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var setEqualsMethod = type.MethodFactory.Static.DefineFunctor<bool>("ROSet_SetEquals", [typeof(IReadOnlySet<int>), typeof(IEnumerable<int>)]);
        var set = setEqualsMethod.Argument<IReadOnlySet<int>>(0);
        var other = setEqualsMethod.Argument<IEnumerable<int>>(1);
        setEqualsMethod.Return(set.SetEquals(other));
        type.Build();

        var setEqualsFunc = setEqualsMethod.BuildingMethod.CreateDelegate<Func<IReadOnlySet<int>, IEnumerable<int>, bool>>();
        var testSet = new HashSet<int> { 1, 2, 3 };
        Assert.That(setEqualsFunc(testSet, [3, 2, 1]), Is.True);
    }

    [Test]
    public void ISet_Add()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<bool>("ISet_Add", [typeof(ISet<int>), typeof(int)]);
        var set = method.Argument<ISet<int>>(0);
        var item = method.Argument<int>(1);
        method.Return(set.Add(item));
        type.Build();

        var func = method.BuildingMethod.CreateDelegate<Func<ISet<int>, int, bool>>();
        var testSet = new HashSet<int> { 1 };
        using (Assert.EnterMultipleScope())
        {
            Assert.That(func(testSet, 2), Is.True);
            Assert.That(testSet, Does.Contain(2));
            Assert.That(func(testSet, 1), Is.False);
        }
    }

    [Test]
    public void ISet_Clear()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineAction("ISet_Clear", [typeof(ISet<int>)]);
        var set = method.Argument<ISet<int>>(0);
        set.Clear();
        method.Return();
        type.Build();

        var func = method.BuildingMethod.CreateDelegate<Action<ISet<int>>>();
        var testSet = new HashSet<int> { 1, 2, 3 };
        func(testSet);
        Assert.That(testSet, Is.Empty);
    }

    [Test]
    public void ISet_Remove()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<bool>("ISet_Remove", [typeof(ISet<int>), typeof(int)]);
        var set = method.Argument<ISet<int>>(0);
        var item = method.Argument<int>(1);
        method.Return(set.Remove(item));
        type.Build();

        var func = method.BuildingMethod.CreateDelegate<Func<ISet<int>, int, bool>>();
        var testSet = new HashSet<int> { 1, 2 };
        using (Assert.EnterMultipleScope())
        {
            Assert.That(func(testSet, 1), Is.True);
            Assert.That(testSet, Does.Not.Contain(1));
            Assert.That(func(testSet, 3), Is.False);
        }
    }

    [Test]
    public void ISet_UnionWith()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineAction("ISet_UnionWith", [typeof(ISet<int>), typeof(IEnumerable<int>)]);
        var set = method.Argument<ISet<int>>(0);
        var other = method.Argument<IEnumerable<int>>(1);
        set.UnionWith(other);
        method.Return();
        type.Build();

        var func = method.BuildingMethod.CreateDelegate<Action<ISet<int>, IEnumerable<int>>>();
        var testSet = new HashSet<int> { 1, 2 };
        func(testSet, [2, 3]);
        Assert.That(testSet.SetEquals([1, 2, 3]), Is.True);
    }

    [Test]
    public void ISet_IntersectWith()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineAction("ISet_IntersectWith", [typeof(ISet<int>), typeof(IEnumerable<int>)]);
        var set = method.Argument<ISet<int>>(0);
        var other = method.Argument<IEnumerable<int>>(1);
        set.IntersectWith(other);
        method.Return();
        type.Build();

        var func = method.BuildingMethod.CreateDelegate<Action<ISet<int>, IEnumerable<int>>>();
        var testSet = new HashSet<int> { 1, 2, 3 };
        func(testSet, [2, 3, 4]);
        Assert.That(testSet.SetEquals([2, 3]), Is.True);
    }

    [Test]
    public void ISet_ExceptWith()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineAction("ISet_ExceptWith", [typeof(ISet<int>), typeof(IEnumerable<int>)]);
        var set = method.Argument<ISet<int>>(0);
        var other = method.Argument<IEnumerable<int>>(1);
        set.ExceptWith(other);
        method.Return();
        type.Build();

        var func = method.BuildingMethod.CreateDelegate<Action<ISet<int>, IEnumerable<int>>>();
        var testSet = new HashSet<int> { 1, 2, 3 };
        func(testSet, [2, 4]);
        Assert.That(testSet.SetEquals([1, 3]), Is.True);
    }

    [Test]
    public void ISet_SymmetricExceptWith()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineAction("ISet_SymmetricExceptWith", [typeof(ISet<int>), typeof(IEnumerable<int>)]);
        var set = method.Argument<ISet<int>>(0);
        var other = method.Argument<IEnumerable<int>>(1);
        set.SymmetricExceptWith(other);
        method.Return();
        type.Build();

        var func = method.BuildingMethod.CreateDelegate<Action<ISet<int>, IEnumerable<int>>>();
        var testSet = new HashSet<int> { 1, 2, 3 };
        func(testSet, [2, 3, 4]);
        Assert.That(testSet.SetEquals([1, 4]), Is.True);
    }
}
