namespace EmitToolbox.Framework;

public class ActionMethodContext(MethodBuilder builder, ILGenerator code) : MethodContext(builder, code)
{
    public void Return()
    {
        Code.Emit(OpCodes.Ret);
    }
}