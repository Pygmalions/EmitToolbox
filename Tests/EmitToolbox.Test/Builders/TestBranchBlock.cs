using EmitToolbox.Builders;
using EmitToolbox.Extensions;
using EmitToolbox.Symbols;

namespace EmitToolbox.Test.Builders;

[TestFixture, TestOf(typeof(BranchBlock))]
public class TestBranchBlock
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    [Test]
    public void Branch_If()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<bool>("Branch", [typeof(int)]);
        var argument = method.Argument<int>(0);
        using (method.If(argument.IsEqualTo(method.Literal(0))))
        {
            method.Return(method.Literal(true));
        }

        method.Return(method.Literal(false));
        type.Build();

        var functor = method.BuildingMethod.CreateDelegate<Func<int, bool>>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(functor(0), Is.True);
            Assert.That(functor(1), Is.False);
        }
    }

    [Test]
    public void Branch_IfNot()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<bool>("Branch", [typeof(int)]);
        var argument = method.Argument<int>(0);
        using (method.IfNot(argument.IsEqualTo(method.Literal(0))))
        {
            method.Return(method.Literal(true));
        }

        method.Return(method.Literal(false));
        type.Build();

        var functor = method.BuildingMethod.CreateDelegate<Func<int, bool>>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(functor(0), Is.False);
            Assert.That(functor(1), Is.True);
        }
    }

    [Test]
    public void Branch_If_Else()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int>("Branch", [typeof(int)]);
        var argument = method.Argument<int>(0);
        method.IfElse(argument.IsEqualTo(method.Literal(0)),
            () => { method.Return(method.Literal(1)); },
            () => { method.Return(method.Literal(2)); });
        method.Return(method.Literal(3));
        type.Build();

        var functor = method.BuildingMethod.CreateDelegate<Func<int, int>>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(functor(0), Is.EqualTo(1));
            Assert.That(functor(1), Is.EqualTo(2));
        }
    }
}