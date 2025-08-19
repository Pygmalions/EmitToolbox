namespace EmitToolbox.Framework.Symbols.Extensions;

public static class VariableElementInteger32Extensions
{
    public static void Assign(this VariableSymbol<int> target, int value)
    {
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.EmitStoreFromValue();
    }
    
    public static void SelfAdd(this VariableSymbol<int> target, ValueSymbol<int> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        target.EmitStoreFromValue();
    }
    
    public static void SelfAdd(this VariableSymbol<int> target, int value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Add);
        target.EmitStoreFromValue();
    }
    
    public static void SelfSubtract(this VariableSymbol<int> target, ValueSymbol<int> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        target.EmitStoreFromValue();
    }

    public static void SelfSubtract(this VariableSymbol<int> target, int value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Sub);
        target.EmitStoreFromValue();
    }

    public static void SelfMultiply(this VariableSymbol<int> target, ValueSymbol<int> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        target.EmitStoreFromValue();
    }

    public static void SelfMultiply(this VariableSymbol<int> target, int value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Mul);
        target.EmitStoreFromValue();
    }

    public static void SelfDivide(this VariableSymbol<int> target, ValueSymbol<int> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        target.EmitStoreFromValue();
    }

    public static void SelfDivide(this VariableSymbol<int> target, int value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Div);
        target.EmitStoreFromValue();
    }

    public static void SelfModulus(this VariableSymbol<int> target, ValueSymbol<int> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        target.EmitStoreFromValue();
    }

    public static void SelfModulus(this VariableSymbol<int> target, int value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I4, value);
        target.Context.Code.Emit(OpCodes.Rem);
        target.EmitStoreFromValue();
    }
}