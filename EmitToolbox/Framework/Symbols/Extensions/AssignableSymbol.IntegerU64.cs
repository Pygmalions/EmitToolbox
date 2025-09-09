namespace EmitToolbox.Framework.Symbols.Extensions;

public static class AssignableSymbolUnsignedInteger64Extensions
{
    public static void Assign(this IAssignableSymbol<ulong> target, ulong value)
    {
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.EmitStoreFromValue();
    }

    public static void SelfAdd(this IAssignableSymbol<ulong> target, ISymbol<ulong> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        target.EmitStoreFromValue();
    }

    public static void SelfAdd(this IAssignableSymbol<ulong> target, ulong value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Add);
        target.EmitStoreFromValue();
    }

    public static void SelfSubtract(this IAssignableSymbol<ulong> target, ISymbol<ulong> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        target.EmitStoreFromValue();
    }

    public static void SelfSubtract(this IAssignableSymbol<ulong> target, ulong value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Sub);
        target.EmitStoreFromValue();
    }

    public static void SelfMultiply(this IAssignableSymbol<ulong> target, ISymbol<ulong> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        target.EmitStoreFromValue();
    }

    public static void SelfMultiply(this IAssignableSymbol<ulong> target, ulong value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Mul);
        target.EmitStoreFromValue();
    }

    public static void SelfDivide(this IAssignableSymbol<ulong> target, ISymbol<ulong> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        target.EmitStoreFromValue();
    }

    public static void SelfDivide(this IAssignableSymbol<ulong> target, ulong value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Div);
        target.EmitStoreFromValue();
    }

    public static void SelfModulus(this IAssignableSymbol<ulong> target, ISymbol<ulong> value)
    {
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        target.EmitStoreFromValue();
    }

    public static void SelfModulus(this IAssignableSymbol<ulong> target, ulong value)
    {
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_I8, value);
        target.Context.Code.Emit(OpCodes.Rem);
        target.EmitStoreFromValue();
    }
}
