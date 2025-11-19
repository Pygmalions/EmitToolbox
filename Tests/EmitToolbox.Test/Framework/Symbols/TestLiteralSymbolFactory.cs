using System.Reflection;
using EmitToolbox.Symbols;
using EmitToolbox.Symbols.Literals;
using JetBrains.Annotations;

namespace EmitToolbox.Test.Framework.Symbols;

[TestFixture,
 TestOf(typeof(LiteralSymbolFactory))]
public class TestLiteralSymbolFactory
{
    private DynamicAssembly _assembly = null!;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    private Func<TContent> CreateTestMethod<TContent>(Func<DynamicFunction, ISymbol<TContent>> factory)
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<TContent>(
            $"ReturnLiteral_{typeof(TContent)}", []);
        var symbol = factory(method);
        method.Return(symbol);
        type.Build();
        return method.BuildingMethod.CreateDelegate<Func<TContent>>();
    }
    
    private Func<TContent> CreateTestMethod<TContent>(Func<DynamicFunction, ISymbol> factory)
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor(
            $"ReturnLiteral_{typeof(TContent)}", typeof(TContent), []);
        var symbol = factory(method);
        method.Return(symbol);
        type.Build();
        return method.BuildingMethod.CreateDelegate<Func<TContent>>();
    }

    public enum SampleEnum
    {
        A, B, C
    }

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    // ReSharper disable once EmptyConstructor
    private class SampleClass()
    {
        public void Method() {}
        
        // ReSharper disable once RedundantDefaultMemberInitializer
        public int Field = 0;
        
        public int Property { get; set; }
    }
    
    [Test]
    public void Create_FromTypedValue_Primitives()
    {
        var testSByte = TestContext.CurrentContext.Random.NextSByte();
        var functorSByte = CreateTestMethod(
            context => LiteralSymbolFactory.Create(context, testSByte));
        
        var testByte = TestContext.CurrentContext.Random.NextByte();
        var functorByte = CreateTestMethod(
            context => LiteralSymbolFactory.Create(context, testByte));
        
        var testShort = TestContext.CurrentContext.Random.NextShort();
        var functorShort = CreateTestMethod(
            context => LiteralSymbolFactory.Create(context, testShort));
        
        var testUShort = TestContext.CurrentContext.Random.NextUShort();
        var functorUShort = CreateTestMethod(
            context => LiteralSymbolFactory.Create(context, testUShort));
        
        var testInt = TestContext.CurrentContext.Random.Next();
        var functorInt = CreateTestMethod(
            context => LiteralSymbolFactory.Create(context, testInt));
        
        var testUInt = TestContext.CurrentContext.Random.NextUInt();
        var functorUInt = CreateTestMethod(
            context => LiteralSymbolFactory.Create(context, testUInt));
        
        var testLong = TestContext.CurrentContext.Random.NextLong();
        var functorLong = CreateTestMethod(
            context => LiteralSymbolFactory.Create(context, testLong));
        
        var testULong = TestContext.CurrentContext.Random.NextULong();
        var functorULong = CreateTestMethod(
            context => LiteralSymbolFactory.Create(context, testULong));

        var testFloat = TestContext.CurrentContext.Random.NextFloat();
        var functorFloat = CreateTestMethod(
            context => LiteralSymbolFactory.Create(context, testFloat));
        
        var testDouble = TestContext.CurrentContext.Random.NextDouble();
        var functorDouble = CreateTestMethod(
            context => LiteralSymbolFactory.Create(context, testDouble));
        
        var testEnum = TestContext.CurrentContext.Random.NextEnum<SampleEnum>();
        var functorEnum = CreateTestMethod(
            context => LiteralSymbolFactory.Create(context, testEnum));
        
        var testString = TestContext.CurrentContext.Random.GetString(10);
        var functorString = CreateTestMethod(
            context => LiteralSymbolFactory.Create(context, testString));
        
        var testType = typeof(SampleClass);
        var functorType = CreateTestMethod(
            context => LiteralSymbolFactory.Create(context, testType));

        var testConstructorInfo = typeof(SampleClass).GetConstructor(Type.EmptyTypes)!;
        var functorConstructor = CreateTestMethod(
            context => LiteralSymbolFactory.Create(context, testConstructorInfo));
        
        var testMethodInfo = typeof(SampleClass).GetMethod(nameof(SampleClass.Method))!;
        var functorMethod = CreateTestMethod(
            context => LiteralSymbolFactory.Create(context, testMethodInfo));
        
        var testFieldInfo = typeof(SampleClass).GetField(nameof(SampleClass.Field))!;
        var functorField = CreateTestMethod(
            context => LiteralSymbolFactory.Create(context, testFieldInfo));
        
        var testPropertyInfo = typeof(SampleClass).GetProperty(nameof(SampleClass.Property))!;
        var functorProperty = CreateTestMethod(
            context => LiteralSymbolFactory.Create(context, testPropertyInfo));
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(functorSByte(), Is.EqualTo(testSByte));
            Assert.That(functorByte(), Is.EqualTo(testByte));
            Assert.That(functorShort(), Is.EqualTo(testShort));
            Assert.That(functorUShort(), Is.EqualTo(testUShort));
            Assert.That(functorInt(), Is.EqualTo(testInt));
            Assert.That(functorUInt(), Is.EqualTo(testUInt));
            Assert.That(functorLong(), Is.EqualTo(testLong));
            Assert.That(functorULong(), Is.EqualTo(testULong));
            
            Assert.That(functorFloat(), Is.EqualTo(testFloat));
            Assert.That(functorDouble(), Is.EqualTo(testDouble));
            Assert.That(functorString(), Is.EqualTo(testString));
            Assert.That(functorEnum(), Is.EqualTo(testEnum));
            
            Assert.That(functorType(), Is.EqualTo(testType));
            Assert.That(functorConstructor(), Is.EqualTo(testConstructorInfo));
            Assert.That(functorMethod(), Is.EqualTo(testMethodInfo));
            Assert.That(functorField(), Is.EqualTo(testFieldInfo));
            Assert.That(functorProperty(), Is.EqualTo(testPropertyInfo));
        }
    }
    
    [Test]
    public void Create_FromObject_Primitives()
    {
        object testSByte = TestContext.CurrentContext.Random.NextSByte();
        var functorSByte = CreateTestMethod<sbyte>(
            context => LiteralSymbolFactory.Create(context, testSByte));
        
        object testByte = TestContext.CurrentContext.Random.NextByte();
        var functorByte = CreateTestMethod<byte>(
            context => LiteralSymbolFactory.Create(context, testByte));
        
        object testShort = TestContext.CurrentContext.Random.NextShort();
        var functorShort = CreateTestMethod<short>(
            context => LiteralSymbolFactory.Create(context, testShort));
        
        object testUShort = TestContext.CurrentContext.Random.NextUShort();
        var functorUShort = CreateTestMethod<ushort>(
            context => LiteralSymbolFactory.Create(context, testUShort));
        
        object testInt = TestContext.CurrentContext.Random.Next();
        var functorInt = CreateTestMethod<int>(
            context => LiteralSymbolFactory.Create(context, testInt));
        
        object testUInt = TestContext.CurrentContext.Random.NextUInt();
        var functorUInt = CreateTestMethod<uint>(
            context => LiteralSymbolFactory.Create(context, testUInt));
        
        object testLong = TestContext.CurrentContext.Random.NextLong();
        var functorLong = CreateTestMethod<long>(
            context => LiteralSymbolFactory.Create(context, testLong));
        
        object testULong = TestContext.CurrentContext.Random.NextULong();
        var functorULong = CreateTestMethod<ulong>(
            context => LiteralSymbolFactory.Create(context, testULong));

        object testFloat = TestContext.CurrentContext.Random.NextFloat();
        var functorFloat = CreateTestMethod<float>(
            context => LiteralSymbolFactory.Create(context, testFloat));
        
        object testDouble = TestContext.CurrentContext.Random.NextDouble();
        var functorDouble = CreateTestMethod<double>(
            context => LiteralSymbolFactory.Create(context, testDouble));
        
        object testEnum = TestContext.CurrentContext.Random.NextEnum<SampleEnum>();
        var functorEnum = CreateTestMethod<SampleEnum>(
            context => LiteralSymbolFactory.Create(context, testEnum));
        
        object testString = TestContext.CurrentContext.Random.GetString(10);
        var functorString = CreateTestMethod<string>(
            context => LiteralSymbolFactory.Create(context, testString));
        
        object testType = typeof(SampleClass);
        var functorType = CreateTestMethod<Type>(
            context => LiteralSymbolFactory.Create(context, testType));

        object testConstructorInfo = typeof(SampleClass).GetConstructor(Type.EmptyTypes)!;
        var functorConstructor = CreateTestMethod<ConstructorInfo>(
            context => LiteralSymbolFactory.Create(context, testConstructorInfo));
        
        object testMethodInfo = typeof(SampleClass).GetMethod(nameof(SampleClass.Method))!;
        var functorMethod = CreateTestMethod<MethodInfo>(
            context => LiteralSymbolFactory.Create(context, testMethodInfo));
        
        object testFieldInfo = typeof(SampleClass).GetField(nameof(SampleClass.Field))!;
        var functorField = CreateTestMethod<FieldInfo>(
            context => LiteralSymbolFactory.Create(context, testFieldInfo));
        
        object testPropertyInfo = typeof(SampleClass).GetProperty(nameof(SampleClass.Property))!;
        var functorProperty = CreateTestMethod<PropertyInfo>(
            context => LiteralSymbolFactory.Create(context, testPropertyInfo));
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(functorSByte(), Is.EqualTo(testSByte));
            Assert.That(functorByte(), Is.EqualTo(testByte));
            Assert.That(functorShort(), Is.EqualTo(testShort));
            Assert.That(functorUShort(), Is.EqualTo(testUShort));
            Assert.That(functorInt(), Is.EqualTo(testInt));
            Assert.That(functorUInt(), Is.EqualTo(testUInt));
            Assert.That(functorLong(), Is.EqualTo(testLong));
            Assert.That(functorULong(), Is.EqualTo(testULong));
            
            Assert.That(functorFloat(), Is.EqualTo(testFloat));
            Assert.That(functorDouble(), Is.EqualTo(testDouble));
            Assert.That(functorString(), Is.EqualTo(testString));
            Assert.That(functorEnum(), Is.EqualTo(testEnum));
            
            Assert.That(functorType(), Is.EqualTo(testType));
            Assert.That(functorConstructor(), Is.EqualTo(testConstructorInfo));
            Assert.That(functorMethod(), Is.EqualTo(testMethodInfo));
            Assert.That(functorField(), Is.EqualTo(testFieldInfo));
            Assert.That(functorProperty(), Is.EqualTo(testPropertyInfo));
        }
    }
}
