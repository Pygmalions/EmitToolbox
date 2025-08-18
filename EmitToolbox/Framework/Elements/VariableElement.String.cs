namespace EmitToolbox.Framework.Elements;

public static class VariableElementStringExtensions
{
    public static void Assign(this VariableElement<string> target, string value)
    {
        target.Context.Code.Emit(OpCodes.Ldstr, value);
        target.EmitStoreValue();
    }
}