using EmitToolbox.Extensions;
using EmitToolbox.Symbols;

namespace EmitToolbox.Test.Extensions;

[TestFixture, TestOf(typeof(DictionaryExtensions))]
public class TestDictionaryExtensions
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    [Test]
    public void IReadOnlyDictionary_ElementAt_ContainsKey_TryGetValue_Keys_Values()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        // Build 3 methods:
        // 1) get value by key
        var getValue = type.MethodFactory.Static.DefineFunctor<int>("RODict_GetValue", [typeof(IReadOnlyDictionary<string, int>), typeof(string)]);
        var dict1 = getValue.Argument<IReadOnlyDictionary<string, int>>(0);
        var key1 = getValue.Argument<string>(1);
        getValue.Return(dict1.ElementAt(key1));
        // 2) contains key
        var contains = type.MethodFactory.Static.DefineFunctor<bool>("RODict_Contains", [typeof(IReadOnlyDictionary<string, int>), typeof(string)]);
        var dict2 = contains.Argument<IReadOnlyDictionary<string, int>>(0);
        var key2 = contains.Argument<string>(1);
        contains.Return(dict2.ContainsKey(key2));
        // 3) try get value
        var tryGet = type.MethodFactory.Static.DefineFunctor<bool>("RODict_TryGet", [typeof(IReadOnlyDictionary<string, int>), typeof(string)]);
        var dict3 = tryGet.Argument<IReadOnlyDictionary<string, int>>(0);
        var key3 = tryGet.Argument<string>(1);
        var tmp = tryGet.Variable<int>();
        tryGet.Return(dict3.TryGetValue(key3, tmp));
        // 4) keys/values count
        var countAll = type.MethodFactory.Static.DefineFunctor<int>("RODict_CountKeysValues", [typeof(IReadOnlyDictionary<string, int>)]);
        var dict4 = countAll.Argument<IReadOnlyDictionary<string, int>>(0);
        var keysCount = countAll.Invoke<int>(typeof(Enumerable).GetMethods().First(x => x.Name == nameof(Enumerable.Count) && x.GetParameters().Length == 1).MakeGenericMethod(typeof(string)), [dict4.Keys]);
        var valuesCount = countAll.Invoke<int>(typeof(Enumerable).GetMethods().First(x => x.Name == nameof(Enumerable.Count) && x.GetParameters().Length == 1).MakeGenericMethod(typeof(int)), [dict4.Values]);
        countAll.Return(keysCount + valuesCount);
        type.Build();

        var fGet = getValue.BuildingMethod.CreateDelegate<Func<IReadOnlyDictionary<string, int>, string, int>>();
        var fHas = contains.BuildingMethod.CreateDelegate<Func<IReadOnlyDictionary<string, int>, string, bool>>();
        var fTry = tryGet.BuildingMethod.CreateDelegate<Func<IReadOnlyDictionary<string, int>, string, bool>>();
        var fCount = countAll.BuildingMethod.CreateDelegate<Func<IReadOnlyDictionary<string, int>, int>>();
        var data = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 };
        Assert.That(fGet(data, "a"), Is.EqualTo(1));
        Assert.That(fHas(data, "a"), Is.True);
        Assert.That(fHas(data, "c"), Is.False);
        Assert.That(fTry(data, "b"), Is.True);
        Assert.That(fTry(data, "c"), Is.False);
        Assert.That(fCount(data), Is.EqualTo(4));
    }

    [Test]
    public void IDictionary_Set_Add_Remove_Clear_ContainsKey_TryGetValue_Keys_Values()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var m = type.MethodFactory.Static.DefineAction(nameof(IDictionary_Set_Add_Remove_Clear_ContainsKey_TryGetValue_Keys_Values), [typeof(IDictionary<string, int>)]);
        var dict = m.Argument<IDictionary<string, int>>(0);
        var one = m.Variable<int>();
        one.AssignValue(1);
        dict.Clear();
        dict.Add(m.Literal("x"), one);
        dict.Set(m.Literal("y"), m.Literal(2));
        dict.Remove(m.Literal("x"));
        var tmp = m.Variable<int>();
        dict.TryGetValue(m.Literal("y"), tmp);
        m.Return();
        type.Build();

        var f = m.BuildingMethod.CreateDelegate<Action<IDictionary<string, int>>>();
        var d = new Dictionary<string, int>();
        f(d);
        Assert.That(d.ContainsKey("y"), Is.True);
        Assert.That(d.ContainsKey("x"), Is.False);
        Assert.That(d["y"], Is.EqualTo(2));
    }
}
