using EmitToolbox.Framework;

namespace EmitToolbox.Test.Framework.Symbols;

[TestFixture]
public class TestLiteralSymbol
{
    private static AssemblyBuildingContext _assembly;

    [SetUp]
    public void Initialize()
    {
        _assembly = AssemblyBuildingContext
            .CreateExecutableContextBuilder("TestLiteralSymbol")
            .Build();
    }

    public FunctorMethodBuildingContext CreateMethodContext<TValue>()
    {
        var typeContext = _assembly.DefineClass("TestLiteralSymbol_" + typeof(TValue).Name);
        return typeContext.DefineStaticFunctor("Test", [], ResultDefinition.Value<TValue>());
    }

    [Test]
    public void TestLiteralSymbol_String()
    {
        var methodContext = CreateMethodContext<string>();
        var value = TestContext.CurrentContext.Random.GetString();
        methodContext.Return(methodContext.Value(value));
        methodContext.TypeContext.Build();
        Assert.That(methodContext.BuildingMethod.Invoke(null, null),
            Is.EqualTo(value));
    }

    [Test]
    public void TestLiteralSymbol_Boolean()
    {
        var methodContext = CreateMethodContext<bool>();
        var value = TestContext.CurrentContext.Random.NextBool();
        methodContext.Return(methodContext.Value(value));
        methodContext.TypeContext.Build();
        Assert.That(methodContext.BuildingMethod.Invoke(null, null),
            Is.EqualTo(value));
    }

    [Test]
    public void TestLiteralSymbol_SByte()
    {
        var methodContext = CreateMethodContext<sbyte>();
        var value = TestContext.CurrentContext.Random.NextSByte();
        methodContext.Return(methodContext.Value(value));
        methodContext.TypeContext.Build();
        Assert.That(methodContext.BuildingMethod.Invoke(null, null), Is.EqualTo(value));
    }

    [Test]
    public void TestLiteralSymbol_Byte()
    {
        var methodContext = CreateMethodContext<byte>();
        var value = TestContext.CurrentContext.Random.NextByte();
        methodContext.Return(methodContext.Value(value));
        methodContext.TypeContext.Build();
        Assert.That(methodContext.BuildingMethod.Invoke(null, null), Is.EqualTo(value));
    }

    [Test]
    public void TestLiteralSymbol_Short()
    {
        var methodContext = CreateMethodContext<short>();
        var value = TestContext.CurrentContext.Random.NextShort();
        methodContext.Return(methodContext.Value(value));
        methodContext.TypeContext.Build();
        Assert.That(methodContext.BuildingMethod.Invoke(null, null), Is.EqualTo(value));
    }

    [Test]
    public void TestLiteralSymbol_UShort()
    {
        var methodContext = CreateMethodContext<ushort>();
        var value = TestContext.CurrentContext.Random.NextUShort();
        methodContext.Return(methodContext.Value(value));
        methodContext.TypeContext.Build();
        Assert.That(methodContext.BuildingMethod.Invoke(null, null), Is.EqualTo(value));
    }

    [Test]
    public void TestLiteralSymbol_Int()
    {
        var methodContext = CreateMethodContext<int>();
        var value = TestContext.CurrentContext.Random.Next();
        methodContext.Return(methodContext.Value(value));
        methodContext.TypeContext.Build();
        Assert.That(methodContext.BuildingMethod.Invoke(null, null), Is.EqualTo(value));
    }

    [Test]
    public void TestLiteralSymbol_UInt()
    {
        var methodContext = CreateMethodContext<uint>();
        var value = TestContext.CurrentContext.Random.NextUInt();
        methodContext.Return(methodContext.Value(value));
        methodContext.TypeContext.Build();
        Assert.That(methodContext.BuildingMethod.Invoke(null, null), Is.EqualTo(value));
    }

    [Test]
    public void TestLiteralSymbol_Long()
    {
        var methodContext = CreateMethodContext<long>();
        var value = TestContext.CurrentContext.Random.NextLong();
        methodContext.Return(methodContext.Value(value));
        methodContext.TypeContext.Build();
        Assert.That(methodContext.BuildingMethod.Invoke(null, null), Is.EqualTo(value));
    }

    [Test]
    public void TestLiteralSymbol_ULong()
    {
        var methodContext = CreateMethodContext<ulong>();
        var value = TestContext.CurrentContext.Random.NextULong();
        methodContext.Return(methodContext.Value(value));
        methodContext.TypeContext.Build();
        Assert.That(methodContext.BuildingMethod.Invoke(null, null), Is.EqualTo(value));
    }
    
    [Test]
    public void TestLiteralSymbol_Float()
    {
        var methodContext = CreateMethodContext<float>();
        var value = TestContext.CurrentContext.Random.NextFloat();
        methodContext.Return(methodContext.Value(value));
        methodContext.TypeContext.Build();
        Assert.That(methodContext.BuildingMethod.Invoke(null, null), Is.EqualTo(value));
    }
    
    [Test]
    public void TestLiteralSymbol_Double()
    {
        var methodContext = CreateMethodContext<double>();
        var value = TestContext.CurrentContext.Random.NextDouble();
        methodContext.Return(methodContext.Value(value));
        methodContext.TypeContext.Build();
        Assert.That(methodContext.BuildingMethod.Invoke(null, null), Is.EqualTo(value));
    }

    public enum TestEnum
    {
        A, B, C, D
    }
    
    [Test]
    public void TestLiteralSymbol_Enum()
    {
        var methodContext = CreateMethodContext<TestEnum>();
        var value = TestContext.CurrentContext.Random.NextEnum<TestEnum>();
        methodContext.Return(methodContext.Value(value));
        methodContext.TypeContext.Build();
        Assert.That(methodContext.BuildingMethod.Invoke(null, null), Is.EqualTo(value));
    }
}