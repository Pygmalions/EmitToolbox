using System.Reflection;
using EmitToolbox.Framework;
using EmitToolbox.Framework.Symbols.Members;

namespace EmitToolbox.Test.Framework.Symbols;

public class TestMemberSymbols
{
    private AssemblyBuildingContext _assembly;

    [SetUp]
    public void Initialize()
    {
        _assembly = AssemblyBuildingContext
            .CreateExecutableContextBuilder("TestMemberSymbols")
            .IgnoreAccessToAssembly(Assembly.GetExecutingAssembly())
            .Build();
    }
    
    public class TestClass(int methodValue)
    {
        public int Field;
        public int Property { get; set; }
        public int Method() => methodValue;
    }
    
    [Test]
    public void TestMemberSymbol_Field()
    {
        var typeContext = _assembly.DefineClass("TestMember_Field");
        var methodContext = typeContext.Functors.Static("Test", 
            [ParameterDefinition.Value<TestClass>()],
            ResultDefinition.Value<int>());
        var argumentInstance = methodContext.Argument<TestClass>(0);
        methodContext.Return(argumentInstance.GetField(instance => instance.Field));
        typeContext.Build();
        var value = TestContext.CurrentContext.Random.Next();
        var instance = new TestClass(0) { Field = value };
        
        Assert.That(methodContext.BuildingMethod.Invoke(null, [instance]),
            Is.EqualTo(value));
    }
    
    [Test]
    public void TestMemberSymbol_Property()
    {
        var typeContext = _assembly.DefineClass("TestMember_Property");
        var methodContext = typeContext.Functors.Static("Test", 
            [ParameterDefinition.Value<TestClass>()],
            ResultDefinition.Value<int>());
        var argumentInstance = methodContext.Argument<TestClass>(0);
        methodContext.Return(argumentInstance.GetProperty(instance => instance.Property));
        typeContext.Build();
        var value = TestContext.CurrentContext.Random.Next();
        var instance = new TestClass(0) { Property = value };
        
        Assert.That(methodContext.BuildingMethod.Invoke(null, [instance]),
            Is.EqualTo(value));
    }
    
    [Test]
    public void TestMemberSymbol_Method()
    {
        var typeContext = _assembly.DefineClass("TestMember_Method");
        var methodContext = typeContext.Functors.Static("Test", 
            [ParameterDefinition.Value<TestClass>()],
            ResultDefinition.Value<int>());
        var argumentInstance = methodContext.Argument<TestClass>(0);
        var method = argumentInstance.GetMethod(instance => instance.Method());
        methodContext.Return(method.Invoke());
        typeContext.Build();
        var value = TestContext.CurrentContext.Random.Next();
        var instance = new TestClass(value);
        
        Assert.That(methodContext.BuildingMethod.Invoke(null, [instance]),
            Is.EqualTo(value));
    }

    private class SampleClassForConstructor(int number)
    {
        public int Number = number;
    }

    private struct SampleStructForConstructor(int number)
    {
        public int Number = number;
    }
    
    [Test]
    public void TestMemberSymbol_Constructor_Class()
    {
        var typeContext = _assembly.DefineClass("TestMember_Constructor_Class");
        var methodContext = typeContext.Functors.Static("Test", 
            [ParameterDefinition.Value<int>()],
            ResultDefinition.Value<SampleClassForConstructor>());
        var argumentValue = methodContext.Argument<int>(0);
        var constructor = new ConstructorSymbol<SampleClassForConstructor>(methodContext,
            typeof(SampleClassForConstructor).GetConstructor([typeof(int)])!);
        methodContext.Return(constructor.New([argumentValue]));
        typeContext.Build();
        var value = TestContext.CurrentContext.Random.Next();
        var result = (SampleClassForConstructor)methodContext.BuildingMethod.Invoke(null, [value])!;
        Assert.That(result.Number, Is.EqualTo(value));
    }
    
    [Test]
    public void TestMemberSymbol_Constructor_Struct()
    {
        var typeContext = _assembly.DefineClass("TestMember_Constructor_Struct");
        var methodContext = typeContext.Functors.Static("Test", 
            [ParameterDefinition.Value<int>()],
            ResultDefinition.Value<SampleStructForConstructor>());
        var argumentValue = methodContext.Argument<int>(0);
        var constructor = new ConstructorSymbol<SampleStructForConstructor>(methodContext,
            typeof(SampleStructForConstructor).GetConstructor([typeof(int)])!);
        methodContext.Return(constructor.New([argumentValue]));
        typeContext.Build();
        var value = TestContext.CurrentContext.Random.Next();
        var result = (SampleStructForConstructor)methodContext.BuildingMethod.Invoke(null, [value])!;
        Assert.That(result.Number, Is.EqualTo(value));
    }
}