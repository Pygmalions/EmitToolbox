using System.Reflection.Emit;
using EmitToolbox.Extensions;
using EmitToolbox.Symbols;
using EmitToolbox.Utilities;

namespace EmitToolbox.Test;

[TestFixture, TestOf(typeof(DynamicAssembly)), TestOf(typeof(DynamicType))]
public class TestDynamicType
{
    [Test]
    public void DefineClass_Empty()
    {
        var assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
        var type = assembly.DefineClass("TestClass");

        Assert.DoesNotThrow(() => type.Build());
    }

    [Test]
    public void DefineStruct_Empty()
    {
        var assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
        var type = assembly.DefineStruct("TestStruct");

        Assert.DoesNotThrow(() => type.Build());
    }

    [Test]
    public void BuildingType_BeforeAndAfterBuild()
    {
        var assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
        var type = assembly.DefineClass("BT_BeforeAfter");

        // Before build, BuildingType should be a TypeBuilder
        Assert.That(type.BuildingType, Is.InstanceOf<TypeBuilder>());
        Assert.That(type.IsBuilt, Is.False);

        type.Build();

        // After build, BuildingType should be a runtime Type, not a TypeBuilder
        Assert.That(type.BuildingType, Is.Not.InstanceOf<TypeBuilder>());
        Assert.That(type.BuildingType, Is.InstanceOf<Type>());
        Assert.That(type.IsBuilt, Is.True);
    }

    [Test]
    public void Build_Twice_ShouldThrow()
    {
        var assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
        var type = assembly.DefineClass("BuildTwice");
        type.Build();

        Assert.Throws<InvalidOperationException>(() => type.Build());
    }

    public interface ISampleInterface
    {
        void Method();
    }

    public interface ISampleInterfaceWithParameter
    {
        void Method(int number);
    }

    public interface ISampleInterfaceWithOutParameter
    {
        public delegate void Delegate(out int number);

        void Method(out int number);
    }

    public interface ISampleInterfaceWithRefParameter
    {
        public delegate void Delegate(ref int number);
        
        void Method(ref int number);
    }

    public interface ISampleInterfaceWithInParameter
    {
        public delegate void Delegate(in int number);
        
        void Method(in int number);
    }

    [Test]
    public void OverrideMethod_Parameterless()
    {
        var assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
        var type = assembly.DefineClass(nameof(OverrideMethod_Parameterless));

        var fieldCounter = type.FieldFactory.DefineInstance("Counter", typeof(int));

        type.ImplementInterface(typeof(ISampleInterface));
        var method = type.MethodFactory.Instance.OverrideAction<ISampleInterface>(target => target.Method());
        var symbolThis = method.This();
        var symbolCounter = fieldCounter.SymbolOf<int>(method, symbolThis);
        AssignmentExtensions.AssignValue<int>(symbolCounter, symbolCounter + method.Value(1));
        method.Return();
        type.Build();

        var testInstance = Activator.CreateInstance(type.BuildingType);
        var functor = method.BuildingMethod.CreateDelegate<Action>(testInstance);
        var testTimes = TestContext.CurrentContext.Random.Next(1, 10);
        foreach (var _ in Enumerable.Range(0, testTimes))
            functor();
        Assert.That(fieldCounter.BuildingField.GetValue(testInstance),
            Is.EqualTo(testTimes));
    }

    [Test]
    public void OverrideMethod_Parameter()
    {
        var assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
        var type = assembly.DefineClass(nameof(OverrideMethod_Parameterless));

        var fieldCounter = type.FieldFactory.DefineInstance("Counter", typeof(int));

        type.ImplementInterface(typeof(ISampleInterfaceWithParameter));
        var method =
            type.MethodFactory.Instance.OverrideAction<ISampleInterfaceWithParameter>(target =>
                target.Method(Any<int>.Value));
        var symbolThis = method.This();
        var symbolNumber = method.Argument<int>(0);
        var symbolCounter = fieldCounter.SymbolOf<int>(method, symbolThis);
        AssignmentExtensions.AssignValue<int>(symbolCounter, symbolNumber);
        method.Return();
        type.Build();

        var testInstance = Activator.CreateInstance(type.BuildingType);
        var functor = method.BuildingMethod.CreateDelegate<Action<int>>(testInstance);
        var testNumber = TestContext.CurrentContext.Random.Next();
        functor(testNumber);
        Assert.That(fieldCounter.BuildingField.GetValue(testInstance),
            Is.EqualTo(testNumber));
    }

    [Test]
    public void OverrideMethod_Parameter_Out()
    {
        var assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
        var type = assembly.DefineClass(nameof(OverrideMethod_Parameterless));

        var fieldValue = type.FieldFactory.DefineInstance("Value", typeof(int));

        type.ImplementInterface(typeof(ISampleInterfaceWithOutParameter));
        var method = type.MethodFactory.Instance.OverrideAction(
            typeof(ISampleInterfaceWithOutParameter).GetMethod(nameof(ISampleInterfaceWithOutParameter.Method))!);
        var symbolThis = method.This();
        var symbolArgument = method.Argument<int>(0, ContentModifier.Reference);
        var symbolValue = fieldValue.SymbolOf<int>(method, symbolThis);
        AssignmentExtensions.AssignValue<int>(symbolArgument, symbolValue);
        method.Return();
        type.Build();

        var testInstance = Activator.CreateInstance(type.BuildingType);
        var functor = method.BuildingMethod.CreateDelegate<ISampleInterfaceWithOutParameter.Delegate>(
            testInstance);
        var testNumber = TestContext.CurrentContext.Random.Next();
        fieldValue.BuildingField.SetValue(testInstance, testNumber);
        var result = 0;
        Assert.DoesNotThrow(() => functor(out result));
        Assert.That(result, Is.EqualTo(testNumber));
    }
    
    [Test]
    public void OverrideMethod_Parameter_Ref()
    {
        var assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
        var type = assembly.DefineClass(nameof(OverrideMethod_Parameterless));

        var fieldValue = type.FieldFactory.DefineInstance("Value", typeof(int));

        type.ImplementInterface(typeof(ISampleInterfaceWithRefParameter));
        var method = type.MethodFactory.Instance.OverrideAction(
            typeof(ISampleInterfaceWithRefParameter).GetMethod(nameof(ISampleInterfaceWithRefParameter.Method))!);
        var symbolThis = method.This();
        var symbolArgument = method.Argument<int>(0, ContentModifier.Reference);
        var symbolValue = fieldValue.SymbolOf<int>(method, symbolThis);
        AssignmentExtensions.AssignValue<int>(symbolArgument, symbolValue);
        method.Return();
        type.Build();

        var testInstance = Activator.CreateInstance(type.BuildingType);
        var functor = method.BuildingMethod.CreateDelegate<ISampleInterfaceWithRefParameter.Delegate>(
            testInstance);
        var testNumber = TestContext.CurrentContext.Random.Next();
        fieldValue.BuildingField.SetValue(testInstance, testNumber);
        var result = 0;
        Assert.DoesNotThrow(() => functor(ref result));
        Assert.That(result, Is.EqualTo(testNumber));
    }
    
    [Test]
    public void OverrideMethod_Parameter_In()
    {
        var assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
        var type = assembly.DefineClass(nameof(OverrideMethod_Parameterless));

        var fieldValue = type.FieldFactory.DefineInstance("Value", typeof(int));

        type.ImplementInterface(typeof(ISampleInterfaceWithInParameter));
        var method = type.MethodFactory.Instance.OverrideAction(
            typeof(ISampleInterfaceWithInParameter).GetMethod(
                nameof(ISampleInterfaceWithInParameter.Method))!);
        var symbolThis = method.This();
        var symbolArgument = method.Argument<int>(0, ContentModifier.Reference);
        var symbolValue = fieldValue.SymbolOf<int>(method, symbolThis);
        AssignmentExtensions.AssignValue<int>(symbolValue, symbolArgument);
        method.Return();
        type.Build();

        var testInstance = Activator.CreateInstance(type.BuildingType);
        var functor = method.BuildingMethod.CreateDelegate<ISampleInterfaceWithInParameter.Delegate>(
            testInstance);
        var testNumber = TestContext.CurrentContext.Random.Next();
        Assert.DoesNotThrow(() => functor(in testNumber));
        Assert.That(fieldValue.BuildingField.GetValue(testInstance),
            Is.EqualTo(testNumber));
    }
}