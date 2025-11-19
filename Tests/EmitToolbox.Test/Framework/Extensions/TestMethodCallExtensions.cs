using EmitToolbox.Extensions;
using EmitToolbox.Symbols;
using EmitToolbox.Utilities;

namespace EmitToolbox.Test.Framework.Extensions;

[TestFixture, TestOf(typeof(MethodCallExtensions)), SingleThreaded]
public class TestMethodCallExtensions
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    [Test]
    public void Invoke_InstanceMethod_FromSelector()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int>(
            "StringIndexOf", [typeof(string), typeof(char)]);

        var argumentString = method.Argument<string>(0);
        var argumentChar = method.Argument<char>(1);

        // Use selector-based invocation on an instance symbol
        method.Return(argumentString.Invoke(
            symbol => symbol.IndexOf(Any<char>.Value),
            [argumentChar]));

        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<string, char, int>>();

        var testString = TestContext.CurrentContext.Random.GetString();
        var testChar = testString[TestContext.CurrentContext.Random.Next(0, testString.Length)];
        Assert.That(functor(testString, testChar), Is.EqualTo(
            testString.IndexOf(testChar)));
    }

    [Test]
    public void Invoke_StaticMethod_FromSelector()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int>(
            "Max", [typeof(int), typeof(int)]);

        var a = method.Argument<int>(0);
        var b = method.Argument<int>(1);

        // Use DynamicMethod.Invoke with selector for static method
        method.Return(method.Invoke(() => Math.Max(Any<int>.Value, Any<int>.Value), [a, b]));

        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<int, int, int>>();

        var x = TestContext.CurrentContext.Random.Next();
        var y = TestContext.CurrentContext.Random.Next();
        Assert.That(functor(x, y), Is.EqualTo(Math.Max(x, y)));
    }
    
    public class SampleClass
    {
        public static int StaticBackingField { get; set; }
        
        public static int StaticProperty
        {
            get => StaticBackingField;
            set => StaticBackingField = value;
        }
        
        public int BackingField { get; set; }

        public int Property
        {
            get => BackingField;
            set => BackingField = value;
        }
    }
    
    [Test]
    public void GetPropertyValue_Instance_FromSelector()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        // Define and bind getter
        var method = type.MethodFactory.Static.DefineFunctor<int>(
            nameof(SetPropertyValue_Instance_FromSelector), [typeof(SampleClass)]);
        var argumentTarget = method.Argument<SampleClass>(0);
        method.Return(argumentTarget.GetPropertyValue(symbol => symbol.Property));
        type.Build();
        
        var functor = method.BuildingMethod.CreateDelegate<Func<SampleClass, int>>();
        var testInstance = new SampleClass();
        var testValue = TestContext.CurrentContext.Random.Next();
        testInstance.BackingField = testValue;
        Assert.That(functor(testInstance), Is.EqualTo(testValue));
    }
    
    [Test]
    public void SetPropertyValue_Instance_FromSelector()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        // Define and bind getter
        var method = type.MethodFactory.Static.DefineAction(
            nameof(SetPropertyValue_Instance_FromSelector), [typeof(SampleClass), typeof(int)]);
        var argumentTarget = method.Argument<SampleClass>(0);
        var argumentNumber = method.Argument<int>(1);
        argumentTarget.SetPropertyValue(symbol => symbol.Property, argumentNumber);
        method.Return();
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Action<SampleClass, int>>();
        var testInstance = new SampleClass();
        var testValue = TestContext.CurrentContext.Random.Next();
        Assert.DoesNotThrow(()=> functor(testInstance, testValue));
        Assert.That(testInstance.BackingField, Is.EqualTo(testValue));
    }
    
    [Test]
    public void GetPropertyValue_Static_FromSelector()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        // Define and bind getter
        var method = type.MethodFactory.Static.DefineFunctor<int>(
            nameof(SetPropertyValue_Instance_FromSelector), []);
        method.Return(method.GetPropertyValue(() => SampleClass.StaticProperty));
        type.Build();
        
        var functor = method.BuildingMethod.CreateDelegate<Func<int>>();
        var testValue = TestContext.CurrentContext.Random.Next();
        SampleClass.StaticBackingField = testValue;
        Assert.That(functor(), Is.EqualTo(testValue));
    }
    
    [Test]
    public void SetPropertyValue_Static_FromSelector()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        // Define and bind getter
        var method = type.MethodFactory.Static.DefineAction(
            nameof(SetPropertyValue_Instance_FromSelector), [typeof(int)]);
        var argumentNumber = method.Argument<int>(0);
        method.SetPropertyValue(() => SampleClass.StaticProperty, argumentNumber);
        method.Return();
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Action<int>>();
        var testValue = TestContext.CurrentContext.Random.Next();
        Assert.DoesNotThrow(()=> functor(testValue));
        Assert.That(SampleClass.StaticBackingField, Is.EqualTo(testValue));
    }

    [Test]
    public void GetPropertyValue_StaticAccess_InstanceProperty_Throws()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        
        var method = type.MethodFactory.Static.DefineFunctor<int>(
            nameof(GetPropertyValue_StaticAccess_InstanceProperty_Throws), []);

        Assert.Throws<InvalidOperationException>(() =>
        {
            // ReSharper disable once MustUseReturnValue
            method.GetPropertyValue<int>(
                typeof(SampleClass).GetProperty(nameof(SampleClass.Property))!);
        });
    }
}