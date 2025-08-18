namespace EmitToolbox.Framework;

public class ActionMethodContext(ILGenerator code) : MethodContext(code)
{
    public void Return()
    {
        Code.Emit(OpCodes.Ret);
    }
}