namespace EmitToolbox.Framework.Elements;

public static class VariableElementIntegerU64Extensions
{
    public static void Assign(this VariableElement<ulong> target, ulong value)
    {
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.EmitStoreValue();
    }

    public static void SelfAdd(this VariableElement<ulong> target, ValueElement<ulong> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        target.EmitStoreValue();
    }

    public static void SelfAdd(this VariableElement<ulong> target, ulong value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Add);
        target.EmitStoreValue();
    }

    public static void SelfSubtract(this VariableElement<ulong> target, ValueElement<ulong> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        target.EmitStoreValue();
    }

    public static void SelfSubtract(this VariableElement<ulong> target, ulong value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Sub);
        target.EmitStoreValue();
    }

    public static void SelfMultiply(this VariableElement<ulong> target, ValueElement<ulong> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        target.EmitStoreValue();
    }

    public static void SelfMultiply(this VariableElement<ulong> target, ulong value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Mul);
        target.EmitStoreValue();
    }

    public static void SelfDivide(this VariableElement<ulong> target, ValueElement<ulong> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        target.EmitStoreValue();
    }

    public static void SelfDivide(this VariableElement<ulong> target, ulong value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Div);
        target.EmitStoreValue();
    }

    public static void SelfModulus(this VariableElement<ulong> target, ValueElement<ulong> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        target.EmitStoreValue();
    }

    public static void SelfModulus(this VariableElement<ulong> target, ulong value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Rem);
        target.EmitStoreValue();
    }
}
