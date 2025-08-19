namespace EmitToolbox.Framework.Symbols.Extensions;

public static class VariableElementDoubleExtensions
{
    public static void Assign(this VariableSymbol<double> target, double value)
    {
        target.Context.Code.Emit(OpCodes.Ldc_R8, value);
        target.EmitStoreFromValue();
    }

    public static void SelfAdd(this VariableSymbol<double> target, ValueSymbol<double> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        target.EmitStoreFromValue();
    }

    public static void SelfAdd(this VariableSymbol<double> target, double value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R8, value);
        target.Context.Code.Emit(OpCodes.Add);
        target.EmitStoreFromValue();
    }

    public static void SelfSubtract(this VariableSymbol<double> target, ValueSymbol<double> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        target.EmitStoreFromValue();
    }

    public static void SelfSubtract(this VariableSymbol<double> target, double value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R8, value);
        target.Context.Code.Emit(OpCodes.Sub);
        target.EmitStoreFromValue();
    }

    public static void SelfMultiply(this VariableSymbol<double> target, ValueSymbol<double> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        target.EmitStoreFromValue();
    }

    public static void SelfMultiply(this VariableSymbol<double> target, double value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R8, value);
        target.Context.Code.Emit(OpCodes.Mul);
        target.EmitStoreFromValue();
    }

    public static void SelfDivide(this VariableSymbol<double> target, ValueSymbol<double> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        target.EmitStoreFromValue();
    }

    public static void SelfDivide(this VariableSymbol<double> target, double value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R8, value);
        target.Context.Code.Emit(OpCodes.Div);
        target.EmitStoreFromValue();
    }

    public static void SelfModulus(this VariableSymbol<double> target, ValueSymbol<double> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        target.EmitStoreFromValue();
    }

    public static void SelfModulus(this VariableSymbol<double> target, double value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R8, value);
        target.Context.Code.Emit(OpCodes.Rem);
        target.EmitStoreFromValue();
    }
}
