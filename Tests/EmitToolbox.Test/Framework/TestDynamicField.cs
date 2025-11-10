using EmitToolbox.Framework;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Test.Framework;

[TestFixture, TestOf(typeof(DynamicField))]
public class TestDynamicField
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }
    
    [Test]
    public void DefineField_Static_Getter()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var field = type.FieldFactory.DefineStatic(typeof(int), "Field");
        var getter = type.MethodFactory.Static.DefineFunctor<int>(
           "Getter", []);
        getter.Return(getter.Field<int>(field));
        type.Build();
        var functor = getter.BuildingMethod.CreateDelegate<Func<int>>();

        var testNumber = TestContext.CurrentContext.Random.Next();
        field.BuildingField.SetValue(null, testNumber);
        Assert.That(functor(), Is.EqualTo(testNumber));
    }
    
    [Test]
    public void DefineField_Static_Setter()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var field = type.FieldFactory.DefineStatic(typeof(int), "Field");
        var getter = type.MethodFactory.Static.DefineAction(
            "Setter", [typeof(int)]);   
        getter.Field<int>(field).AssignContent(getter.Argument<int>(0));
        getter.Return();
        type.Build();
        var functor = getter.BuildingMethod.CreateDelegate<Action<int>>();

        var testNumber = TestContext.CurrentContext.Random.Next();
        functor(testNumber);
        Assert.That(field.BuildingField.GetValue(null), Is.EqualTo(testNumber));
    }
    
    [Test]
    public void DefineField_Instance_Getter()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var field = type.FieldFactory.DefineInstance(typeof(int), "Field");
        var getter = type.MethodFactory.Instance.DefineFunctor<int>(
            "Getter", []);
        getter.Return(field.SymbolOf<int>(getter, getter.This()));
        type.Build();
        var testInstance = Activator.CreateInstance(type.BuildingType)!;
        var functor = getter.BuildingMethod.CreateDelegate<Func<int>>(testInstance);
        var testNumber = TestContext.CurrentContext.Random.Next();
        field.BuildingField.SetValue(testInstance, testNumber);
        Assert.That(functor(), Is.EqualTo(testNumber));
    }
    
    [Test]
    public void DefineField_Instance_Setter()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var field = type.FieldFactory.DefineInstance(typeof(int), "Field");
        var getter = type.MethodFactory.Instance.DefineAction(
            "Setter", [typeof(int)]);
        field.SymbolOf(getter, getter.This()).AssignContent(getter.Argument<int>(0));
        getter.Return();
        type.Build();
        var testInstance = Activator.CreateInstance(type.BuildingType)!;
        var functor = getter.BuildingMethod.CreateDelegate<Action<int>>(testInstance);
        var testNumber = TestContext.CurrentContext.Random.Next();
        functor(testNumber);
        Assert.That(field.BuildingField.GetValue(testInstance), Is.EqualTo(testNumber));
    }
}