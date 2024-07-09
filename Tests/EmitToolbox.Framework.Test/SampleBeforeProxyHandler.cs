using System.Reflection.Emit;
using EmitToolbox.Framework.Contexts;

namespace EmitToolbox.Framework.Test;

public class SampleBeforeProxyHandler : IProxyHandler
{
    public Task Process(ClassContext context) 
    {
        var methodContext = context.OverrideMethod(
            typeof(SampleProxyObject).GetMethod(nameof(SampleProxyObject.Add))!)!;
        var code = methodContext.Builder.GetILGenerator();
        
        code.Emit(OpCodes.Ldc_I4_5);
        code.Emit(OpCodes.Starg, 1);

        return Task.CompletedTask;
    }
}