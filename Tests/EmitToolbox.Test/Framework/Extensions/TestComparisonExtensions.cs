using System.Numerics;
using EmitToolbox.Framework;
using EmitToolbox.Framework.Extensions;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Test.Framework.Extensions;

[TestFixture, TestOf(typeof(ComparisonExtensions))]
public class TestComparisonExtensions
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    private Func<TLeft, TRight, int> CreateCompareToMethod_Generic<TLeft, TRight>()
        where TLeft : IComparable<TRight>
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int>(
            $"CompareTo_{typeof(TLeft).Name}_{typeof(TRight).Name}",
            [typeof(TLeft), typeof(TRight)]);
        var left = method.Argument<TLeft>(0);
        var right = method.Argument<TRight>(1);
        method.Return(left.CompareTo(right));
        type.Build();
        return method.BuildingMethod.CreateDelegate<Func<TLeft, TRight, int>>();
    }

    private Func<TLeft, object, int> CreateCompareToMethod_NonGenericOther<TLeft>()
        where TLeft : IComparable
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int>(
            $"CompareTo_{typeof(TLeft).Name}_Object",
            [typeof(TLeft), typeof(object)]);
        var left = method.Argument<TLeft>(0);
        var right = method.Argument<object>(1);
        method.Return(left.CompareTo(right));
        type.Build();
        return method.BuildingMethod.CreateDelegate<Func<TLeft, object, int>>();
    }

    [Test]
    public void CompareTo_Generic_Int()
    {
        var func = CreateCompareToMethod_Generic<int, int>();
        var number = TestContext.CurrentContext.Random.Next(1, int.MaxValue - 1);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(func(number, number), 
                Is.EqualTo(0));
            Assert.That(func(number, 
                    TestContext.CurrentContext.Random.Next(int.MinValue, number - 1)), 
                Is.GreaterThan(0));
            Assert.That(func(number, 
                    TestContext.CurrentContext.Random.Next(number + 1, int.MaxValue)), 
                Is.LessThan(0));
        }
        
    }

    [Test]
    public void CompareTo_NonGeneric_String_To_Object()
    {
        var func = CreateCompareToMethod_NonGenericOther<string>();
        var a = TestContext.CurrentContext.Random.GetString();
        object b = a;
        var result = func(a, b);
        Assert.That(Math.Sign(result),
            Is.EqualTo(Math.Sign(string.Compare(a, (string)b, StringComparison.Ordinal))));
    }
    
    private Func<T, T, bool> CreateBinaryComparisonMethod<T>(
        string name, Func<ISymbol<T>, ISymbol<T>, ISymbol<bool>> op)
        where T : IComparisonOperators<T, T, bool>
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<bool>(name, [typeof(T), typeof(T)]);
        var left = method.Argument<T>(0);
        var right = method.Argument<T>(1);
        method.Return(op(left, right));
        type.Build();
        return method.BuildingMethod.CreateDelegate<Func<T, T, bool>>();
    }
    
    [Test]
    public void Operators_ValueType()
    {
        var gt = CreateBinaryComparisonMethod<int>(nameof(Operators_ValueType) + "_GT",
            (l, r) => l > r);
        var ge = CreateBinaryComparisonMethod<int>(nameof(Operators_ValueType) + "_GE",
            (l, r) => l >= r);
        var lt = CreateBinaryComparisonMethod<int>(nameof(Operators_ValueType) + "_LT",
            (l, r) => l < r);
        var le = CreateBinaryComparisonMethod<int>(nameof(Operators_ValueType) + "_LE",
            (l, r) => l <= r);
        var eq = CreateBinaryComparisonMethod<int>(nameof(Operators_ValueType) + "_EQ",
            (l, r) => l.IsEqualTo(r));
        var ne = CreateBinaryComparisonMethod<int>(nameof(Operators_ValueType) + "_NE",
            (l, r) => l.IsNotEqualTo(r));

        
        var a = TestContext.CurrentContext.Random.Next();
        var b = TestContext.CurrentContext.Random.Next();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(gt(a, b), Is.EqualTo(a > b));
            Assert.That(ge(a, b), Is.EqualTo(a >= b));
            Assert.That(lt(a, b), Is.EqualTo(a < b));
            Assert.That(le(a, b), Is.EqualTo(a <= b));
            Assert.That(eq(a, b), Is.EqualTo(a == b));
            Assert.That(ne(a, b), Is.EqualTo(a != b));
            
            Assert.That(gt(b, a), Is.EqualTo(b > a));
            Assert.That(ge(b, a), Is.EqualTo(b >= a));
            Assert.That(lt(b, a), Is.EqualTo(b < a));
            Assert.That(le(b, a), Is.EqualTo(b <= a));
            Assert.That(eq(b, a), Is.EqualTo(b == a));
            Assert.That(ne(b, a), Is.EqualTo(b != a));
        }
    }
}