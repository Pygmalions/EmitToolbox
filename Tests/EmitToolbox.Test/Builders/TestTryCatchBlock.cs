using EmitToolbox.Builders;
using EmitToolbox.Extensions;
using EmitToolbox.Symbols;

namespace EmitToolbox.Test.Builders;

[TestFixture, TestOf(typeof(TryCatchBlock))]
public class TestTryCatchBlock
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    [Test]
    public void TryAndCatch_MatchExceptionType_Catch()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<Exception?>(
            nameof(Test), [typeof(Action)]);
        var argumentAction = method.Argument<Action>(0);
        var variableException = method.Variable<Exception?>();

        using (var scope = method.Try())
        {
            argumentAction.Invoke(target => target.Invoke());

            using (scope.Catch<InvalidOperationException>(out var symbol))
            {
                variableException.AssignContent(symbol);
            }
        }

        method.Return(variableException);

        type.Build();

        var functor = method.BuildingMethod.CreateDelegate<Func<Action, Exception?>>();

        Assert.That(functor(() => throw new InvalidOperationException()),
            Is.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void TryAndCatch_MismatchExceptionType_DoNotCatch()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<Exception?>(
            nameof(Test), [typeof(Action)]);
        var argumentAction = method.Argument<Action>(0);
        var variableException = method.Variable<Exception?>();

        using (var scope = method.Try())
        {
            argumentAction.Invoke(target => target.Invoke());

            using (scope.Catch<InvalidOperationException>(out var symbol))
            {
                variableException.AssignContent(symbol);
            }
        }

        method.Return(variableException);

        type.Build();

        var functor = method.BuildingMethod.CreateDelegate<Func<Action, Exception?>>();

        Assert.Throws<InvalidCastException>(() => functor(() => throw new InvalidCastException()));
    }

    [Test]
    public void TryAndCatch_MultipleCatch()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<Exception?>(
            nameof(Test), [typeof(Action)]);
        var argumentAction = method.Argument<Action>(0);
        var variableException = method.Variable<Exception?>();

        using (var scope = method.Try())
        {
            argumentAction.Invoke(target => target.Invoke());

            using (scope.Catch<InvalidOperationException>(out var symbol))
            {
                variableException.AssignContent(symbol);
            }

            using (scope.Catch<InvalidCastException>(out var symbol))
            {
                variableException.AssignContent(symbol);
            }
        }

        method.Return(variableException);

        type.Build();

        var functor = method.BuildingMethod.CreateDelegate<Func<Action, Exception?>>();

        using (Assert.EnterMultipleScope())
        {
            Assert.Throws<Exception>(() => functor(() => throw new Exception()));
            Assert.That(functor(() => throw new InvalidCastException()), Is.TypeOf<InvalidCastException>());
            Assert.That(functor(() => throw new InvalidOperationException()), Is.TypeOf<InvalidOperationException>());
        }
    }

    [Test]
    public void TryAndCatchAndFinally_WithoutException_FinallyExecuted()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int>(
            nameof(Test), [typeof(Action)]);
        var argumentAction = method.Argument<Action>(0);
        var variableState = method.Variable<int>();
        variableState.AssignValue(0);
        using (var scope = method.Try())
        {
            argumentAction.Invoke(target => target.Invoke());

            using (scope.Catch<InvalidOperationException>(out var symbol))
            {
                variableState.AssignValue(1);
            }

            using (scope.Finally())
            {
                variableState.AssignValue(2);
            }
        }

        method.Return(variableState);

        type.Build();

        var functor = method.BuildingMethod.CreateDelegate<Func<Action, int>>();

        Assert.That(functor(() => { }), Is.EqualTo(2));
    }

    [Test]
    public void TryAndCatchAndFinally_WithException_FinallyExecuted()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int>(
            nameof(Test), [typeof(Action)]);
        var argumentAction = method.Argument<Action>(0);
        var variableState = method.Variable<int>();
        variableState.AssignValue(0);
        using (var scope = method.Try())
        {
            argumentAction.Invoke(target => target.Invoke());

            using (scope.Catch<InvalidOperationException>(out var symbol))
            {
                variableState.AssignValue(1);
            }

            using (scope.Finally())
            {
                variableState.AssignValue(2);
            }
        }

        method.Return(variableState);

        type.Build();

        var functor = method.BuildingMethod.CreateDelegate<Func<Action, int>>();

        Assert.That(functor(() => { }), Is.EqualTo(2));
    }

    [Test]
    public void TryCatchBlock_DefineMultipleFinallyBlocks_Throws()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int>(
            nameof(Test), [typeof(Action)]);

        Assert.Throws<InvalidOperationException>(() =>
        {
            using var scope = method.Try();
            using (scope.Finally())
            {
            }
            using (scope.Finally())
            {
            }
        });
    }
}