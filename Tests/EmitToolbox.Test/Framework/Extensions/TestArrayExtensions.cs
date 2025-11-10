using EmitToolbox.Framework;
using EmitToolbox.Framework.Extensions;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Test.Framework.Extensions;

[TestFixture, TestOf(typeof(ArrayExtensions))]
public class TestArrayExtensions
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    private TElement[] CreateRandomArray<TElement>(Func<TElement> elementFactory)
    {
        var length = TestContext.CurrentContext.Random.Next(1, 30);
        var array = new TElement[length];
        for (var index = 0; index < length; index++)
            array[index] = elementFactory();
        return array;
    }
    
    [Test]
    public void NewArray()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int[]>(nameof(NewArray), [typeof(int)]);
        var argumentLength = method.Argument<int>(0);
        var variableArray = method.NewArray<int>(argumentLength);
        method.Return(variableArray);
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<int, int[]>>();

        var length = TestContext.CurrentContext.Random.Next(0, 100);
        Assert.That(functor(length), Has.Length.EqualTo(length));
    }

    [Test]
    public void GetLength()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int>(nameof(GetLength), [typeof(int[])]);
        var argumentArray = method.Argument<int[]>(0);
        method.Return(argumentArray.Length);
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<int[], int>>();

        var input = new int[TestContext.CurrentContext.Random.Next(0, 30)];
        Assert.That(functor(input), Is.EqualTo(input.Length));
    }

    [Test]
    public void ElementAt_Write_And_Read_ValueType_Int()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineAction(nameof(
            ElementAt_Write_And_Read_ValueType_Int), [typeof(int[]), typeof(int), typeof(int)]);
        var argumentArray = method.Argument<int[]>(0);
        var argumentIndex = method.Argument<int>(1);
        var argumentValue = method.Argument<int>(2);
        var element = argumentArray.ElementAt(argumentIndex);
        element.AssignContent(element + argumentValue);
        method.Return();
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Action<int[], int, int>>();
        
        var testArray = CreateRandomArray(
            () => TestContext.CurrentContext.Random.Next(-100, 100));
        var testIndex = TestContext.CurrentContext.Random.Next(0, testArray.Length);
        var testValue = TestContext.CurrentContext.Random.Next(-100, 100);
        var answer = testArray[testIndex] + testValue;
        functor(testArray, testIndex, testValue);
        Assert.That(testArray[testIndex], Is.EqualTo(answer));
    }

    [Test]
    public void ElementAt_Write_And_Read_ReferenceType_String()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<string>(
            nameof(ElementAt_Write_And_Read_ReferenceType_String), [
                typeof(string[]), typeof(int), typeof(string)]);
        var argumentArray = method.Argument<string[]>(0);
        var argumentIndex = method.Argument<int>(1);
        var argumentValue = method.Argument<string>(2);
        var element = argumentArray.ElementAt(argumentIndex);
        element.AssignContent(argumentValue);
        method.Return(element);
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<string[], int, string, string>>();

        var testArray = CreateRandomArray(
            () => TestContext.CurrentContext.Random.GetString(10));
        var testIndex = TestContext.CurrentContext.Random.Next(0, testArray.Length);
        var testValue = TestContext.CurrentContext.Random.GetString(10);
        
        Assert.That(functor(testArray, testIndex, testValue), 
            Is.SameAs(testValue));
    }
}
