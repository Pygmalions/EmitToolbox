using EmitToolbox.Framework;
using EmitToolbox.Framework.Extensions;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Test.Framework.Extensions;

[TestFixture, TestOf(typeof(ConsoleExtensions))]
public class TestConsoleExtensions
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Setup()
    {
        _assembly = DynamicAssembly.DefineExecutable(Guid.CreateVersion7().ToString());
    }
    
    [Test]
    public void WriteLine()
    {
        var type = _assembly.DefineClass(Guid.CreateVersion7().ToString());
        var method = type.MethodFactory.Static.DefineAction("WriteLine",
            [typeof(int), typeof(string)]);
        var argumentArgument0 = method.Argument<int>(0);
        var argumentArgument1 = method.Argument<string>(1);
        method.Console.WriteLine("Test, {0}, {1}", 
            argumentArgument0, argumentArgument1);
        method.Return();
        type.Build();
        var functor = method.BuildingMethod.CreateDelegate<Action<int, string>>();
        Assert.DoesNotThrow(() => functor(1, "Test"));
    }
}