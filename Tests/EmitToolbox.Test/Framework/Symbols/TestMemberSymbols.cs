using System.Reflection;
using EmitToolbox.Framework;
using EmitToolbox.Framework.Symbols.Members;

namespace EmitToolbox.Test.Framework.Symbols;

public class TestMemberSymbols
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Initialize()
    {
        _assembly = DynamicAssembly
            .DefineExecutable("TestMemberSymbols")
            .IgnoreAccessChecksToAssembly(Assembly.GetExecutingAssembly());
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
        var methodContext = typeContext.FunctorBuilder.DefineStatic("Test",
            [ParameterDefinition.Value<TestClass>()],
            ResultDefinition.Value<int>());
        var argumentInstance = methodContext.Argument<TestClass>(0);
        methodContext.Return(argumentInstance.FieldOf(instance => instance.Field));
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
        var methodContext = typeContext.FunctorBuilder.DefineStatic("Test",
            [ParameterDefinition.Value<TestClass>()],
            ResultDefinition.Value<int>());
        var argumentInstance = methodContext.Argument<TestClass>(0);
        methodContext.Return(argumentInstance.PropertyOf(instance => instance.Property));
        typeContext.Build();
        var value = TestContext.CurrentContext.Random.Next();
        var instance = new TestClass(0) { Property = value };

        Assert.That(methodContext.BuildingMethod.Invoke(null, [instance]),
            Is.EqualTo(value));
    }
}