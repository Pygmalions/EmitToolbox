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
    public void IReadOnlySet_ReadOperations()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        // Contains
        var mContains = type.MethodFactory.Static.DefineFunctor<bool>("ROSet_Contains", [typeof(IReadOnlySet<int>), typeof(int)]);
        var s1 = mContains.Argument<IReadOnlySet<int>>(0);
        var k1 = mContains.Argument<int>(1);
        mContains.Return(s1.Contains(k1));
        // IsSubsetOf
        var mSubset = type.MethodFactory.Static.DefineFunctor<bool>("ROSet_IsSubsetOf", [typeof(IReadOnlySet<int>), typeof(IEnumerable<int>)]);
        var s2 = mSubset.Argument<IReadOnlySet<int>>(0);
        var o2 = mSubset.Argument<IEnumerable<int>>(1);
        mSubset.Return(s2.IsSubsetOf(o2));
        // Overlaps
        var mOverlap = type.MethodFactory.Static.DefineFunctor<bool>("ROSet_Overlaps", [typeof(IReadOnlySet<int>), typeof(IEnumerable<int>)]);
        var s3 = mOverlap.Argument<IReadOnlySet<int>>(0);
        var o3 = mOverlap.Argument<IEnumerable<int>>(1);
        mOverlap.Return(s3.Overlaps(o3));
        // SetEquals
        var mEq = type.MethodFactory.Static.DefineFunctor<bool>("ROSet_SetEquals", [typeof(IReadOnlySet<int>), typeof(IEnumerable<int>)]);
        var s4 = mEq.Argument<IReadOnlySet<int>>(0);
        var o4 = mEq.Argument<IEnumerable<int>>(1);
        mEq.Return(s4.SetEquals(o4));
        type.Build();

        var fContains = mContains.BuildingMethod.CreateDelegate<Func<IReadOnlySet<int>, int, bool>>();
        var fSubset = mSubset.BuildingMethod.CreateDelegate<Func<IReadOnlySet<int>, IEnumerable<int>, bool>>();
        var fOverlap = mOverlap.BuildingMethod.CreateDelegate<Func<IReadOnlySet<int>, IEnumerable<int>, bool>>();
        var fEq = mEq.BuildingMethod.CreateDelegate<Func<IReadOnlySet<int>, IEnumerable<int>, bool>>();

        var data = new HashSet<int> { 1, 2, 3 };
        Assert.That(fContains(data, 2), Is.True);
        Assert.That(fContains(data, 5), Is.False);
        Assert.That(fSubset(data, [1, 2, 3, 4]), Is.True);
        Assert.That(fOverlap(data, [4, 5, 3]), Is.True);
        Assert.That(fEq(data, [3, 2, 1]), Is.True);
    }

    [Test]
    public void ISet_WriteOperations_And_Relations()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var m = type.MethodFactory.Static.DefineAction(nameof(ISet_WriteOperations_And_Relations), [
            typeof(ISet<int>),
            typeof(IEnumerable<int>), // unionWith
            typeof(IEnumerable<int>), // intersectWith
            typeof(IEnumerable<int>), // exceptWith
            typeof(IEnumerable<int>)  // symmetricExceptWith
        ]);
        var s = m.Argument<ISet<int>>(0);
        var u = m.Argument<IEnumerable<int>>(1);
        var inter = m.Argument<IEnumerable<int>>(2);
        var exc = m.Argument<IEnumerable<int>>(3);
        var sym = m.Argument<IEnumerable<int>>(4);
        s.Clear();
        s.Add(m.Value(1));
        s.Add(m.Value(2));
        s.Add(m.Value(3));
        s.Remove(m.Value(2));
        // Use UnionWith/IntersectWith/ExceptWith/SymmetricExceptWith with provided params
        s.UnionWith(u);
        s.IntersectWith(inter);
        s.ExceptWith(exc);
        s.SymmetricExceptWith(sym);
        m.Return();
        type.Build();

        var f = m.BuildingMethod.CreateDelegate<Action<ISet<int>, IEnumerable<int>, IEnumerable<int>, IEnumerable<int>, IEnumerable<int>>>();
        var set = new HashSet<int> { 10 };
        f(set, [4], [1, 3, 4], [1], [4, 5]);
        // After operations: start {}, add 1,2,3 -> {1,2,3}, remove 2 -> {1,3}
        // UnionWith {4} -> {1,3,4}; Intersect with {1,3,4} -> {1,3,4}; Except {1} -> {3,4}
        // SymmetricExceptWith {4,5} -> {3,5}
        Assert.That(set.SetEquals([3, 5]), Is.True);
    }
}
