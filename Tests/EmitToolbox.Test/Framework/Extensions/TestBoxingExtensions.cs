using EmitToolbox.Extensions;
using EmitToolbox.Symbols;

namespace EmitToolbox.Test.Framework.Extensions;

[TestFixture, TestOf(typeof(BoxingExtensions))]
public class TestBoxingExtensions
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    [Test]
    public void Box_Int()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<object>(
            nameof(Box_Int), [typeof(int)]);
        var value = method.Argument<int>(0);
        method.Return(BoxingExtensions.Box<int>(value));
        type.Build();
        var func = method.BuildingMethod.CreateDelegate<Func<int, object>>();

        var number = TestContext.CurrentContext.Random.Next();
        var result = func(number);
        Assert.That(result, Is.InstanceOf<object>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.TypeOf<int>()); // boxed int
            Assert.That((int)result, Is.EqualTo(number));
        }
    }

    [Test]
    public void ToObject_ValueType_Int()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<object>(
            nameof(ToObject_ValueType_Int), [typeof(int)]);
        var value = method.Argument<int>(0);
        method.Return(value.ToObject());
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<int, object>>();

        var number = TestContext.CurrentContext.Random.Next();
        var result = functor(number);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.TypeOf<int>());
            Assert.That((int)result, Is.EqualTo(number));
        }
    }

    [Test]
    public void ToObject_ReferenceType_String()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<object>(
            nameof(ToObject_ReferenceType_String), [typeof(string)]);
        var value = method.Argument<string>(0);
        method.Return(value.ToObject());
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<string, object>>();

        var text = TestContext.CurrentContext.Random.GetString();
        var result = functor(text);
        Assert.That(ReferenceEquals(result, text), Is.True);
    }

    [Test]
    public void Unbox_Int()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int>(
            nameof(Unbox_Int), [typeof(object)]);
        var value = method.Argument<object>(0);
        method.Return(BoxingExtensions.Unbox<int>(value));
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<object, int>>();

        var number = TestContext.CurrentContext.Random.Next();
        var boxed = (object)number;
        var result = functor(boxed);
        Assert.That(result, Is.EqualTo(number));
    }

    [Test]
    public void Unbox_Int_AsReference()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int>(
            nameof(Unbox_Int_AsReference), [typeof(object)]);
        var value = method.Argument<object>(0);
        // 'Return' automatically dereferences the address.
        method.Return(BoxingExtensions.Unbox<int>(value, true));
        type.Build();
        var func = method.BuildingMethod.CreateDelegate<Func<object, int>>();

        var number = TestContext.CurrentContext.Random.Next();
        var result = func(number);
        Assert.That(result, Is.EqualTo(number));
    }

    [Test]
    public void RoundTrip_Box_Unbox_Int()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int>(
            nameof(RoundTrip_Box_Unbox_Int), [typeof(int)]);
        var value = method.Argument<int>(0);
        method.Return(BoxingExtensions.Box<int>(value).Unbox<int>());
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<int, int>>();

        var number = TestContext.CurrentContext.Random.Next();
        var result = functor(number);
        Assert.That(result, Is.EqualTo(number));
    }
}
