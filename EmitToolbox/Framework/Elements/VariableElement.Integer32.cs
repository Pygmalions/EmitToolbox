namespace EmitToolbox.Framework.Elements;

public static class VariableElementInteger32Extensions
{
    public static void Assign(this VariableElement<int> target, int value)
    {
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.EmitStoreValue();
    }
    
    public static void SelfAdd(this VariableElement<int> target, ValueElement<int> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        target.EmitStoreValue();
    }
    
    public static void SelfAdd(this VariableElement<int> target, int value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Add);
        target.EmitStoreValue();
    }
    
    public static void SelfSubtract(this VariableElement<int> target, ValueElement<int> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        target.EmitStoreValue();
    }

    public static void SelfSubtract(this VariableElement<int> target, int value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Sub);
        target.EmitStoreValue();
    }

    public static void SelfMultiply(this VariableElement<int> target, ValueElement<int> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        target.EmitStoreValue();
    }

    public static void SelfMultiply(this VariableElement<int> target, int value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Mul);
        target.EmitStoreValue();
    }

    public static void SelfDivide(this VariableElement<int> target, ValueElement<int> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        target.EmitStoreValue();
    }

    public static void SelfDivide(this VariableElement<int> target, int value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Div);
        target.EmitStoreValue();
    }

    public static void SelfModulus(this VariableElement<int> target, ValueElement<int> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        target.EmitStoreValue();
    }

    public static void SelfModulus(this VariableElement<int> target, int value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Rem);
        target.EmitStoreValue();
    }
}