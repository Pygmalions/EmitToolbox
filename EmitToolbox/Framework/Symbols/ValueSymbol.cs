using System.Diagnostics.CodeAnalysis;

namespace EmitToolbox.Framework.Symbols;

public abstract class ValueSymbol(MethodBuildingContext context, bool isReference = false)
{
    /// <summary>
    /// Value type of this symbol.
    /// </summary>
    public abstract Type ValueType { get; }

    /// <summary>
    /// Indicate whether this symbol holds a reference to the value.
    /// </summary>
    public bool IsReference { get; } = isReference;

    /// <summary>
    /// Context of this value.
    /// </summary>
    public MethodBuildingContext Context { get; } = context;

    /// <summary>
    /// Directly load this value symbol into the stack, invoked when this value symbol is not a reference.
    /// </summary>
    public abstract void EmitDirectlyLoadValue();

    /// <summary>
    /// Load the address of this value symbol, or wrap it into a variable and load the address of that variable.
    /// </summary>
    public abstract void EmitDirectlyLoadAddress();
    
    /// <summary>
    /// Load this value as a value.
    /// </summary>
    public abstract void EmitLoadAsValue();

    /// <summary>
    /// Load this value as an address.
    /// </summary>
    public abstract void EmitLoadAsAddress();

    /// <summary>
    /// Load this value as a target for method calls.
    /// </summary>
    public void EmitLoadAsTarget()
    {
        if (!ValueType.IsValueType)
            EmitLoadAsValue();
        else
            EmitLoadAsAddress();
    }

    /// <summary>
    /// Load this value according to the parameter information.
    /// </summary>
    /// <param name="parameter">Parameter information.</param>
    public void EmitLoadAsParameter(ParameterInfo parameter)
    {
        if (parameter.IsIn || parameter.IsOut || parameter.ParameterType.IsByRef)
            EmitLoadAsAddress();
        else
            EmitLoadAsValue();
    }
    
    public VariableSymbol<bool> IsNull()
    {
        var result = Context.Variable<bool>();
        
        if (ValueType.IsValueType)
        {
            // Nullable Value Type: check if it has value.
            if (ValueType.IsGenericType && ValueType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                EmitLoadAsAddress();
                Context.Code.Emit(OpCodes.Call,
                    ValueType.GetProperty(nameof(Nullable<>.HasValue))!.GetGetMethod()!);
                // Negate the result.
                Context.Code.Emit(OpCodes.Ldc_I4_0);
                Context.Code.Emit(OpCodes.Ceq);
                result.EmitStoreFromValue();
                return result;
            }
            
            // Non-nullable Value Type: always false.
            Context.Code.Emit(OpCodes.Ldc_I4_0);
            result.EmitStoreFromValue();
            return result;
        }
        
        // Reference Type: compare the value and null.
        EmitLoadAsValue();
        Context.Code.Emit(OpCodes.Ldnull);
        Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreFromValue();

        return result;
    }

    public VariableSymbol<bool> IsInstanceOf<TValue>()
    {
        var result = Context.Variable<bool>();
        
        if (ValueType.IsValueType)
        {
            Context.Code.Emit(ValueType == typeof(TValue) ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
            result.EmitStoreFromValue();
            return result;
        }
        
        EmitLoadAsValue();
        Context.Code.Emit(OpCodes.Isinst, typeof(TValue));
        Context.Code.Emit(OpCodes.Ldnull);
        Context.Code.Emit(OpCodes.Cgt_Un);
        result.EmitStoreFromValue();
        
        return result;
    }
}

public abstract class ValueSymbol<TValue>(MethodBuildingContext context, bool isReference = false) 
    : ValueSymbol(context, isReference)
{
    public sealed override Type ValueType { get; } = typeof(TValue);

    /// <summary>
    /// Temporary variable for this value symbol to use.
    /// This variable will be declared the first time this property is accessed.
    /// This symbol always holds a value rather than a reference.
    /// </summary>
    [field: MaybeNull]
    protected VariableSymbol<TValue> TemporaryVariable 
        => field ??= Context.Variable<TValue>();
}