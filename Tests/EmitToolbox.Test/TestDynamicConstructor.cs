using System.Reflection;
using EmitToolbox.Symbols;

namespace EmitToolbox.Test;

[TestFixture, TestOf(typeof(DynamicConstructor))]
public class TestDynamicConstructor
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
        _assembly.IgnoreVisibilityChecksToAssembly(Assembly.GetExecutingAssembly());
    }

    private class SampleClass
    {
        public int Value { get; set; }
        
        public SampleClass()
        {
            Value = 1;
        }
        
        protected SampleClass(int value)
        {
            Value = value;
        }

        protected SampleClass(int value, string value2)
        {
            Value = 2;
        }
    }
    
    [Test]
    public void InvokeBaseConstructor_Parameterless()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString(),
            parent: typeof(SampleClass));
        var constructor = type.MethodFactory.Constructor.Define([]);
        constructor.InvokeBaseTypeConstructor();
        constructor.Return();
        type.Build();
        
        var instance = Activator.CreateInstance(type.BuildingType);
        Assert.That(instance, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(instance, Is.InstanceOf<SampleClass>());
            Assert.That(type.BuildingType.GetProperty("Value")!.GetValue(instance),
                Is.EqualTo(1));
        }
    }
    
    [Test]
    public void InvokeBaseConstructor_WithParameters()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString(),
            parent: typeof(SampleClass));
        var constructor = type.MethodFactory.Constructor.Define([]);
        constructor.InvokeBaseTypeConstructor(constructor.Literal(3));
        constructor.Return();
        type.Build();
        
        var instance = Activator.CreateInstance(type.BuildingType);
        Assert.That(instance, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(instance, Is.InstanceOf<SampleClass>());
            Assert.That(type.BuildingType.GetProperty("Value")!.GetValue(instance),
                Is.EqualTo(3));
        }
    }
}