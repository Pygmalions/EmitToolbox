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
            [argumentNumber, argumentText]));
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
            [argumentNumber, argumentText]));
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
            [argumentNumber, argumentText]));
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
    public void EmplaceNew_Class_DefaultConstructor()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<SampleClass>(
            nameof(EmplaceNew_Class_DefaultConstructor), []);
        var variable = method.Variable<SampleClass>();
        variable.AssignNew();
        method.Return(variable);
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
    public void EmplaceNew_Class_ParameterizedConstructor()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<SampleClass>(
            nameof(EmplaceNew_Class_ParameterizedConstructor), [typeof(int), typeof(string)]);
        var variable = method.Variable<SampleClass>();
        var argumentNumber = method.Argument<int>(0);
        var argumentText = method.Argument<string>(1);
        variable.AssignNew(
            typeof(SampleClass).GetConstructor([typeof(int), typeof(string)])!,
            [argumentNumber, argumentText]);
        method.Return(variable);
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
    public void EmplaceNew_Struct_DefaultConstructor()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<SampleStruct>(
            nameof(EmplaceNew_Struct_DefaultConstructor), []);
        var variable = method.Variable<SampleStruct>();
        variable.AssignNew();
        method.Return(variable);
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
    public void EmplaceNew_Struct_ParameterizedConstructor()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<SampleStruct>(
            nameof(EmplaceNew_Struct_ParameterizedConstructor), [typeof(int), typeof(string)]);
        var variable = method.Variable<SampleStruct>();
        var argumentNumber = method.Argument<int>(0);
        var argumentText = method.Argument<string>(1);
        variable.AssignNew(
            typeof(SampleStruct).GetConstructor([typeof(int), typeof(string)])!,
            [argumentNumber, argumentText]);
        method.Return(variable);
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
    public void EmplaceNew_Class_Selector_DefaultConstructor()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<SampleClass>(
            nameof(EmplaceNew_Class_Selector_DefaultConstructor), []);
        var variable = method.Variable<SampleClass>();
        variable.AssignNew(() => new SampleClass());
        method.Return(variable);
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
    public void EmplaceNew_Class_Selector_ParameterizedConstructor()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<SampleClass>(
            nameof(EmplaceNew_Class_Selector_ParameterizedConstructor), [typeof(int), typeof(string)]);
        var variable = method.Variable<SampleClass>();
        var argumentNumber = method.Argument<int>(0);
        var argumentText = method.Argument<string>(1);
        variable.AssignNew(
            () => new SampleClass(
                Any<int>.Value, Any<string>.Value),
            [argumentNumber, argumentText]);
        method.Return(variable);
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

    public delegate void ActionWithRefParameter<TParameter>(ref TParameter value);
    
    [Test]
    public void EmplaceNew_ByRef_Class_String()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineAction(
            nameof(EmplaceNew_Class_Selector_ParameterizedConstructor), [typeof(SampleClass).MakeByRefType()]);
        var argument = method.Argument<SampleClass>(0, ContentModifier.Reference);
        argument.AssignNew(() => new SampleClass(Any<int>.Value, Any<string>.Value), 
            [method.Value(1), method.Value("Test String")]);
        method.Return();
        type.Build();
        var action = method.BuildingMethod.CreateDelegate<ActionWithRefParameter<SampleClass>>();
        var testInstance = new SampleClass(0, string.Empty);
        Assert.DoesNotThrow(() => action(ref testInstance));
        using (Assert.EnterMultipleScope())
        {
            Assert.That(testInstance.Number, Is.EqualTo(1));
            Assert.That(testInstance.Text, Is.EqualTo("Test String"));
        }
    }
    
    [Test]
    public void EmplaceNew_ByRef_Struct_Value()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineAction(
            nameof(EmplaceNew_Class_Selector_ParameterizedConstructor), [typeof(SampleStruct).MakeByRefType()]);
        var argument = method.Argument<SampleStruct>(0, ContentModifier.Reference);
        argument.AssignNew(
            () => new SampleStruct(Any<int>.Value, Any<string>.Value), 
            [method.Value(1), method.Value("Test String")]);
        method.Return();
        type.Build();
        var action = method.BuildingMethod.CreateDelegate<ActionWithRefParameter<SampleStruct>>();
        var testValue = new SampleStruct(0, string.Empty);
        Assert.DoesNotThrow(() => action(ref testValue));
        using (Assert.EnterMultipleScope())
        {
            Assert.That(testValue.Number, Is.EqualTo(1));
            Assert.That(testValue.Text, Is.EqualTo("Test String"));
        }
    }
}