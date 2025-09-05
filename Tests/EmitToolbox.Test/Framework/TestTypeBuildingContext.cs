using EmitToolbox.Framework;

namespace EmitToolbox.Test.Framework;

[TestFixture]
public class TestTypeBuildingContext
{
    private AssemblyBuildingContext _assembly;

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
        var fieldContext = typeContext.Fields.Instance<int>("TestField");
        var methodContext = typeContext.Actions.Instance("FieldSetter",
            [ParameterDefinition.Value<int>()]);
        var argument = methodContext.Argument<int>(0);
        fieldContext.Symbol(methodContext.This).Assign(argument);
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
        var fieldContext = typeContext.Fields.Instance<int>("TestField");
        var methodContext = typeContext.Functors.Instance("FieldGetter",
            [], ResultDefinition.Value<int>());
        var field = fieldContext.Symbol(methodContext.This);
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
        var fieldContext = typeContext.Fields.Static<int>("TestField");
        var methodContext = typeContext.Actions.Static("FieldSetter",
            [ParameterDefinition.Value<int>()]);
        var argument = methodContext.Argument<int>(0);
        fieldContext.Symbol(methodContext).Assign(argument);
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
        var fieldContext = typeContext.Fields.Static<int>("TestField");
        var methodContext = typeContext.Functors.Static("FieldGetter",
            [], ResultDefinition.Value<int>());
        var field = fieldContext.Symbol(methodContext);
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
        var propertyContext = typeContext.Properties.Instance<int>("TestProperty");
        var fieldContext = typeContext.Fields.Instance<int>("_field");
        
        var setter = propertyContext.DefineSetter();
        fieldContext.Symbol(setter.This).Assign(setter.Argument<int>(0));
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
        var propertyContext = typeContext.Properties.Instance<int>("TestProperty");
        var fieldContext = typeContext.Fields.Instance<int>("_field");
        
        var getter = propertyContext.DefineGetter();
        getter.Return(fieldContext.Symbol(getter.This));
        
        typeContext.Build();
        var instance = Activator.CreateInstance(typeContext.BuildingType);
        var value = TestContext.CurrentContext.Random.Next();
        fieldContext.BuildingField.SetValue(instance, value);
        Assert.That( propertyContext.BuildingProperty.GetValue(instance),
            Is.EqualTo(value));
    }
}