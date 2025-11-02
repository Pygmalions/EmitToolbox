using EmitToolbox.Framework;
using EmitToolbox.Framework.Symbols.Extensions;

namespace EmitToolbox.Test.Framework;

[TestFixture, TestOf(typeof(DynamicMethod))]
public class TestCapturedObjects
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Initialize()
    {
        _assembly = DynamicAssembly.DefineExecutable("TestTypeBuildingContext");
    }

    [TestCase(1),
     TestCase(1.0),
     TestCase(true)]
    public void UseCapturedValue<TValue>(TValue value) where TValue : struct
    {
        var typeContext = _assembly.DefineClass("TestCapturedObjects_UseCapturedValue");
        var methodContext = typeContext.FunctorBuilder.DefineStatic("Test",
            [], ResultDefinition.Value<TValue>());
        var capturedLiteral = typeContext.CaptureObject(value);
        methodContext.Return(capturedLiteral.SymbolOf(methodContext).Unbox<TValue>());
        typeContext.Build();
        Assert.That((TValue?)methodContext.BuildingMethod.Invoke(null, []),
            Is.EqualTo(value));
    }
}