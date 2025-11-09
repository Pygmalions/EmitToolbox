using EmitToolbox.Framework;
using EmitToolbox.Framework.Extensions;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Test.Framework.Extensions;

[TestFixture, TestOf(typeof(BooleanExtensions))]
public class TestBooleanExtensions
{
    private DynamicAssembly _assembly;
    
    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }
    
    [Test]
    public void Boolean_And()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<bool>(
            nameof(Boolean_And), [typeof(bool), typeof(bool)]);
        var argumentA = method.Argument<bool>(0);
        var argumentB = method.Argument<bool>(1);
        method.Return(argumentA.And(argumentB));
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<bool, bool, bool>>();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(functor(true, true), Is.True);
            Assert.That(functor(true, false), Is.False);
            Assert.That(functor(false, true), Is.False);
            Assert.That(functor(false, false), Is.False);
        }
    }
    
    [Test]
    public void Boolean_Or()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<bool>(
            nameof(Boolean_And), [typeof(bool), typeof(bool)]);
        var argumentA = method.Argument<bool>(0);
        var argumentB = method.Argument<bool>(1);
        method.Return(argumentA.Or(argumentB));
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<bool, bool, bool>>();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(functor(true, true), Is.True);
            Assert.That(functor(true, false), Is.True);
            Assert.That(functor(false, true), Is.True);
            Assert.That(functor(false, false), Is.False);
        }
    }
}