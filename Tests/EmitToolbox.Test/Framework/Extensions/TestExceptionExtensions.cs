using EmitToolbox.Extensions;
using EmitToolbox.Symbols;
using EmitToolbox.Utilities;

namespace EmitToolbox.Test.Framework.Extensions;

[TestFixture, TestOf(typeof(ExceptionExtensions))]
public class TestExceptionExtensions
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }
    
    [Test]
    public void Throw_WithoutMessage()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineAction(nameof(Throw_WithoutMessage));
        method.ThrowException();
        method.Return();
        
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Action>();
        Assert.Throws<Exception>(() => functor());
    }
    
    [Test]
    public void Throw_WithoutMessage_Generic()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineAction(nameof(Throw_WithoutMessage_Generic));
        method.ThrowException<ArgumentException>();
        method.Return();
        
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Action>();
        Assert.Throws<ArgumentException>(() => functor());
    }
    
    [Test]
    public void Throw_WithMessage()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineAction(nameof(Throw_WithoutMessage));
        method.ThrowException("Test");
        method.Return();
        
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Action>();
        Assert.Throws<Exception>(() => functor(), "Test");
    }
    
    [Test]
    public void Throw_WithMessage_Generic()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineAction(nameof(Throw_WithoutMessage_Generic));
        method.ThrowException<ArgumentException>("Test");
        method.Return();
        
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Action>();
        Assert.Throws<ArgumentException>(() => functor(), "Test");
    }
    
    [Test]
    public void Throw_WithMessage_Selector()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineAction(nameof(Throw_WithoutMessage_Generic));
        method.ThrowException(() => new ArgumentException(Any<string>.Value),
            [method.Value("Test")]);
        method.Return();
        
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Action>();
        Assert.Throws<ArgumentException>(() => functor(), "Test");
    }
}