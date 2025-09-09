using System.Reflection.Emit;
using EmitToolbox.Extensions;
using EmitToolbox.Framework;
using EmitToolbox.Framework.Symbols.Extensions;

namespace EmitToolbox.Test.Framework.Extensions;

public class TestDebuggerExtensions
{
    private DynamicAssembly _assembly;

    [SetUp]
    public void Initialize()
    {
        _assembly = DynamicAssembly.DefineExecutable("TestDebuggerExtensions");
    }
    
    [Test]
    public void Debug()
    {
        var typeContext = _assembly.DefineClass("Debug");
        var methodContext = typeContext.FunctorBuilder.DefineStatic("Test", 
            [ParameterDefinition.Value<int>()], ResultDefinition.Value<object>());
        var code = methodContext.Code;

        var variableArgument = code.DeclareLocal(typeof(int));
        code.LoadArgument_0();
        code.StoreLocal(variableArgument);
        code.Debug("Method is invoked", new Dictionary<string, LocalBuilder>()
        {
            { "argument", variableArgument },
        });
        
        methodContext.Return(methodContext.Value(1));
        typeContext.Build();
        var value = TestContext.CurrentContext.Random.Next();
        methodContext.BuildingMethod.Invoke(null, [value]);
    }
}