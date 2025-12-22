using EmitToolbox.Extensions;
using EmitToolbox.Symbols;

namespace EmitToolbox.Test.Extensions;

[TestFixture, TestOf(typeof(LoopBlock))]
public class TestLoopBlock
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    [Test]
    public void Loop_While()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int>("Loop_While", [typeof(int)]);
        var argument = method.Argument<int>(0);
        var variable = method.Variable<int>();
        variable.AssignContent(method.Value(0));
        using (method.While(variable < argument))
        {
            variable.AssignContent(variable + method.Value(1));
        }

        method.Return(variable);
        type.Build();

        var functor = method.BuildingMethod.CreateDelegate<Func<int, int>>();
        var testNumber = TestContext.CurrentContext.Random.Next(1, 100);
        Assert.That(functor(testNumber), Is.EqualTo(testNumber));
    }

    [Test]
    public void Loop_Continue()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int>("Loop_Continue", [typeof(int)]);
        var argument = method.Argument<int>(0);
        var current = method.Variable<int>();
        current.AssignValue(0);
        var count = method.Variable<int>();
        count.AssignValue(0);
        using (var loop = method.While(current < argument))
        {
            using (method.If((current % 2).IsEqualTo(0)))
            {
                current.AssignContent(current + method.Value(1));
                loop.Continue();
            }

            current.AssignContent(current + method.Value(1));
            count.AssignContent(count + method.Value(1));
        }

        method.Return(count);
        type.Build();

        var functor = method.BuildingMethod.CreateDelegate<Func<int, int>>();
        var testNumber = TestContext.CurrentContext.Random.Next(1, 100);
        Assert.That(functor(testNumber), Is.EqualTo(
            Enumerable.Range(0, testNumber).Count(x => x % 2 != 0)));
    }

    [Test]
    public void Loop_Break()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int>("Loop_Break",
            [typeof(int), typeof(int)]);
        var argumentStart = method.Argument<int>(0);
        var argumentEnd = method.Argument<int>(1);
        var current = method.Variable<int>();
        current.AssignContent(argumentStart);
        var count = method.Variable<int>();
        count.AssignContent(method.Value(0));
        using (var loop = method.While(current < argumentEnd))
        {
            using (method.If(current
                       .Modulus(method.Value(7))
                       .IsEqualTo(method.Value(0))))
            {
                loop.Break();
            }

            current.AssignContent(current + method.Value(1));
            count.AssignContent(count + method.Value(1));
        }

        method.Return(count);
        type.Build();

        var functor = method.BuildingMethod.CreateDelegate<Func<int, int, int>>();
        var testStart = TestContext.CurrentContext.Random.Next(50, 100);
        var testCount = TestContext.CurrentContext.Random.Next(1, 10);
        Assert.That(functor(testStart, testStart + testCount), Is.EqualTo(
            Enumerable
                .Range(testStart, testCount)
                .TakeWhile(x => x % 7 != 0)
                .Count()));
    }

    [Test]
    public void Loop_Continue_Conditional()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int>("Loop_Continue", [typeof(int)]);
        var argument = method.Argument<int>(0);
        var current = method.Variable<int>();
        current.AssignContent(method.Value(0));
        var count = method.Variable<int>();
        count.AssignContent(method.Value(0));
        using (var loop = method.While(current < argument))
        {
            current.AssignContent(current + method.Value(1));

            loop.ContinueIfTrue(
                current.Subtract(method.Value(1))
                    .Modulus(method.Value(2))
                    .IsEqualTo(method.Value(0)));

            current.AssignContent(current + method.Value(1));
            count.AssignContent(count + method.Value(1));
        }

        method.Return(count);
        type.Build();

        var functor = method.BuildingMethod.CreateDelegate<Func<int, int>>();
        var testNumber = TestContext.CurrentContext.Random.Next(1, 100);
        Assert.That(functor(testNumber), Is.EqualTo(
            Enumerable.Range(0, testNumber).Count(x => x % 2 != 0)));
    }

    [Test]
    public void Loop_Break_Conditional()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int>("Loop_Break",
            [typeof(int), typeof(int)]);
        var argumentStart = method.Argument<int>(0);
        var argumentEnd = method.Argument<int>(1);
        var current = method.Variable<int>();
        current.AssignContent(argumentStart);
        var count = method.Variable<int>();
        count.AssignContent(method.Value(0));
        using (var loop = method.While(current < argumentEnd))
        {
            loop.BreakIfTrue(current
                .Modulus(method.Value(7))
                .IsEqualTo(method.Value(0)));

            current.AssignContent(current + method.Value(1));
            count.AssignContent(count + method.Value(1));
        }

        method.Return(count);
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<int, int, int>>();

        var testStart = TestContext.CurrentContext.Random.Next(50, 100);
        var testCount = TestContext.CurrentContext.Random.Next(1, 10);

        Assert.That(functor(testStart, testStart + testCount),
            Is.EqualTo(Enumerable.Range(testStart, testCount)
                .TakeWhile(testNumber => testNumber % 7 != 0)
                .Count()));
    }
}