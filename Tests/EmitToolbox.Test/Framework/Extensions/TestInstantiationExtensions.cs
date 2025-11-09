using EmitToolbox.Framework;
using EmitToolbox.Framework.Extensions;
using EmitToolbox.Framework.Symbols;
using EmitToolbox.Framework.Utilities;

namespace EmitToolbox.Test.Framework.Extensions;

[TestFixture, TestOf(typeof(InstantiationExtensions))]
public class TestInstantiationExtensions
{
    private DynamicAssembly _assembly = null!;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    public class SampleClass(int number, string text)
    {
        public SampleClass() : this(1, "Default")
        {}
        
        public readonly int Number = number;
        public readonly string Text = text;
    }
    
    public struct SampleStruct(int number, string text)
    {
        public SampleStruct() : this(1, "Default")
        {
        }
        
        public readonly int Number = number;
        public readonly string Text = text;
    }

    [Test]
    public void New_Class_DefaultConstructor()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<SampleClass>(
            nameof(New_Class_DefaultConstructor), []);
        method.Return(method.New<SampleClass>(typeof(SampleClass).GetConstructor(Type.EmptyTypes)!));
        type.Build();
        
        var functor = method.BuildingMethod.CreateDelegate<Func<SampleClass>>();

        var instance = functor();
        Assert.That(instance, Is.Not.Null);
        Assert.That(instance, Is.InstanceOf<SampleClass>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(instance.Number, Is.EqualTo(1));
            Assert.That(instance.Text, Is.EqualTo("Default"));
        }
    }
    
    [Test]
    public void New_Class_ParameterizedConstructor()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<SampleClass>(
            nameof(New_Class_ParameterizedConstructor), [typeof(int), typeof(string)]);
        var argumentNumber = method.Argument<int>(0);
        var argumentText = method.Argument<string>(1);
        method.Return(method.New<SampleClass>(
            typeof(SampleClass).GetConstructor([typeof(int), typeof(string)])!,
            argumentNumber, argumentText));
        type.Build();
        
        var functor = method.BuildingMethod.CreateDelegate<Func<int, string, SampleClass>>();

        var testNumber = TestContext.CurrentContext.Random.Next();
        var testText = TestContext.CurrentContext.Random.GetString();
        var instance = functor(testNumber, testText);
        Assert.That(instance, Is.Not.Null);
        Assert.That(instance, Is.InstanceOf<SampleClass>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(instance.Number, Is.EqualTo(testNumber));
            Assert.That(instance.Text, Is.EqualTo(testText));
        }
    }
    
    [Test]
    public void New_Struct_DefaultConstructor()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<SampleStruct>(
            nameof(New_Struct_DefaultConstructor), []);
        method.Return(method.New<SampleStruct>(typeof(SampleStruct).GetConstructor(Type.EmptyTypes)!));
        type.Build();
        
        var functor = method.BuildingMethod.CreateDelegate<Func<SampleStruct>>();

        var instance = functor();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(instance.Number, Is.EqualTo(1));
            Assert.That(instance.Text, Is.EqualTo("Default"));
        }
    }
    
    [Test]
    public void New_Struct_ParameterizedConstructor()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<SampleStruct>(
            nameof(New_Class_ParameterizedConstructor), [typeof(int), typeof(string)]);
        var argumentNumber = method.Argument<int>(0);
        var argumentText = method.Argument<string>(1);
        method.Return(method.New<SampleStruct>(
            typeof(SampleStruct).GetConstructor([typeof(int), typeof(string)])!,
            argumentNumber, argumentText));
        type.Build();
        
        var functor = method.BuildingMethod.CreateDelegate<Func<int, string, SampleStruct>>();

        var testNumber = TestContext.CurrentContext.Random.Next();
        var testText = TestContext.CurrentContext.Random.GetString();
        var instance = functor(testNumber, testText);
        Assert.That(instance, Is.InstanceOf<SampleStruct>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(instance.Number, Is.EqualTo(testNumber));
            Assert.That(instance.Text, Is.EqualTo(testText));
        }
    }
    
    [Test]
    public void New_Class_Selector_DefaultConstructor()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<SampleClass>(
            nameof(New_Class_DefaultConstructor), []);
        method.Return(method.New(() => new SampleClass()));
        type.Build();
        
        var functor = method.BuildingMethod.CreateDelegate<Func<SampleClass>>();

        var instance = functor();
        Assert.That(instance, Is.Not.Null);
        Assert.That(instance, Is.InstanceOf<SampleClass>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(instance.Number, Is.EqualTo(1));
            Assert.That(instance.Text, Is.EqualTo("Default"));
        }
    }
    
    [Test]
    public void New_Class_Selector_ParameterizedConstructor()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<SampleClass>(
            nameof(New_Class_ParameterizedConstructor), [typeof(int), typeof(string)]);
        var argumentNumber = method.Argument<int>(0);
        var argumentText = method.Argument<string>(1);
        method.Return(method.New(
            () => new SampleClass(
                Any<int>.Value, Any<string>.Value),
            argumentNumber, argumentText));
        type.Build();
        
        var functor = method.BuildingMethod.CreateDelegate<Func<int, string, SampleClass>>();

        var testNumber = TestContext.CurrentContext.Random.Next();
        var testText = TestContext.CurrentContext.Random.GetString();
        var instance = functor(testNumber, testText);
        Assert.That(instance, Is.Not.Null);
        Assert.That(instance, Is.InstanceOf<SampleClass>());
        using (Assert.EnterMultipleScope())
        {
            Assert.That(instance.Number, Is.EqualTo(testNumber));
            Assert.That(instance.Text, Is.EqualTo(testText));
        }
    }
}