using EmitToolbox.Framework;
using EmitToolbox.Framework.Symbols.Facades;

namespace EmitToolbox.Test.Framework.Facades;

[TestFixture]
public class TestArrayFacade
{
    private AssemblyBuildingContext _assembly;

    [SetUp]
    public void Initialize()
    {
        _assembly = AssemblyBuildingContext.DefineExecutable("TestLiteralSymbol");
    }

    [Test]
    public void TestArrayFacade_StoreElement()
    {
        var typeContext = _assembly.DefineClass("TestArrayFacade_Store");
        var methodContext = typeContext.Actions.Static("Test", [ParameterDefinition.Value<int[]>()]);
        var array = methodContext.Argument<int[]>(0).AsArray();

        var value = TestContext.CurrentContext.Random.Next();

        array.Assign(0, methodContext.Value(value));

        methodContext.Return();

        typeContext.Build();

        int[] argument = [1, 2, 3, 4, 5];

        methodContext.BuildingMethod.Invoke(null, [argument]);
        Assert.That(argument[0], Is.EqualTo(value));
    }

    [Test]
    public void TestArrayFacade_LoadElement()
    {
        var typeContext = _assembly.DefineClass("TestArrayFacade_Load");
        var methodContext = typeContext.Functors.Static("Test",
            [ParameterDefinition.Value<int[]>()],
            ResultDefinition.Value<int>());
        var array = methodContext.Argument<int[]>(0).AsArray();
        methodContext.Return(array[0]);
        typeContext.Build();

        var value = TestContext.CurrentContext.Random.Next();
        int[] argument = [value, 2, 3, 4, 5];

        Assert.That(methodContext.BuildingMethod.Invoke(null, [argument]),
            Is.EqualTo(value));
    }

    [Test]
    public void TestArrayFacade_LoadElement_DynamicIndex()
    {
        var typeContext = _assembly.DefineClass("TestArrayFacade_Load");
        var methodContext = typeContext.Functors.Static("Test",
            [
                ParameterDefinition.Value<int[]>("array"),
                ParameterDefinition.Value<int>("index")
            ],
            ResultDefinition.Value<int>());
        var parameterArray = methodContext.Argument<int[]>(0).AsArray();
        var parameterIndex = methodContext.Argument<int>(1);
        methodContext.Return(parameterArray[parameterIndex]);
        typeContext.Build();
        
        int[] array = [1, 2, 3, 4, 5];
        var index = TestContext.CurrentContext.Random.Next(0, array.Length);

        Assert.That(methodContext.BuildingMethod.Invoke(null, [array, index]),
            Is.EqualTo(array[index]));
    }
}