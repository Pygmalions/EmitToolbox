using EmitToolbox.Framework;
using EmitToolbox.Framework.Extensions;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Test.Framework.Extensions;

[TestFixture, TestOf(typeof(NullabilityExtensions))]
public class TestNullabilityExtensions
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    private Func<TIn, TOut> CreateMethod<TIn, TOut>(
        string name, 
        Func<ISymbol<TIn>, ISymbol<TOut>> transform)
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<TOut>(
            name,
            [typeof(TIn)]);
        var argument = method.Argument<TIn>(0);
        method.Return(transform(argument));
        type.Build();
        return method.BuildingMethod.CreateDelegate<Func<TIn, TOut>>();
    }

    [Test]
    public void IsNull_ReferenceType_TrueAndFalse()
    {
        var functor = CreateMethod<string, bool>(
            nameof(IsNull_ReferenceType_TrueAndFalse), 
            symbol => symbol.IsNull());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(functor(null!), Is.True);
            Assert.That(functor(string.Empty), Is.False);
        }
    }

    [Test]
    public void IsNotNull_ReferenceType_TrueAndFalse()
    {
        var functor = CreateMethod<string, bool>(
            nameof(IsNotNull_ReferenceType_TrueAndFalse), symbol => symbol.IsNotNull());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(functor(null!), Is.False);
            Assert.That(functor(string.Empty), Is.True);
        }
    }
    
    [Test]
    public void HasNotNullValue_ReferenceType_TrueAndFalse()
    {
        var functor = CreateMethod<string, bool>(
            nameof(HasNotNullValue_ReferenceType_TrueAndFalse), 
            symbol => symbol.HasNotNullValue());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(functor(null!), Is.False);
            Assert.That(functor(string.Empty), Is.True);
        }
    }

    [Test]
    public void HasNotNullValue_ValueType_TrueAndFalse()
    {
        var functor = CreateMethod<int?, bool>(
            nameof(HasNotNullValue_ValueType_TrueAndFalse), 
            symbol => symbol.HasNotNullValue());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(functor(null), Is.False);
            Assert.That(functor(1), Is.True);
        }
    }

    [Test]
    public void HasValue_NullableValueType_TrueAndFalse()
    {
        var functor = CreateMethod<int?, bool>(
            nameof(HasValue_NullableValueType_TrueAndFalse), symbol => symbol.HasValue());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(functor(null), Is.False);
            Assert.That(functor(1), Is.True);
        }
    }

    [Test]
    public void GetValue_NullableValueType_TrueAndFalse()
    {
        var functor = CreateMethod<int?, int>(
            nameof(GetValue_NullableValueType_TrueAndFalse), symbol => symbol.GetValue());
        var number = TestContext.CurrentContext.Random.Next();
        int? value = number;
        var result = functor(value);
        Assert.That(result, Is.EqualTo(number));
    }

    [Test]
    public void ToNullable_ValueType_WrapValue()
    {
        var functor = CreateMethod<int, int?>(nameof(ToNullable_ValueType_WrapValue), n => n.ToNullable());
        var number = TestContext.CurrentContext.Random.Next();
        var result = functor(number);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.HasValue, Is.True);
            Assert.That(result!.Value, Is.EqualTo(number));
        }
    }
}