using EmitToolbox.Framework;
using EmitToolbox.Framework.Symbols.Extensions;

namespace EmitToolbox.Test.Framework;

[TestFixture]
public class TestTypeBuildingContext
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Initialize()
    {
        _assembly = DynamicAssembly.DefineExecutable("TestTypeBuildingContext");
    }
    
    [Test]
    public void InstanceField_Set()
    {
        var typeContext = _assembly.DefineClass("InstanceField_Set");
        var fieldContext = typeContext.FieldBuilder.DefineInstance("TestField", typeof(int));
        var methodContext = typeContext.ActionBuilder.DefineInstance("FieldSetter",
            [ParameterDefinition.Value<int>()]);
        var argument = methodContext.Argument<int>(0);
        fieldContext.SymbolOf(methodContext.This).AssignFrom(argument);
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
        var fieldContext = typeContext.FieldBuilder.DefineInstance("TestField", typeof(int));
        var methodContext = typeContext.FunctorBuilder.DefineInstance("FieldGetter",
            [], ResultDefinition.Value<int>());
        var field = fieldContext.SymbolOf(methodContext.This);
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
        var fieldContext = typeContext.FieldBuilder.DefineStatic("TestField", typeof(int));
        var methodContext = typeContext.ActionBuilder.DefineStatic("FieldSetter",
            [ParameterDefinition.Value<int>()]);
        var argument = methodContext.Argument<int>(0);
        fieldContext.SymbolOf(methodContext).AssignFrom(argument);
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
        var fieldContext = typeContext.FieldBuilder.DefineStatic("TestField", typeof(int));
        var methodContext = typeContext.FunctorBuilder.DefineStatic("FieldGetter",
            [], ResultDefinition.Value<int>());
        var field = fieldContext.SymbolOf(methodContext);
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
        var propertyContext = typeContext.PropertyBuilder.DefineInstance("TestProperty", typeof(int));
        var fieldContext = typeContext.FieldBuilder.DefineInstance("_field", typeof(int));
        
        var setter = propertyContext.DefineSetter();
        fieldContext.SymbolOf(setter.This).AssignFrom(setter.Argument<int>(0));
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
        var propertyContext = typeContext.PropertyBuilder.DefineInstance("TestProperty", typeof(int));
        var fieldContext = typeContext.FieldBuilder.DefineInstance("_field", typeof(int));
        
        var getter = propertyContext.DefineGetter();
        getter.Return(fieldContext.SymbolOf(getter.This));
        
        typeContext.Build();
        var instance = Activator.CreateInstance(typeContext.BuildingType);
        var value = TestContext.CurrentContext.Random.Next();
        fieldContext.BuildingField.SetValue(instance, value);
        Assert.That( propertyContext.BuildingProperty.GetValue(instance),
            Is.EqualTo(value));
    }
}