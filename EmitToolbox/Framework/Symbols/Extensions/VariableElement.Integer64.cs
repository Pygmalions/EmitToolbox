namespace EmitToolbox.Framework.Symbols.Extensions;

public static class VariableElementInteger64Extensions
{
    public static void Assign(this VariableSymbol<long> target, long value)
    {
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.EmitStoreFromValue();
    }

    public static void SelfAdd(this VariableSymbol<long> target, ValueSymbol<long> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        target.EmitStoreFromValue();
    }

    public static void SelfAdd(this VariableSymbol<long> target, long value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Add);
        target.EmitStoreFromValue();
    }

    public static void SelfSubtract(this VariableSymbol<long> target, ValueSymbol<long> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        target.EmitStoreFromValue();
    }

    public static void SelfSubtract(this VariableSymbol<long> target, long value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Sub);
        target.EmitStoreFromValue();
    }

    public static void SelfMultiply(this VariableSymbol<long> target, ValueSymbol<long> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        target.EmitStoreFromValue();
    }

    public static void SelfMultiply(this VariableSymbol<long> target, long value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Mul);
        target.EmitStoreFromValue();
    }

    public static void SelfDivide(this VariableSymbol<long> target, ValueSymbol<long> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        target.EmitStoreFromValue();
    }

    public static void SelfDivide(this VariableSymbol<long> target, long value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Div);
        target.EmitStoreFromValue();
    }

    public static void SelfModulus(this VariableSymbol<long> target, ValueSymbol<long> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        target.EmitStoreFromValue();
    }

    public static void SelfModulus(this VariableSymbol<long> target, long value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Rem);
        target.EmitStoreFromValue();
    }
}
