using EmitToolbox.Extensions;
using EmitToolbox.Symbols;

namespace EmitToolbox.Test.Extensions;

[TestFixture, TestOf(typeof(ValueTupleExtensions))]
public class TestValueTupleExtensions
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    [Test]
    public void New_ValueTuple_TwoParameters()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<ValueTuple<int, int>>(
            nameof(New_ValueTuple_TwoParameters),
            [typeof(int), typeof(int)]);
        var argumentA = method.Argument<int>(0);
        var argumentB = method.Argument<int>(1);
        method.Return(method.NewValueTuple(argumentA, argumentB));

        type.Build();

        var functor = method.BuildingMethod.CreateDelegate<Func<int, int, ValueTuple<int, int>>>();
        var testA = TestContext.CurrentContext.Random.Next();
        var testB = TestContext.CurrentContext.Random.Next();
        Assert.That(functor(testA, testB), Is.EqualTo((testA, testB)));
    }

    [Test]
    public void New_ValueTuple_ThreeParameters()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<ValueTuple<int, int, int>>(
            nameof(New_ValueTuple_ThreeParameters),
            [typeof(int), typeof(int), typeof(int)]);
        var argumentA = method.Argument<int>(0);
        var argumentB = method.Argument<int>(1);
        var argumentC = method.Argument<int>(2);
        method.Return(method.NewValueTuple(argumentA, argumentB, argumentC));

        type.Build();

        var functor = method.BuildingMethod.CreateDelegate<Func<int, int, int, ValueTuple<int, int, int>>>();
        var testA = TestContext.CurrentContext.Random.Next();
        var testB = TestContext.CurrentContext.Random.Next();
        var testC = TestContext.CurrentContext.Random.Next();
        Assert.That(functor(testA, testB, testC), Is.EqualTo((testA, testB, testC)));
    }

    [Test]
    public void New_ValueTuple_FourParameters()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<ValueTuple<int, int, int, int>>(
            nameof(New_ValueTuple_FourParameters),
            [typeof(int), typeof(int), typeof(int), typeof(int)]);
        var argumentA = method.Argument<int>(0);
        var argumentB = method.Argument<int>(1);
        var argumentC = method.Argument<int>(2);
        var argumentD = method.Argument<int>(3);
        method.Return(method.NewValueTuple(argumentA, argumentB, argumentC, argumentD));

        type.Build();

        var functor = method.BuildingMethod.CreateDelegate<Func<int, int, int, int, ValueTuple<int, int, int, int>>>();
        var testA = TestContext.CurrentContext.Random.Next();
        var testB = TestContext.CurrentContext.Random.Next();
        var testC = TestContext.CurrentContext.Random.Next();
        var testD = TestContext.CurrentContext.Random.Next();
        Assert.That(functor(testA, testB, testC, testD), Is.EqualTo((testA, testB, testC, testD)));
    }

    [Test]
    public void New_ValueTuple_FiveParameters()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<ValueTuple<int, int, int, int, int>>(
            nameof(New_ValueTuple_FiveParameters),
            [typeof(int), typeof(int), typeof(int), typeof(int), typeof(int)]);
        var argumentA = method.Argument<int>(0);
        var argumentB = method.Argument<int>(1);
        var argumentC = method.Argument<int>(2);
        var argumentD = method.Argument<int>(3);
        var argumentE = method.Argument<int>(4);
        method.Return(method.NewValueTuple(argumentA, argumentB, argumentC, argumentD, argumentE));

        type.Build();

        var functor = method.BuildingMethod
            .CreateDelegate<Func<int, int, int, int, int, ValueTuple<int, int, int, int, int>>>();
        var testA = TestContext.CurrentContext.Random.Next();
        var testB = TestContext.CurrentContext.Random.Next();
        var testC = TestContext.CurrentContext.Random.Next();
        var testD = TestContext.CurrentContext.Random.Next();
        var testE = TestContext.CurrentContext.Random.Next();
        Assert.That(functor(testA, testB, testC, testD, testE), Is.EqualTo((testA, testB, testC, testD, testE)));
    }
}