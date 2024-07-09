using System.Reflection.Emit;
using EmitToolbox.Framework.Contexts;

namespace EmitToolbox.Framework.Test;

public class SampleAfterProxyHandler : IProxyHandler
{
    public async Task Process(ClassContext context)
    {
        var methodContext = context.OverrideMethod(
            typeof(SampleProxyObject).GetMethod(nameof(SampleProxyObject.Add))!)!;
        var code = methodContext.Builder.GetILGenerator();

        await methodContext.EmittingInvocation;

        code.Emit(OpCodes.Ldloc, methodContext.Result!);
        code.Emit(OpCodes.Ldc_I4_1);
        code.Emit(OpCodes.Add);
        code.Emit(OpCodes.Stloc, methodContext.Result!);
    }
}