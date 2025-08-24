using EmitToolbox.Framework;

namespace EmitToolbox.Test.Framework;

[TestFixture]
public class TestTypeBuildingContext
{
    private static AssemblyBuildingContext _assembly;

    [SetUp]
    public void Initialize()
    {
        _assembly = AssemblyBuildingContext
            .CreateExecutableContextBuilder("TestTypeBuildingContext")
            .Build();
    }
    
    [Test]
    public void InstanceField_Set()
    {
        var typeContext = _assembly.DefineClass("InstanceField_Set");
        var fieldContext = typeContext.DefineField<int>("TestField");
        var methodContext = typeContext.DefineAction("FieldSetter",
            [ParameterDefinition.Value<int>()]);
        var argument = methodContext.Argument<int>(1);
        fieldContext.InstanceSymbol(methodContext).Assign(argument);
        methodContext.Return();
        typeContext.Build();
        var instance = Activator.CreateInstance(typeContext.BuildingType);
        var value = TestContext.CurrentContext.Random.Next();
        methodContext.BuildingMethod.Invoke(instance, [value]);
        Assert.That(fieldContext.BuildingField.GetValue(instance), Is.EqualTo(value));
    }
    
    [Test]
    public void InstanceField_Get()
    {
        var typeContext = _assembly.DefineClass("InstanceField_Get");
        var fieldContext = typeContext.DefineField<int>("TestField");
        var methodContext = typeContext.DefineFunctor("FieldGetter",
            [], ResultDefinition.Value<int>());
        var field = fieldContext.InstanceSymbol(methodContext);
        methodContext.Return(field);
        typeContext.Build();
        var instance = Activator.CreateInstance(typeContext.BuildingType);
        var value = TestContext.CurrentContext.Random.Next();
        fieldContext.BuildingField.SetValue(instance, value);
        Assert.That(methodContext.BuildingMethod.Invoke(instance, null), Is.EqualTo(value));
    }
    
    [Test]
    public void StaticField_Set()
    {
        var typeContext = _assembly.DefineClass("StaticField_Set");
        var fieldContext = typeContext.DefineField<int>("TestField", isStatic: true);
        var methodContext = typeContext.DefineAction("FieldSetter",
            [ParameterDefinition.Value<int>()], modifier: MethodModifier.Static);
        var argument = methodContext.Argument<int>(0);
        fieldContext.StaticSymbol(methodContext).Assign(argument);
        methodContext.Return();
        typeContext.Build();
        var value = TestContext.CurrentContext.Random.Next();
        methodContext.BuildingMethod.Invoke(null, [value]);
        Assert.That(fieldContext.BuildingField.GetValue(null), Is.EqualTo(value));
    }
    
    [Test]
    public void StaticField_Get()
    {
        var typeContext = _assembly.DefineClass("StaticField_Get");
        var fieldContext = typeContext.DefineField<int>("TestField", isStatic: true);
        var methodContext = typeContext.DefineFunctor("FieldGetter",
            [], ResultDefinition.Value<int>(), modifier: MethodModifier.Static);
        var field = fieldContext.StaticSymbol(methodContext);
        methodContext.Return(field);
        typeContext.Build();
        var value = TestContext.CurrentContext.Random.Next();
        fieldContext.BuildingField.SetValue(null, value);
        Assert.That(methodContext.BuildingMethod.Invoke(null, null), Is.EqualTo(value));
    }
    
    [Test]
    public void InstanceProperty_Set()
    {
        var typeContext = _assembly.DefineClass("InstanceProperty_Set");
        var propertyContext = typeContext.DefineProperty<int>("TestProperty");
        var fieldContext = typeContext.DefineField<int>("_field");
        
        var setter = propertyContext.DefineSetter();
        fieldContext.InstanceSymbol(setter).Assign(setter.Argument<int>(1));
        setter.Return();
        
        typeContext.Build();
        var instance = Activator.CreateInstance(typeContext.BuildingType);
        var value = TestContext.CurrentContext.Random.Next();
        propertyContext.BuildingProperty.SetValue(instance, value);
        Assert.That(fieldContext.BuildingField.GetValue(instance), Is.EqualTo(value));
    }
    
    [Test]
    public void InstanceProperty_Get()
    {
        var typeContext = _assembly.DefineClass("InstanceProperty_Get");
        var propertyContext = typeContext.DefineProperty<int>("TestProperty");
        var fieldContext = typeContext.DefineField<int>("_field");
        
        var getter = propertyContext.DefineGetter();
        getter.Return(fieldContext.InstanceSymbol(getter));
        
        typeContext.Build();
        var instance = Activator.CreateInstance(typeContext.BuildingType);
        var value = TestContext.CurrentContext.Random.Next();
        fieldContext.BuildingField.SetValue(instance, value);
        Assert.That( propertyContext.BuildingProperty.GetValue(instance),
            Is.EqualTo(value));
    }
}