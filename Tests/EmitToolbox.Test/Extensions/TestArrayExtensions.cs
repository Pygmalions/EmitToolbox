using EmitToolbox.Extensions;
using EmitToolbox.Symbols;

namespace EmitToolbox.Test.Extensions;

[TestFixture, TestOf(typeof(ArrayExtensions))]
public class TestArrayExtensions
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    [Test]
    public void ToArraySymbol_FromTypedSymbols()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int[]>(nameof(ToArraySymbol_FromTypedSymbols),
            [typeof(int), typeof(int), typeof(int)]);

        var a0 = method.Argument<int>(0);
        var a1 = method.Argument<int>(1);
        var a2 = method.Argument<int>(2);

        var symbols = new List<ISymbol<int>> { a0, a1, a2 };
        var array = symbols.ToArraySymbol();
        method.Return(array);

        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<int, int, int, int[]>>();

        var v0 = TestContext.CurrentContext.Random.Next(-1000, 1000);
        var v1 = TestContext.CurrentContext.Random.Next(-1000, 1000);
        var v2 = TestContext.CurrentContext.Random.Next(-1000, 1000);

        var result = functor(v0, v1, v2);
        Assert.That(result, Is.EqualTo([v0, v1, v2]));
    }

    [Test]
    public void ToArraySymbol_FromUntypedSymbols_WithGenericArgument()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int[]>(nameof(ToArraySymbol_FromUntypedSymbols_WithGenericArgument),
            [typeof(int), typeof(int), typeof(int)]);

        var a0 = method.Argument<int>(0);
        var a1 = method.Argument<int>(1);
        var a2 = method.Argument<int>(2);

        // Create a non-generic symbol collection and let ToArraySymbol<int>() coerce items
        IReadOnlyCollection<ISymbol> symbols = new List<ISymbol> { a0, a1, a2 };
        var array = symbols.ToArraySymbol<int>();
        method.Return(array);

        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<int, int, int, int[]>>();

        var v0 = TestContext.CurrentContext.Random.Next(-1000, 1000);
        var v1 = TestContext.CurrentContext.Random.Next(-1000, 1000);
        var v2 = TestContext.CurrentContext.Random.Next(-1000, 1000);

        var result = functor(v0, v1, v2);
        Assert.That(result, Is.EqualTo(new[] { v0, v1, v2 }));
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

        var testArray = CreateRandomArray(() => TestContext.CurrentContext.Random.Next(-100, 100));
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
                typeof(string[]), typeof(int), typeof(string)
            ]);
        var argumentArray = method.Argument<string[]>(0);
        var argumentIndex = method.Argument<int>(1);
        var argumentValue = method.Argument<string>(2);
        var element = argumentArray.ElementAt(argumentIndex);
        element.AssignContent(argumentValue);
        method.Return(element);
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Func<string[], int, string, string>>();

        var testArray = CreateRandomArray(() => TestContext.CurrentContext.Random.GetString(10));
        var testIndex = TestContext.CurrentContext.Random.Next(0, testArray.Length);
        var testValue = TestContext.CurrentContext.Random.GetString(10);

        Assert.That(functor(testArray, testIndex, testValue),
            Is.SameAs(testValue));
    }


    [Test]
    public void AssignNew()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<int[]>(
            nameof(AssignNew), [
                typeof(int),
                typeof(int[])
            ]);
        var argumentLength = method.Argument<int>(0);
        var argumentArray = method.Argument<int[]>(1);

        argumentArray.AssignNew(argumentLength);

        method.Return(argumentArray);
        
        type.Build();
        
        var action = method.BuildingMethod.CreateDelegate<Func<int, int[], int[]>>();
        var array = new int[1];
        var testLength = TestContext.CurrentContext.Random.Next(2, 10);
        var resultArray = action(testLength, array);
        Assert.That(resultArray, Has.Length.EqualTo(testLength));
        Assert.That(resultArray, Is.Not.SameAs(array));
    }
    
    private delegate void ActionForAssigningNewByReference<TElement>(int length, ref TElement[] array);

    
    [Test]
    public void AssignNew_ByReference()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineAction(
            nameof(AssignNew_ByReference), [
                typeof(int),
                typeof(int[]).MakeByRefType()
            ]);
        var argumentLength = method.Argument<int>(0);
        var argumentArray = method.Argument<int[]>(1, ContentModifier.Reference);

        argumentArray.AssignNew(argumentLength);

        method.Return();
        
        type.Build();
        
        var action = method.BuildingMethod.CreateDelegate<ActionForAssigningNewByReference<int>>();
        var array = new int[1];
        var testLength = TestContext.CurrentContext.Random.Next(2, 10);
        Assert.DoesNotThrow(() => action(testLength, ref array));
        Assert.That(array, Has.Length.EqualTo(testLength));
    }
}