using EmitToolbox.Framework;
using EmitToolbox.Framework.Extensions;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Test.Framework.Extensions;

[TestFixture, TestOf(typeof(ConversionExtensions))]
public class TestConversionExtensions
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    private Func<TFrom, TTo> CreateCastMethod<TFrom, TTo>() where TTo : class
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<TTo>(
            nameof(CreateCastMethod), [typeof(TFrom)]);
        var arg = method.Argument<object>(0);
        method.Return(arg.CastTo<TTo>());
        type.Build();
        return method.BuildingMethod.CreateDelegate<Func<TFrom, TTo>>();
    }
    
    private Func<TFrom, TTo?> CreateTryCastMethod<TFrom, TTo>() where TTo : class
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<TTo?>(
            nameof(CreateTryCastMethod), [typeof(TFrom)]);
        var argument = method.Argument<TFrom>(0);
        method.Return(argument.TryCastTo<TTo>());
        type.Build();
        return method.BuildingMethod.CreateDelegate<Func<TFrom, TTo?>>();
    }

    [Test]
    public void CastTo_Object_To_String()
    {
        var functor = CreateCastMethod<object, string>();
        var text = TestContext.CurrentContext.Random.GetString();
        var result = functor(text);
        Assert.That(result, Is.EqualTo(text));
    }
    
    [Test]
    public void TryCastTo_Object_To_String()
    {
        var functor = CreateTryCastMethod<object, string>();
        var text = TestContext.CurrentContext.Random.GetString();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(functor(text), Is.EqualTo(text));
            Assert.That(functor(TestContext.CurrentContext.Random.Next()), Is.Null);
        }
    }

    private Func<TInput, bool> CreateIsInstanceOfType<TInput, TTarget>()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<bool>(
            $"IsInstanceOf_{typeof(TTarget).Name}_From_Int", [typeof(TInput)]);
        var argument = method.Argument<TInput>(0);
        method.Return(argument.IsInstanceOf<TTarget>());
        type.Build();
        return method.BuildingMethod.CreateDelegate<Func<TInput, bool>>();
    }

    [Test]
    public void IsInstanceOf_ValueType()
    {
        // value type should use static assignability check
        var isInteger = CreateIsInstanceOfType<int, int>();
        var isObject = CreateIsInstanceOfType<int, object>();
        var number = TestContext.CurrentContext.Random.Next();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(isInteger(number), Is.True);
            Assert.That(isObject(number), Is.False);
        }
    }

    [Test]
    public void IsInstanceOf_ReferenceType()
    {
        var isObject = CreateIsInstanceOfType<string, object>();
        var isCloneable = CreateIsInstanceOfType<string, ICloneable>();
        var isSample = CreateIsInstanceOfType<string, TestConversionExtensions>();
        var target = TestContext.CurrentContext.Random.GetString();
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That((bool)isObject(target), Is.True);
            Assert.That(isCloneable(target), Is.True);
            Assert.That(isSample(target), Is.False);
        }
    }
    
    private interface ISampleInterface { }
    
    private class SampleParent { }
    
    private class SampleChild : SampleParent, ISampleInterface { }

    private Func<SampleChild, SampleParent> CreateConvert_Assignable_Child_To_Parent()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<SampleParent>(
            nameof(CreateConvert_Assignable_Child_To_Parent), [typeof(SampleChild)]);
        var arg = method.Argument<SampleChild>(0);
        method.Return(arg.ConvertTo<SampleParent>());
        type.Build();
        return method.BuildingMethod.CreateDelegate<Func<SampleChild, SampleParent>>();
    }

    private Func<SampleChild, ISampleInterface> CreateConvert_Assignable_Parent_To_Interface()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<ISampleInterface>(
            nameof(CreateConvert_Assignable_Parent_To_Interface), [typeof(SampleChild)]);
        var arg = method.Argument<SampleChild>(0);
        method.Return(arg.ConvertTo<ISampleInterface>());
        type.Build();
        return method.BuildingMethod.CreateDelegate<Func<SampleChild, ISampleInterface>>();
    }

    [Test]
    public void ConvertTo_Assignment_Paths()
    {
        var toA = CreateConvert_Assignable_Child_To_Parent();
        var toI = CreateConvert_Assignable_Parent_To_Interface();
        var b = new SampleChild();
        var a = toA(b);
        var i = toI(b);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(a, Is.InstanceOf<SampleParent>());
            Assert.That(ReferenceEquals(a, b), Is.True);
            Assert.That(i, Is.InstanceOf<ISampleInterface>());
            Assert.That(ReferenceEquals(i, b), Is.True);
        }
    }
}
