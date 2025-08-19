namespace EmitToolbox.Framework.Symbols.Extensions;

public static class VariableElementFloatExtensions
{
    public static void Assign(this VariableSymbol<float> target, float value)
    {
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.EmitStoreFromValue();
    }

    public static void SelfAdd(this VariableSymbol<float> target, ValueSymbol<float> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        target.EmitStoreFromValue();
    }

    public static void SelfAdd(this VariableSymbol<float> target, float value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.Context.Code.Emit(OpCodes.Add);
        target.EmitStoreFromValue();
    }

    public static void SelfSubtract(this VariableSymbol<float> target, ValueSymbol<float> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        target.EmitStoreFromValue();
    }

    public static void SelfSubtract(this VariableSymbol<float> target, float value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.Context.Code.Emit(OpCodes.Sub);
        target.EmitStoreFromValue();
    }

    public static void SelfMultiply(this VariableSymbol<float> target, ValueSymbol<float> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        target.EmitStoreFromValue();
    }

    public static void SelfMultiply(this VariableSymbol<float> target, float value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.Context.Code.Emit(OpCodes.Mul);
        target.EmitStoreFromValue();
    }

    public static void SelfDivide(this VariableSymbol<float> target, ValueSymbol<float> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        target.EmitStoreFromValue();
    }

    public static void SelfDivide(this VariableSymbol<float> target, float value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.Context.Code.Emit(OpCodes.Div);
        target.EmitStoreFromValue();
    }

    public static void SelfModulus(this VariableSymbol<float> target, ValueSymbol<float> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        target.EmitStoreFromValue();
    }

    public static void SelfModulus(this VariableSymbol<float> target, float value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R4, value);
        target.Context.Code.Emit(OpCodes.Rem);
        target.EmitStoreFromValue();
    }
}
