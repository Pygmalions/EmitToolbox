namespace EmitToolbox.Framework.Symbols.Extensions;

public static class ValueElementDoubleExtensions
{
    public static VariableSymbol<double> Add(this ValueSymbol<double> target, ValueSymbol<double> value)
    {
        var result = target.Context.Variable<double>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<double> Add(this ValueSymbol<double> target, double value)
    {
        var result = target.Context.Variable<double>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R8, value);
        target.Context.Code.Emit(OpCodes.Add);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<double> Subtract(this ValueSymbol<double> target, ValueSymbol<double> value)
    {
        var result = target.Context.Variable<double>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<double> Subtract(this ValueSymbol<double> target, double value)
    {
        var result = target.Context.Variable<double>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R8, value);
        target.Context.Code.Emit(OpCodes.Sub);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<double> Multiply(this ValueSymbol<double> target, ValueSymbol<double> value)
    {
        var result = target.Context.Variable<double>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<double> Multiply(this ValueSymbol<double> target, double value)
    {
        var result = target.Context.Variable<double>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R8, value);
        target.Context.Code.Emit(OpCodes.Mul);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<double> Divide(this ValueSymbol<double> target, ValueSymbol<double> value)
    {
        var result = target.Context.Variable<double>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<double> Divide(this ValueSymbol<double> target, double value)
    {
        var result = target.Context.Variable<double>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R8, value);
        target.Context.Code.Emit(OpCodes.Div);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<double> Modulus(this ValueSymbol<double> target, ValueSymbol<double> value)
    {
        var result = target.Context.Variable<double>();
        target.EmitLoadAsValue();
        value.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<double> Modulus(this ValueSymbol<double> target, double value)
    {
        var result = target.Context.Variable<double>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Ldc_R8, value);
        target.Context.Code.Emit(OpCodes.Rem);
        result.EmitStoreFromValue();
        return result;
    }

    public static VariableSymbol<double> Negate(this ValueSymbol<double> target)
    {
        var result = target.Context.Variable<double>();
        target.EmitLoadAsValue();
        target.Context.Code.Emit(OpCodes.Neg);
        result.EmitStoreFromValue();
        return result;
    }
}