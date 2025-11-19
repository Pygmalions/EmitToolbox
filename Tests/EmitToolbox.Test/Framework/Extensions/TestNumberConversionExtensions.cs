using EmitToolbox.Extensions;
using EmitToolbox.Symbols;

namespace EmitToolbox.Test.Framework.Extensions;

[TestFixture]
public class TestNumberConversionExtensions
{
    private DynamicAssembly _assembly;
    
    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }

    private Func<TFrom, TTo> CreateConverterMethod<TFrom, TTo>(
        Func<ISymbol<TFrom>, ISymbol<TTo>> transform)
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineFunctor<TTo>(
            $"Convert_{typeof(TFrom).Name}_{typeof(TTo).Name}", 
            [ParameterDefinition.Value<TFrom>("value")]);
        method.Return(transform(method.Argument<TFrom>(0)));
        type.Build();
        return method.BuildingMethod.CreateDelegate<Func<TFrom, TTo>>();
    }

    [Test]
    public void Converter_Int_To_UInt()
    {
        var converter = CreateConverterMethod<int, uint>(
            symbol => symbol.ToUInt32());
        var number = TestContext.CurrentContext.Random.Next();
        var result = converter(number);
        Assert.That(result, Is.EqualTo((uint)number));
    }
}
