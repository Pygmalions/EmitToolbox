namespace EmitToolbox.Framework.Elements;

public abstract class ValueElement(MethodContext context)
{
    public MethodContext Context { get; } = context;
    
    public abstract Type ValueType { get; }

    public virtual bool IsReference { get; } = false;

    /// <summary>
    /// Load the value of this element.
    /// </summary>
    protected internal abstract void EmitLoadAsValue();

    /// <summary>
    /// Load the address of this element, or emplace it into a variable and load the address of that variable.
    /// </summary>
    protected internal abstract void EmitLoadAsAddress();

    /// <summary>
    /// Load this element as a target to call its functions.
    /// </summary>
    protected internal virtual void EmitLoadAsTarget()
    {
        if (ValueType.IsValueType)
            EmitLoadAsAddress();
        else
            EmitLoadAsValue();
    }
    
    public VariableElement<bool> IsNull()
    {
        var result = Context.DefineVariable<bool>();
        
        if (ValueType.IsValueType)
        {
            // Nullable Value Type: check if it has value.
            if (ValueType.IsGenericType && ValueType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                EmitLoadAsAddress();
                Context.Code.Emit(OpCodes.Call,
                    ValueType.GetProperty("HasValue")!.GetGetMethod()!);
                // Negate the result.
                Context.Code.Emit(OpCodes.Ldc_I4_0);
                Context.Code.Emit(OpCodes.Ceq);
                result.EmitStoreValue();
                return result;
            }
            
            // Non-nullable Value Type: always false.
            Context.Code.Emit(OpCodes.Ldc_I4_0);
            result.EmitStoreValue();
            return result;
        }
        
        // Reference Type: compare the value and null.
        EmitLoadAsValue();
        Context.Code.Emit(OpCodes.Ldnull);
        Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreValue();

        return result;
    }
    
    public VariableElement<bool> IsEqualTo<TValue>(ValueElement<TValue> other)
    {
        var result = Context.DefineVariable<bool>();
        
        EmitLoadAsValue();
        other.EmitLoadAsValue();
        Context.Code.Emit(OpCodes.Ceq);
        result.EmitStoreValue();
        
        return result;
    }

    public VariableElement<bool> IsInstanceOf<TValue>()
    {
        var result = Context.DefineVariable<bool>();
        
        if (ValueType.IsValueType)
        {
            Context.Code.Emit(ValueType == typeof(TValue) ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
            result.EmitStoreValue();
            return result;
        }
        
        EmitLoadAsValue();
        Context.Code.Emit(OpCodes.Isinst, typeof(TValue));
        Context.Code.Emit(OpCodes.Ldnull);
        Context.Code.Emit(OpCodes.Cgt_Un);
        result.EmitStoreValue();
        
        return result;
    }
}

public abstract class ValueElement<TValue>(MethodContext context) : ValueElement(context)
{
    public override Type ValueType { get; } = typeof(TValue);
}