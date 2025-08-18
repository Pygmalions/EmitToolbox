namespace EmitToolbox.Framework.Elements;

public static class VariableElementBooleanExtensions
{
    public static void Assign(this VariableElement<bool> target, bool value)
    {
        target.Context.Code.Emit(value ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
        target.EmitStoreValue();
    }
}