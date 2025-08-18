using EmitToolbox.Framework.Elements;

namespace EmitToolbox.Framework;

public class FunctorMethodContext<TResult>(ILGenerator code) : MethodContext(code)
{
    public void Return(ValueElement<TResult> result)
    {
        result.EmitLoadAsValue();
        Code.Emit(OpCodes.Ret);
    }
}