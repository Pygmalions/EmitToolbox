using EmitToolbox.Framework;
using EmitToolbox.Framework.Extensions;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Test.Framework.Extensions;

[TestFixture, TestOf(typeof(EnumExtensions))]
public class TestEnumExtensions
{
    [Flags]
    private enum TestFlags
    {
        // ReSharper disable once UnusedMember.Local
        None = 0,
        A = 1 << 0,
        B = 1 << 1,
        C = 1 << 2,
        // ReSharper disable once InconsistentNaming
        AB = A | B,
        // ReSharper disable once InconsistentNaming
        ABC = A | B | C,
    }

    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    private Func<TestFlags, TestFlags, TestFlags> CreateBinaryEnumMethod(
        string name, Func<ISymbol<TestFlags>, ISymbol<TestFlags>, ISymbol<TestFlags>> op)
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<TestFlags>(name, [typeof(TestFlags), typeof(TestFlags)]);
        var left = method.Argument<TestFlags>(0);
        var right = method.Argument<TestFlags>(1);
        method.Return(op(left, right));
        type.Build();
        return method.BuildingMethod.CreateDelegate<Func<TestFlags, TestFlags, TestFlags>>();
    }

    private Func<TestFlags, TestFlags, bool> CreateBinaryEnumBoolMethod(
        string name, Func<ISymbol<TestFlags>, ISymbol<TestFlags>, ISymbol<bool>> op)
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<bool>(name, [typeof(TestFlags), typeof(TestFlags)]);
        var left = method.Argument<TestFlags>(0);
        var right = method.Argument<TestFlags>(1);
        method.Return(op(left, right));
        type.Build();
        return method.BuildingMethod.CreateDelegate<Func<TestFlags, TestFlags, bool>>();
    }

    private Func<TestFlags, bool> CreateUnaryEnumBoolWithLiteral(
        string name, Func<ISymbol<TestFlags>, ISymbol<bool>> body)
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<bool>(name, [typeof(TestFlags)]);
        var self = method.Argument<TestFlags>(0);
        method.Return(body(self));
        type.Build();
        return method.BuildingMethod.CreateDelegate<Func<TestFlags, bool>>();
    }

    private Func<TestFlags, TestFlags> CreateUnaryEnumWithLiteral(
        string name, Func<ISymbol<TestFlags>, ISymbol<TestFlags>> body)
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<TestFlags>(name, [typeof(TestFlags)]);
        var self = method.Argument<TestFlags>(0);
        method.Return(body(self));
        type.Build();
        return method.BuildingMethod.CreateDelegate<Func<TestFlags, TestFlags>>();
    }

    [Test]
    public void Operators_Enum_Bitwise_Or_And_Xor()
    {
        var or = CreateBinaryEnumMethod(nameof(Operators_Enum_Bitwise_Or_And_Xor) + "_Or", (l, r) => l | r);
        var and = CreateBinaryEnumMethod(nameof(Operators_Enum_Bitwise_Or_And_Xor) + "_And", (l, r) => l & r);
        var xor = CreateBinaryEnumMethod(nameof(Operators_Enum_Bitwise_Or_And_Xor) + "_Xor", (l, r) => l ^ r);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(or(TestFlags.A, TestFlags.B), Is.EqualTo(TestFlags.A | TestFlags.B));
            Assert.That(and(TestFlags.AB, TestFlags.B | TestFlags.C), Is.EqualTo(TestFlags.B));
            Assert.That(xor(TestFlags.AB, TestFlags.B | TestFlags.C), Is.EqualTo(TestFlags.A | TestFlags.C));
        }
    }

    [Test]
    public void Operators_Enum_With_Literals()
    {
        var or = CreateUnaryEnumWithLiteral(nameof(Operators_Enum_With_Literals) + "_Or",
            self => self | TestFlags.C);
        var and = CreateUnaryEnumWithLiteral(nameof(Operators_Enum_With_Literals) + "_And",
            self => self & TestFlags.AB);
        var xor = CreateUnaryEnumWithLiteral(nameof(Operators_Enum_With_Literals) + "_Xor",
            self => self ^ TestFlags.B);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(or(TestFlags.AB), Is.EqualTo(TestFlags.ABC));
            Assert.That(and(TestFlags.ABC), Is.EqualTo(TestFlags.AB));
            Assert.That(xor(TestFlags.AB), Is.EqualTo(TestFlags.A));
        }
    }

    [Test]
    public void Equality_Enum_IsEqual_IsNotEqual()
    {
        var eq = CreateBinaryEnumBoolMethod(nameof(Equality_Enum_IsEqual_IsNotEqual) + "_Eq",
            (l, r) => l.IsEqualTo(r));
        var ne = CreateBinaryEnumBoolMethod(nameof(Equality_Enum_IsEqual_IsNotEqual) + "_Ne",
            (l, r) => l.IsNotEqualTo(r));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(eq(TestFlags.AB, TestFlags.AB), Is.True);
            Assert.That(ne(TestFlags.AB, TestFlags.AB), Is.False);
            Assert.That(eq(TestFlags.AB, TestFlags.C), Is.False);
            Assert.That(ne(TestFlags.AB, TestFlags.C), Is.True);
        }
    }

    [Test]
    public void Equality_Enum_IsEqual_IsNotEqual_With_Literal()
    {
        var eq = CreateUnaryEnumBoolWithLiteral(nameof(Equality_Enum_IsEqual_IsNotEqual_With_Literal) + "_Eq",
            self => self.IsEqualTo(TestFlags.AB));
        var ne = CreateUnaryEnumBoolWithLiteral(nameof(Equality_Enum_IsEqual_IsNotEqual_With_Literal) + "_Ne",
            self => self.IsNotEqualTo(TestFlags.AB));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(eq(TestFlags.AB), Is.True);
            Assert.That(ne(TestFlags.AB), Is.False);
            Assert.That(eq(TestFlags.C), Is.False);
            Assert.That(ne(TestFlags.C), Is.True);
        }
    }

    [Test]
    public void Enum_HasFlag_Symbol_And_Literal()
    {
        var hasFlag = CreateBinaryEnumBoolMethod(
            nameof(Enum_HasFlag_Symbol_And_Literal) + "_Symbol",
            (l, r) => l.HasFlag(r));
        var hasFlagLiteral = CreateUnaryEnumBoolWithLiteral(
            nameof(Enum_HasFlag_Symbol_And_Literal) + "_Literal",
            self => self.HasFlag(TestFlags.B));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(hasFlag(TestFlags.AB, TestFlags.B), Is.True);
            Assert.That(hasFlag(TestFlags.A, TestFlags.B), Is.False);

            Assert.That(hasFlagLiteral(TestFlags.AB), Is.True);
            Assert.That(hasFlagLiteral(TestFlags.A | TestFlags.C), Is.False);
        }
    }
}
