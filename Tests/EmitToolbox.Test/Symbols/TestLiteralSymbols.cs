using System.Reflection;
using EmitToolbox.Symbols;
using EmitToolbox.Symbols.Literals;
using JetBrains.Annotations;

#pragma warning disable NUnit20210 // This has false positive.

namespace EmitToolbox.Test.Symbols;

[TestFixture,
 TestOf(typeof(LiteralStringSymbol)),
 TestOf(typeof(LiteralBooleanSymbol)),
 TestOf(typeof(LiteralEnumSymbol<>)),
 TestOf(typeof(LiteralIntegerCharacterSymbol)),
 TestOf(typeof(LiteralInteger16Symbol)),
 TestOf(typeof(LiteralInteger32Symbol)),
 TestOf(typeof(LiteralInteger64Symbol)),
 TestOf(typeof(LiteralUnsignedInteger16Symbol)),
 TestOf(typeof(LiteralUnsignedInteger32Symbol)),
 TestOf(typeof(LiteralUnsignedInteger64Symbol)),
 TestOf(typeof(LiteralFloat32Symbol)),
 TestOf(typeof(LiteralFloat64Symbol)),
 TestOf(typeof(LiteralDecimalSymbol)),
 TestOf(typeof(LiteralNullSymbol)),
 TestOf(typeof(LiteralTypeInfoSymbol)),
 TestOf(typeof(LiteralMethodInfoSymbol)),
 TestOf(typeof(LiteralConstructorInfoSymbol)),
 TestOf(typeof(LiteralFieldInfoSymbol)),
 TestOf(typeof(LiteralPropertyInfoSymbol)),
]
public class TestLiteralSymbols
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    private Func<TResult> CreateTestMethod<TResult>(string name, Func<DynamicFunction, ISymbol<TResult>> makeValue)
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<TResult>(name, []);
        var valueSymbol = makeValue(method);
        method.Return(valueSymbol);
        type.Build();
        return method.BuildingMethod.CreateDelegate<Func<TResult>>();
    }

    [Test]
    public void Return_Literal_Bool()
    {
        foreach (var value in new[] { false, true })
        {
            var functor = CreateTestMethod(
                $"ReturnLiteral_Bool_{value}", method => method.Literal(value));
            Assert.That(functor(), Is.EqualTo(value));
        }
    }

    [Test]
    public void Return_Literal_String()
    {
        var value = TestContext.CurrentContext.Random.GetString(
            TestContext.CurrentContext.Random.Next(1, 20));
        var functor = CreateTestMethod("ReturnLiteral_String",
            method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_Char()
    {
        var value = (char)TestContext.CurrentContext.Random.Next(char.MinValue, char.MaxValue);
        var functor = CreateTestMethod("ReturnLiteral_Char",
            method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_Short()
    {
        var value = TestContext.CurrentContext.Random.NextShort();
        var functor = CreateTestMethod(
            "ReturnLiteral_Short", method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_UShort()
    {
        var value = TestContext.CurrentContext.Random.NextUShort();
        var functor = CreateTestMethod(
            "ReturnLiteral_UShort", method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_Int()
    {
        var value = TestContext.CurrentContext.Random.Next();
        var functor = CreateTestMethod(
            "ReturnLiteral_Int", method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_UInt()
    {
        var value = TestContext.CurrentContext.Random.NextUInt();
        var functor = CreateTestMethod(
            "ReturnLiteral_UInt", method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_Long()
    {
        var value = TestContext.CurrentContext.Random.NextLong();
        var functor = CreateTestMethod(
            "ReturnLiteral_Long", method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_ULong()
    {
        var value = TestContext.CurrentContext.Random.NextULong();
        var functor = CreateTestMethod(
            "ReturnLiteral_ULong", method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_Float()
    {
        var value = TestContext.CurrentContext.Random.NextFloat();
        var functor = CreateTestMethod(
            "ReturnLiteral_Float", method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_Double()
    {
        var value = TestContext.CurrentContext.Random.NextDouble();
        var functor = CreateTestMethod(
            "ReturnLiteral_Double", method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_Decimal()
    {
        var value = TestContext.CurrentContext.Random.NextDecimal();
        var functor = CreateTestMethod(
            "ReturnLiteral_Decimal", method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.EqualTo(value));
    }

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    private enum SampleIntEnum
    {
        Zero = 0,
        One = 1,
        MinusOne = -1,
        Big = 123456789
    }

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    private enum SampleLongEnum : long
    {
        A = 0,
        B = long.MaxValue,
        C = -1
    }

    [Test]
    public void Return_Literal_Enum_Int_Underlying()
    {
        var value = TestContext.CurrentContext.Random.NextEnum<SampleIntEnum>();
        var functor = CreateTestMethod(
            "ReturnEnumInt", method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_Enum_Long_Underlying()
    {
        var value = TestContext.CurrentContext.Random.NextEnum<SampleLongEnum>();
        var functor = CreateTestMethod(
            "ReturnEnumInt", method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_Null()
    {
        var functor = CreateTestMethod(
            "ReturnNull", method => method.Null<string>());
        Assert.That(functor(), Is.Null);
    }

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class SampleClass
    {
        public SampleClass()
        {
        }

        public SampleClass(int argument)
        {
        }
        
        protected SampleClass(long argument)
        {
        }

        protected SampleClass(string argument)
        {
        }

        public int Field;

        public int Property { get; set; }

        public static int StaticField;

        public static int StaticProperty { get; set; }

        protected int ProtectedField;

        protected int ProtectedProperty { get; set; }

        protected static int ProtectedStaticField;

        protected static int ProtectedStaticProperty { get; set; }

        public static void StaticMethod()
        {
        }

        public void InstanceMethod(int argument)
        {
        }

        public void InstanceMethod(string argument)
        {
        }

        protected static void ProtectedStaticMethod()
        {
        }

        protected void ProtectedInstanceMethod(int argument)
        {
        }

        protected void ProtectedInstanceMethod(string argument)
        {
        }
    }

    [Test]
    public void Return_Literal_TypeInfo()
    {
        var functor = CreateTestMethod("ReturnLiteral_Type",
            method => method.Literal(typeof(SampleClass)));
        var result = functor();
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(typeof(SampleClass)));
    }

    [Test]
    public void Return_Literal_MethodInfo_Static()
    {
        var value = typeof(SampleClass).GetMethod(nameof(SampleClass.StaticMethod))!;
        var functor = CreateTestMethod("ReturnLiteral_MethodInfo_Static",
            method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_MethodInfo_Instance()
    {
        var value = typeof(SampleClass).GetMethod(nameof(SampleClass.InstanceMethod),
            [typeof(string)])!;
        var functor = CreateTestMethod("ReturnLiteral_MethodInfo_Static",
            method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_ConstructorInfo_Default()
    {
        var value = typeof(SampleClass).GetConstructor(Type.EmptyTypes)!;
        var functor = CreateTestMethod(
            "ReturnLiteral_ConstructorInfo_Default",
            method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_ConstructorInfo_Argument()
    {
        var value = typeof(SampleClass).GetConstructor([typeof(int)])!;
        var functor = CreateTestMethod(
            "ReturnLiteral_ConstructorInfo_Argument",
            method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_FieldInfo_Static()
    {
        var value = typeof(SampleClass).GetField(nameof(SampleClass.StaticField),
            BindingFlags.Static | BindingFlags.Public)!;
        var functor = CreateTestMethod(
            "ReturnLiteral_FieldInfo_Static", method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_FieldInfo_Instance()
    {
        var value = typeof(SampleClass).GetField(nameof(SampleClass.Field))!;
        var functor = CreateTestMethod(
            "ReturnLiteral_FieldInfo_Instance", method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_PropertyInfo_Static()
    {
        var value = typeof(SampleClass).GetProperty(nameof(SampleClass.StaticProperty),
            BindingFlags.Static | BindingFlags.Public)!;
        var functor = CreateTestMethod(
            "ReturnLiteral_PropertyInfo_Static", method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_PropertyInfo_Instance()
    {
        var value = typeof(SampleClass).GetProperty(nameof(SampleClass.Property))!;
        var functor = CreateTestMethod(
            "ReturnLiteral_PropertyInfo_Instance", method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_ConstructorInfo_Protected()
    {
        var value = typeof(SampleClass).GetConstructor(
            BindingFlags.NonPublic | BindingFlags.Instance,
            null, [typeof(string)], null)!;
        var functor = CreateTestMethod(
            "ReturnLiteral_ConstructorInfo_Protected",
            method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_MethodInfo_Protected_Static()
    {
        var value = typeof(SampleClass).GetMethod("ProtectedStaticMethod",
            BindingFlags.NonPublic | BindingFlags.Static)!;
        var functor = CreateTestMethod("ReturnLiteral_MethodInfo_Protected_Static",
            method => method.Literal(value)); 
        var result = functor();
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_MethodInfo_Protected_Instance()
    {
        var value = typeof(SampleClass).GetMethod("ProtectedInstanceMethod",
            BindingFlags.NonPublic | BindingFlags.Instance,
            null, [typeof(string)], null)!;
        var functor = CreateTestMethod("ReturnLiteral_MethodInfo_Protected_Instance",
            method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_FieldInfo_Protected_Static()
    {
        var value = typeof(SampleClass).GetField("ProtectedStaticField",
            BindingFlags.NonPublic | BindingFlags.Static)!;
        var functor = CreateTestMethod(
            "ReturnLiteral_FieldInfo_Protected_Static", method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.Not.Null);

        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_FieldInfo_Protected_Instance()
    {
        var value = typeof(SampleClass).GetField("ProtectedField",
            BindingFlags.NonPublic | BindingFlags.Instance)!;
        var functor = CreateTestMethod(
            "ReturnLiteral_FieldInfo_Protected_Instance", method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_PropertyInfo_Protected_Static()
    {
        var value = typeof(SampleClass).GetProperty("ProtectedStaticProperty",
            BindingFlags.NonPublic | BindingFlags.Static)!;
        var functor = CreateTestMethod(
            "ReturnLiteral_PropertyInfo_Protected_Static", method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Return_Literal_PropertyInfo_Protected_Instance()
    {
        var value = typeof(SampleClass).GetProperty("ProtectedProperty",
            BindingFlags.NonPublic | BindingFlags.Instance)!;
        var functor = CreateTestMethod(
            "ReturnLiteral_PropertyInfo_Protected_Instance", 
            method => method.Literal(value));
        var result = functor();
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(value));
    }
}