using EmitToolbox.Framework;
using EmitToolbox.Framework.Extensions;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Test.Framework.Extensions;

[TestFixture, TestOf(typeof(MathOperationExtensions))]
public class TestMathOperationExtensions
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    private Func<int, int, int> CreateBinaryMathMethod(
        string name, Func<ISymbol<int>, ISymbol<int>, ISymbol<int>> op)
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int>(name, [typeof(int), typeof(int)]);
        var left = method.Argument<int>(0);
        var right = method.Argument<int>(1);
        method.Return(op(left, right));
        type.Build();
        return method.BuildingMethod.CreateDelegate<Func<int, int, int>>();
    }

    [Test]
    public void Operators_ValueType_Int_Add_Sub_Mul_Div_Mod()
    {
        var add = CreateBinaryMathMethod(nameof(Operators_ValueType_Int_Add_Sub_Mul_Div_Mod) + "_Add",
            (l, r) => l + r);
        var sub = CreateBinaryMathMethod(nameof(Operators_ValueType_Int_Add_Sub_Mul_Div_Mod) + "_Sub",
            (l, r) => l - r);
        var mul = CreateBinaryMathMethod(nameof(Operators_ValueType_Int_Add_Sub_Mul_Div_Mod) + "_Mul",
            (l, r) => l * r);
        var div = CreateBinaryMathMethod(nameof(Operators_ValueType_Int_Add_Sub_Mul_Div_Mod) + "_Div",
            (l, r) => l / r);
        var mod = CreateBinaryMathMethod(nameof(Operators_ValueType_Int_Add_Sub_Mul_Div_Mod) + "_Mod",
            (l, r) => l % r);

        var a = TestContext.CurrentContext.Random.Next(int.MinValue / 2, int.MaxValue / 2);
        var b = TestContext.CurrentContext.Random.Next(1, int.MaxValue / 2); // avoid zero for div/mod

        using (Assert.EnterMultipleScope())
        {
            Assert.That(add(a, b), Is.EqualTo(a + b));
            Assert.That(sub(a, b), Is.EqualTo(a - b));
            Assert.That(mul(a, b), Is.EqualTo(a * b));
            Assert.That(div(a, b), Is.EqualTo(a / b));
            Assert.That(mod(a, b), Is.EqualTo(a % b));
        }
    }

    [Test]
    public void Methods_ValueType_Int_Add_Sub_Mul_Div_Mod()
    {
        var add = CreateBinaryMathMethod(nameof(Methods_ValueType_Int_Add_Sub_Mul_Div_Mod) + "_Add",
            (l, r) => l.Add(r));
        var sub = CreateBinaryMathMethod(nameof(Methods_ValueType_Int_Add_Sub_Mul_Div_Mod) + "_Sub",
            (l, r) => l.Subtract(r));
        var mul = CreateBinaryMathMethod(nameof(Methods_ValueType_Int_Add_Sub_Mul_Div_Mod) + "_Mul",
            (l, r) => l.Multiply(r));
        var div = CreateBinaryMathMethod(nameof(Methods_ValueType_Int_Add_Sub_Mul_Div_Mod) + "_Div",
            (l, r) => l.Divide(r));
        var mod = CreateBinaryMathMethod(nameof(Methods_ValueType_Int_Add_Sub_Mul_Div_Mod) + "_Mod",
            (l, r) => l.Modulus(r));

        var a = TestContext.CurrentContext.Random.Next(int.MinValue / 2, int.MaxValue / 2);
        var b = TestContext.CurrentContext.Random.Next(1, int.MaxValue / 2); // avoid zero for div/mod

        using (Assert.EnterMultipleScope())
        {
            Assert.That(add(a, b), Is.EqualTo(a + b));
            Assert.That(sub(a, b), Is.EqualTo(a - b));
            Assert.That(mul(a, b), Is.EqualTo(a * b));
            Assert.That(div(a, b), Is.EqualTo(a / b));
            Assert.That(mod(a, b), Is.EqualTo(a % b));
        }
    }

    [Test]
    public void Checked_Add_Sub_Mul_NoOverflow()
    {
        var add = CreateBinaryMathMethod(nameof(Checked_Add_Sub_Mul_NoOverflow) + "_Add",
            (l, r) => l.CheckedAdd(r));
        var sub = CreateBinaryMathMethod(nameof(Checked_Add_Sub_Mul_NoOverflow) + "_Sub",
            (l, r) => l.CheckedSubtract(r));
        var mul = CreateBinaryMathMethod(nameof(Checked_Add_Sub_Mul_NoOverflow) + "_Mul",
            (l, r) => l.CheckedMultiply(r));

        var a = 1000;
        var b = 2000;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(add(a, b), Is.EqualTo(checked(a + b)));
            Assert.That(sub(a, b), Is.EqualTo(checked(a - b)));
            Assert.That(mul(a, b), Is.EqualTo(checked(a * b)));
        }
    }

    [Test]
    public void Checked_Add_Overflow_Throws()
    {
        var add = CreateBinaryMathMethod(nameof(Checked_Add_Overflow_Throws), (l, r) => l.CheckedAdd(r));
        Assert.Throws<OverflowException>(() => add(int.MaxValue, 1));
    }

    [Test]
    public void Checked_Sub_Overflow_Throws()
    {
        var sub = CreateBinaryMathMethod(nameof(Checked_Sub_Overflow_Throws), (l, r) => l.CheckedSubtract(r));
        Assert.Throws<OverflowException>(() => sub(int.MinValue, 1));
    }

    [Test]
    public void Checked_Mul_Overflow_Throws()
    {
        var mul = CreateBinaryMathMethod(nameof(Checked_Mul_Overflow_Throws), (l, r) => l.CheckedMultiply(r));
        Assert.Throws<OverflowException>(() => mul(int.MaxValue, 2));
    }

    [Test]
    public void Division_By_Zero_Throws()
    {
        var div = CreateBinaryMathMethod(nameof(Division_By_Zero_Throws), (l, r) => l / r);
        var a = TestContext.CurrentContext.Random.Next();
        Assert.Throws<DivideByZeroException>(() => div(a, 0));
    }
}