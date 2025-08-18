using EmitToolbox.Framework.Elements;

namespace EmitToolbox.Framework;

public class FunctorMethodContext(MethodBuilder builder, ILGenerator code) : MethodContext(builder, code)
{
    public void Return(ValueElement result)
    {
        result.EmitLoadAsValue();
        Code.Emit(OpCodes.Ret);
    }
}

public class FunctorMethodContext<TResult>(MethodBuilder builder, ILGenerator code) : MethodContext(builder, code)
{
    public void Return(ValueElement<TResult> result)
    {
        result.EmitLoadAsValue();
        Code.Emit(OpCodes.Ret);
    }
}