using EmitToolbox.Symbols;

namespace EmitToolbox.Test.Extensions;

[TestFixture]
public class TestReferenceExtensions
{
    private DynamicAssembly _assembly;
    
    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }
    
    public delegate TResult DeferenceDelegate<TResult>(ref TResult value);
    
    private DeferenceDelegate<TResult> CreateTestMethod<TResult>(string name)
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<TResult>(
            name, 
            [typeof(TResult).MakeByRefType()]);
        var argument = method.Argument<TResult>(0, ContentModifier.Reference);
        method.Return(argument);
        type.Build();
        return method.BuildingMethod.CreateDelegate<DeferenceDelegate<TResult>>();
    }

    [Test]
    public void Dereference_Int()
    {
        var method = CreateTestMethod<int>(
            nameof(Dereference_Int));
        var value = TestContext.CurrentContext.Random.Next();
        Assert.That(method(ref value), Is.EqualTo(value));
    }
    
    [Test]
    public void Dereference_UInt()
    {
        var method = CreateTestMethod<uint>(
            nameof(Dereference_UInt));
        var value = TestContext.CurrentContext.Random.NextUInt();
        Assert.That(method(ref value), Is.EqualTo(value));
    }

    [Test]
    public void Dereference_Short()
    {
        var method = CreateTestMethod<short>(
            nameof(Dereference_Short));
        var value = TestContext.CurrentContext.Random.NextShort();
        Assert.That(method(ref value), Is.EqualTo(value));
    }

    [Test]
    public void Dereference_UShort()
    {
        var method = CreateTestMethod<ushort>(
            nameof(Dereference_UShort));
        var value = TestContext.CurrentContext.Random.NextUShort();
        Assert.That(method(ref value), Is.EqualTo(value));
    }

    [Test]
    public void Dereference_Byte()
    {
        var method = CreateTestMethod<byte>(
            nameof(Dereference_Byte));
        var value = TestContext.CurrentContext.Random.NextByte();
        Assert.That(method(ref value), Is.EqualTo(value));
    }

    [Test]
    public void Dereference_SByte()
    {
        var method = CreateTestMethod<sbyte>(
            nameof(Dereference_SByte));
        var value = TestContext.CurrentContext.Random.NextSByte();
        Assert.That(method(ref value), Is.EqualTo(value));
    }

    [Test]
    public void Dereference_Char()
    {
        var method = CreateTestMethod<char>(
            nameof(Dereference_Char));
        var value = (char)TestContext.CurrentContext.Random.Next(char.MinValue, char.MaxValue);
        Assert.That(method(ref value), Is.EqualTo(value));
    }

    [Test]
    public void Dereference_Long()
    {
        var method = CreateTestMethod<long>(
            nameof(Dereference_Long));
        var value = TestContext.CurrentContext.Random.NextLong();
        Assert.That(method(ref value), Is.EqualTo(value));
    }

    [Test]
    public void Dereference_ULong()
    {
        var method = CreateTestMethod<ulong>(
            nameof(Dereference_ULong));
        var value = TestContext.CurrentContext.Random.NextULong();
        Assert.That(method(ref value), Is.EqualTo(value));
    }

    [Test]
    public void Dereference_Float()
    {
        var method = CreateTestMethod<float>(
            nameof(Dereference_Float));
        var value = TestContext.CurrentContext.Random.NextFloat();
        Assert.That(method(ref value), Is.EqualTo(value));
    }

    [Test]
    public void Dereference_Double()
    {
        var method = CreateTestMethod<double>(
            nameof(Dereference_Double));
        var value = TestContext.CurrentContext.Random.NextDouble();
        Assert.That(method(ref value), Is.EqualTo(value));
    }

    [Test]
    public void Dereference_IntPtr()
    {
        var method = CreateTestMethod<IntPtr>(
            nameof(Dereference_IntPtr));
        var value = new IntPtr(TestContext.CurrentContext.Random.Next());
        Assert.That(method(ref value), Is.EqualTo(value));
    }

    [Test]
    public void Dereference_UIntPtr()
    {
        var method = CreateTestMethod<UIntPtr>(
            nameof(Dereference_UIntPtr));
        var value = new UIntPtr((uint)TestContext.CurrentContext.Random.Next());
        Assert.That(method(ref value), Is.EqualTo(value));
    }

    [Test]
    public void Dereference_String()
    {
        var method = CreateTestMethod<string>(
            nameof(Dereference_String));
        var value = TestContext.CurrentContext.Random.GetString();
        Assert.That(method(ref value), Is.EqualTo(value));
    }

    [Test]
    public void Dereference_Object()
    {
        var method = CreateTestMethod<object>(
            nameof(Dereference_Object));
        var value = new object();
        Assert.That(method(ref value), Is.EqualTo(value));
    }
}