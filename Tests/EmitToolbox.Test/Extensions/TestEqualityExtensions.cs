using EmitToolbox.Extensions;
using EmitToolbox.Symbols;

namespace EmitToolbox.Test.Extensions;

[TestFixture, TestOf(typeof(EqualityExtensions))]
public class TestEqualityExtensions
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    private Func<T, T, bool> CreateEqualityComparator<T>(
        string name,
        Func<ISymbol<T>, ISymbol<T>, ISymbol<bool>> compare)
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<bool>(
            name,
            [typeof(T), typeof(T)]);
        var a = method.Argument<T>(0);
        var b = method.Argument<T>(1);
        method.Return(compare(a, b));
        type.Build();
        return method.BuildingMethod.CreateDelegate<Func<T, T, bool>>();
    }

    [Test]
    public void InvokeEquals_Primitive_Int_Equal_And_NotEqual()
    {
        var functor = CreateEqualityComparator<int>(nameof(InvokeEquals_Primitive_Int_Equal_And_NotEqual),
            (a, b) => a.InvokeEquals(b));
        using (Assert.EnterMultipleScope())
        {
            var x = TestContext.CurrentContext.Random.Next();
            Assert.That(functor(x, x), Is.True,
                "Expected Ceq path for identical ints to be true");

            int y;
            do
            {
                y = TestContext.CurrentContext.Random.Next();
            } while (y == x);

            Assert.That(functor(x, y), Is.False,
                "Expected Ceq path for different ints to be false");
        }
    }

    [Test]
    public void InvokeEquals_String_OperatorEquality()
    {
        var functor =
            CreateEqualityComparator<string>(nameof(InvokeEquals_String_OperatorEquality), (a, b) => a.InvokeEquals(b));
        using (Assert.EnterMultipleScope())
        {
            var s1 = TestContext.CurrentContext.Random.GetString();
            var s2 = new string(s1);
            Assert.That(functor(s1, s2), Is.True);

            var t = s1 + "_diff";
            Assert.That(functor(s1, t), Is.False);
        }
    }

    [Test]
    public void InvokeEquals_Tuple_SpecializedEquals()
    {
        var functor = CreateEqualityComparator<Tuple<int>>(nameof(InvokeEquals_Tuple_SpecializedEquals),
            (a, b) => a.InvokeEquals(b));
        using (Assert.EnterMultipleScope())
        {
            var x = TestContext.CurrentContext.Random.Next();
            var t1 = Tuple.Create(x);
            var t2 = Tuple.Create(x); // Different instance, same value
            Assert.That(functor(t1, t2), Is.True, "Tuple<T>.Equals(Tuple<T>) should compare by value");

            var t3 = Tuple.Create(x + 1);
            Assert.That(functor(t1, t3), Is.False);
        }
    }

    [Test]
    public void InvokeEquals_ObjectFallback_ObjectEquals()
    {
        var functor = CreateEqualityComparator<object>(nameof(InvokeEquals_ObjectFallback_ObjectEquals),
            (a, b) => a.InvokeEquals(b));
        using (Assert.EnterMultipleScope())
        {
            var testObject = new object();
            Assert.That(functor(testObject, testObject), Is.True);

            Assert.That(functor(testObject, new object()), Is.False);
        }
    }

    [Test]
    public void InvokeReferenceEquals_ReferenceType()
    {
        var functor = CreateEqualityComparator<object>(nameof(InvokeReferenceEquals_ReferenceType),
            (a, b) => a.InvokeReferenceEquals(b));
        using (Assert.EnterMultipleScope())
        {
            var obj = new object();
            Assert.That(functor(obj, obj), Is.True, "Same reference should be true");

            Assert.That(functor(new object(), new object()), Is.False, "Different references should be false");
        }
    }

    [Test]
    public void InvokeReferenceEquals_ValueType_AlwaysFalse()
    {
        var functor = CreateEqualityComparator<int>(nameof(InvokeReferenceEquals_ValueType_AlwaysFalse),
            (a, b) => a.InvokeReferenceEquals(b));
        using (Assert.EnterMultipleScope())
        {
            var x = TestContext.CurrentContext.Random.Next();
            Assert.That(functor(x, x), Is.False);

            var y = x + 1;
            Assert.That(functor(x, y), Is.False);
        }
    }
}