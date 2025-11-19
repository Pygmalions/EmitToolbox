using EmitToolbox.Extensions;
using EmitToolbox.Symbols;

namespace EmitToolbox.Test.Framework.Extensions;

[TestFixture, TestOf(typeof(LiteralValueExtensions))]
public class TestLiteralValueExtensions
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    private Func<T, TResult> CreateUnaryTestFunctor<T, TResult>(string name, Func<ISymbol<T>, ISymbol<TResult>> body)
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<TResult>(name, [typeof(T)]);
        var a = method.Argument<T>(0);
        method.Return(body(a));
        type.Build();
        return method.BuildingMethod.CreateDelegate<Func<T, TResult>>();
    }

    [Test]
    public void AssignValue_FromLiteral_Int()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int>(nameof(AssignValue_FromLiteral_Int));
        var local = method.Variable<int>();
        var k = TestContext.CurrentContext.Random.Next();
        local.AssignValue(k);
        method.Return(local);
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<int>>();
        var result = functor();
        Assert.That(result, Is.EqualTo(k));
    }

    [Test]
    public void Arithmetic_With_Literal_RHS_Int()
    {
        var k = TestContext.CurrentContext.Random.Next(1, int.MaxValue / 4);

        var add = CreateUnaryTestFunctor<int, int>(
            nameof(Arithmetic_With_Literal_RHS_Int) + "_Add", a => a + k);
        var sub = CreateUnaryTestFunctor<int, int>(
            nameof(Arithmetic_With_Literal_RHS_Int) + "_Sub", a => a - k);
        var mul = CreateUnaryTestFunctor<int, int>(
            nameof(Arithmetic_With_Literal_RHS_Int) + "_Mul", a => a * k);
        var div = CreateUnaryTestFunctor<int, int>(
            nameof(Arithmetic_With_Literal_RHS_Int) + "_Div", a => a / k);
        var rem = CreateUnaryTestFunctor<int, int>(
            nameof(Arithmetic_With_Literal_RHS_Int) + "_Rem", a => a % k);

        var x = TestContext.CurrentContext.Random.Next();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(add(x), Is.EqualTo(x + k));
            Assert.That(sub(x), Is.EqualTo(x - k));
            Assert.That(mul(x), Is.EqualTo(x * k));
            Assert.That(div(x), Is.EqualTo(x / k));
            Assert.That(rem(x), Is.EqualTo(x % k));
        }
    }

    [Test]
    public void Comparisons_With_Literal_RHS_Int()
    {
        var k = TestContext.CurrentContext.Random.Next();
        var gt = CreateUnaryTestFunctor<int, bool>(
            nameof(Comparisons_With_Literal_RHS_Int) + "_GT", a => a > k);
        var ge = CreateUnaryTestFunctor<int, bool>(
            nameof(Comparisons_With_Literal_RHS_Int) + "_GE", a => a >= k);
        var lt = CreateUnaryTestFunctor<int, bool>(
            nameof(Comparisons_With_Literal_RHS_Int) + "_LT", a => a < k);
        var le = CreateUnaryTestFunctor<int, bool>(
            nameof(Comparisons_With_Literal_RHS_Int) + "_LE", a => a <= k);
        var eq = CreateUnaryTestFunctor<int, bool>(
            nameof(Comparisons_With_Literal_RHS_Int) + "_EQ", a => a.IsEqualTo(k));
        var ne = CreateUnaryTestFunctor<int, bool>(
            nameof(Comparisons_With_Literal_RHS_Int) + "_NE", a => a.IsNotEqualTo(k));

        var a = TestContext.CurrentContext.Random.Next();
        var b = TestContext.CurrentContext.Random.Next();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(gt(a), Is.EqualTo(a > k));
            Assert.That(ge(a), Is.EqualTo(a >= k));
            Assert.That(lt(a), Is.EqualTo(a < k));
            Assert.That(le(a), Is.EqualTo(a <= k));
            Assert.That(eq(a), Is.EqualTo(a == k));
            Assert.That(ne(a), Is.EqualTo(a != k));

            Assert.That(gt(b), Is.EqualTo(b > k));
            Assert.That(ge(b), Is.EqualTo(b >= k));
            Assert.That(lt(b), Is.EqualTo(b < k));
            Assert.That(le(b), Is.EqualTo(b <= k));
            Assert.That(eq(b), Is.EqualTo(b == k));
            Assert.That(ne(b), Is.EqualTo(b != k));
        }
    }

    [Test]
    public void Bool_IsEqualTo_With_Literal()
    {
        var eqTrue = CreateUnaryTestFunctor<bool, bool>(
            nameof(Bool_IsEqualTo_With_Literal) + "_True", 
            a => a.IsEqualTo(true));
        var neTrue = CreateUnaryTestFunctor<bool, bool>(
            nameof(Bool_IsEqualTo_With_Literal) + "_NeTrue", 
            a => a.IsNotEqualTo(true));
        var eqFalse = CreateUnaryTestFunctor<bool, bool>(
            nameof(Bool_IsEqualTo_With_Literal) + "_False", 
            a => a.IsEqualTo(false));
        var neFalse = CreateUnaryTestFunctor<bool, bool>(
            nameof(Bool_IsEqualTo_With_Literal) + "_NeFalse", 
            a => a.IsNotEqualTo(false));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(eqTrue(true), Is.True);
            Assert.That(eqTrue(false), Is.False);
            Assert.That(neTrue(true), Is.False);
            Assert.That(neTrue(false), Is.True);

            Assert.That(eqFalse(false), Is.True);
            Assert.That(eqFalse(true), Is.False);
            Assert.That(neFalse(false), Is.False);
            Assert.That(neFalse(true), Is.True);
        }
    }

    [Test]
    public void String_IsEqualTo_With_Literal()
    {
        var lit = TestContext.CurrentContext.Random.GetString();
        var eq = CreateUnaryTestFunctor<string, bool>(nameof(String_IsEqualTo_With_Literal) + "_Eq", a => a.IsEqualTo(lit));
        var ne = CreateUnaryTestFunctor<string, bool>(nameof(String_IsEqualTo_With_Literal) + "_Ne", a => a.IsNotEqualTo(lit));

        var s1 = lit;
        var s2 = TestContext.CurrentContext.Random.GetString();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(eq(s1), Is.True);
            Assert.That(ne(s1), Is.False);

            Assert.That(eq(s2), Is.EqualTo(string.Equals(s2, lit)));
            Assert.That(ne(s2), Is.EqualTo(!string.Equals(s2, lit)));
        }
    }
}
