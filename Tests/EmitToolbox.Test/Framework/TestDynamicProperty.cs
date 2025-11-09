using EmitToolbox.Framework;
using EmitToolbox.Framework.Extensions;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Test.Framework;

[TestFixture, TestOf(typeof(DynamicProperty))]
public class TestDynamicProperty
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    [Test]
    public void DefineProperty_Static_Getter()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var backing = type.FieldFactory.DefineStatic(typeof(int), "Backing");
        var property = type.PropertyFactory.DefineStatic<int>("Value");

        // Define and bind getter accessor for the property
        var getterAccessor = 
            type.MethodFactory.Static.DefineFunctor<int>(
            "get_Value", [], hasSpecialName: true);
        getterAccessor.Return(getterAccessor.Field<int>(backing));
        property.BindGetter(getterAccessor);

        type.Build();
        var functor = property.Getter!.BuildingMethod!.CreateDelegate<Func<int>>();
        var testNumber = TestContext.CurrentContext.Random.Next();
        backing.BuildingField.SetValue(null, testNumber);
        
        Assert.That(functor(), Is.EqualTo(testNumber));
    }

    [Test]
    public void DefineProperty_Static_Setter()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var backing = type.FieldFactory.DefineStatic(typeof(int), "Backing");
        var property = type.PropertyFactory.DefineStatic<int>("Value");

        // Define and bind setter accessor for the property
        var setterAccessor = type.MethodFactory.Static.DefineAction(
            "set_Value", [new ParameterDefinition(typeof(int))], hasSpecialName: true);
        setterAccessor.Field<int>(backing).Assign(setterAccessor.Argument<int>(0));
        setterAccessor.Return();
        property.BindSetter(setterAccessor);

        type.Build();
        var functor = property.Setter!.BuildingMethod.CreateDelegate<Action<int>>();

        var testNumber = TestContext.CurrentContext.Random.Next();
        functor(testNumber);
        Assert.That(backing.BuildingField.GetValue(null), Is.EqualTo(testNumber));
    }

    [Test]
    public void DefineProperty_Instance_Getter()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var backing = type.FieldFactory.DefineInstance(typeof(int), "Backing");
        var property = type.PropertyFactory.DefineInstance<int>("Value");

        // Define and bind getter accessor for the property
        var getterAccessor = type.MethodFactory.Instance.DefineFunctor<int>(
            "get_Value", [], hasSpecialName: true);
        getterAccessor.Return(backing.SymbolOf<int>(getterAccessor, getterAccessor.This()));
        property.BindGetter(getterAccessor);
        type.Build();
        
        var testInstance = Activator.CreateInstance(type.BuildingType)!;
        var functor = property.Getter!.BuildingMethod.CreateDelegate<Func<int>>(testInstance);
        var testNumber = TestContext.CurrentContext.Random.Next();
        backing.BuildingField.SetValue(testInstance, testNumber);
        Assert.That(functor(), Is.EqualTo(testNumber));
    }

    [Test]
    public void DefineProperty_Instance_Setter()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var backing = type.FieldFactory.DefineInstance(typeof(int), "Backing");
        var property = type.PropertyFactory.DefineInstance<int>("Value");

        // Define and bind setter accessor for the property
        var setterAccessor = type.MethodFactory.Instance.DefineAction(
            "set_Value", [new ParameterDefinition(typeof(int))], hasSpecialName: true);
        backing.SymbolOf(setterAccessor, setterAccessor.This()).Assign(setterAccessor.Argument<int>(0));
        setterAccessor.Return();
        property.BindSetter(setterAccessor);

        type.Build();
        var testInstance = Activator.CreateInstance(type.BuildingType)!;
        var functor = property.Setter!.BuildingMethod.CreateDelegate<Action<int>>(testInstance);
        var testNumber = TestContext.CurrentContext.Random.Next();
        functor(testNumber);
        Assert.That(backing.BuildingField.GetValue(testInstance), Is.EqualTo(testNumber));
    }
}