using EmitToolbox.Framework;
using EmitToolbox.Framework.Symbols.Extensions;

namespace EmitToolbox.Test.Framework;

[TestFixture]
public class TestLogicControl
{
    private static AssemblyBuildingContext _assembly;

    [SetUp]
    public void Initialize()
    {
        _assembly = AssemblyBuildingContext
            .CreateExecutableContextBuilder("TestLiteralSymbol")
            .Build();
    }

    [Test]
    public void TestLogicControl_IfThenElse()
    {
        var typeContext = _assembly.DefineClass("TestLogicControl_IfThenElse");
        var methodContext = typeContext.DefineStaticFunctor("Test",
            [ParameterDefinition.Value<bool>()], ResultDefinition.Value<int>());
        var argument = methodContext.Argument<bool>(0);
        methodContext.If(argument, () => { methodContext.Return(methodContext.Value(1)); },
            () => { methodContext.Return(methodContext.Value(0)); });
        methodContext.Return(methodContext.Value(-1));
        typeContext.Build();
        Assert.Multiple(() =>
        {
            Assert.That(methodContext.BuildingMethod.Invoke(null, [true]), Is.EqualTo(1));
            Assert.That(methodContext.BuildingMethod.Invoke(null, [false]), Is.EqualTo(0));
        });
    }

    [Test]
    public void TestLogicControl_While()
    {
        var typeContext = _assembly.DefineClass("TestLogicControl_While");
        var methodContext = typeContext.DefineStaticFunctor("Test",
            [ParameterDefinition.Value<int>()], ResultDefinition.Value<int>());
        var argument = methodContext.Argument<int>(0);
        methodContext.While(
            methodContext.Expression(() =>
                argument.IsEqualTo(methodContext.Value(0)).Negate()),
            () => argument.SelfSubtract(1));
        methodContext.Return(argument);
        typeContext.Build();
        Assert.That(methodContext.BuildingMethod.Invoke(null, [3]), Is.EqualTo(0));
    }
}