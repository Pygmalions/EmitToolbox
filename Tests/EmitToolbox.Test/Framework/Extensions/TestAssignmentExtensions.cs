using EmitToolbox.Framework;
using EmitToolbox.Framework.Extensions;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Test.Framework.Extensions;

[TestFixture, TestOf(typeof(AssignmentExtensions))]
public class TestAssignmentExtensions
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    [Test]
    public void CopyValueFrom_ValueType_Int()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int>(
            nameof(CopyValueFrom_ValueType_Int), [typeof(int)]);
        var value = method.Argument<int>(0);
        var local = method.Variable<int>();
        local.AssignValue(value);
        method.Return(local);
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<int, int>>();

        var number = TestContext.CurrentContext.Random.Next();
        var result = functor(number);
        Assert.That(result, Is.EqualTo(number));
    }

    [Test]
    public void CopyValueFrom_ReferenceType_String()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<string>(
            nameof(CopyValueFrom_ReferenceType_String), [typeof(string)]);
        var value = method.Argument<string>(0);
        var local = method.Variable<string>();
        local.AssignValue(value);
        method.Return(local);
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<string, string>>();

        var text = TestContext.CurrentContext.Random.GetString();
        var result = functor(text);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.EqualTo(text));
            Assert.That(ReferenceEquals(result, text), Is.True);
        }
    }

    [Test]
    public void CopyValueFrom_NonAddressableSource_ToAddressableDestination_Int()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int>(
           "CopyValue", [typeof(int), typeof(int)]);
        var a = method.Argument<int>(0);
        var b = method.Argument<int>(1);
        // a.Add(b) creates an OperationSymbol<int> which is not addressable.
        var local = method.Variable<int>();
        local.AssignValue(a + b);
        method.Return(local);
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<int, int, int>>();
        var x = TestContext.CurrentContext.Random.Next();
        var y = TestContext.CurrentContext.Random.Next();
        var result = functor(x, y);
        Assert.That(result, Is.EqualTo(x + y));
    }
}
