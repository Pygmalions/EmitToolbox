using System.Runtime.CompilerServices;
using EmitToolbox.Extensions;
using EmitToolbox.Framework.Symbols.Traits;

namespace EmitToolbox.Framework.Symbols.Extensions;

public static class ReferenceSymbolExtensions
{
    /// <summary>
    /// Remove the ByRef modifier from the type, if present.
    /// </summary>
    /// <param name="type">Type to remove the possible by-ref modifier.</param>
    /// <returns>Type without by-ref modifier.</returns>
    public static Type WithoutByRef(this Type type)
        => type.IsByRef
            ? type.GetElementType()!
            : type;
    
    /// <summary>
    /// Check whether this symbol supports loading as a reference,
    /// without copying the value to a temporary variable.
    /// </summary>
    /// <param name="symbol">Symbol to check.</param>
    /// <returns>
    /// True if the symbol is either a by-ref type or an addressable symbol;
    /// false if the symbol needed to be copied to a temporary variable to load as a reference.
    /// </returns>
    public static bool CanLoadAsReference(this ISymbol symbol)
        => symbol.ContentType.IsByRef || symbol is IAddressableSymbol;
    
    /// <summary>
    /// Assign the value of <paramref name="assignor"/> to <paramref name="assignee"/>.
    /// </summary>
    /// <param name="assignee">Symbol to be assigned the value.</param>
    /// <param name="assignor">Symbol to retrieve the value.</param>
    public static void AssignFrom(this IAssignableSymbol assignee, ISymbol assignor)
    {
        InvalidAssignmentException.Examine(assignor, assignee);
        
        if (assignee is ISpecialAssignee specialAssignee)
        {
            specialAssignee.AssignFrom(assignor);
            return;
        }
        if (assignor is ISpecialAssignor specialAssignor)
        {
            specialAssignor.AssignTo(assignee);
            return;
        }

        var elementType = assignee.ContentType.WithoutByRef();
        if (!elementType.IsValueType || elementType.IsPrimitive)
        {
            assignor.EmitLoadAsValue();
            assignee.EmitStoreFromValue();
            return;
        }

        // For non-primitive value types, use copy-object if both symbols can be loaded as reference.
        if (assignee.CanLoadAsReference() && assignor.CanLoadAsReference())
        {
            assignee.EmitLoadAsReference();
            assignor.EmitLoadAsReference();
            
            assignee.Context.Code.Emit(OpCodes.Cpobj, elementType);
        }
        
        assignor.EmitLoadAsValue();
        assignee.EmitStoreFromValue();
    }

    /// <summary>
    /// Assign the value of <paramref name="assignor"/> to <paramref name="assignee"/>.
    /// </summary>
    /// <param name="assignee">Symbol to be assigned the value.</param>
    /// <param name="assignor">Symbol to retrieve the value.</param>
    public static void AssignFrom<TTo, TFrom>(this IAssignableSymbol<TTo> assignee, ISymbol<TFrom> assignor)
        where TFrom : TTo
        => AssignFrom((IAssignableSymbol)assignee, assignor);
    
    /// <summary>
    /// Load this symbol as a value.
    /// If the symbol is a by-ref type, the value will be dereferenced and loaded.
    /// </summary>
    /// <param name="symbol">Symbol to load as a value.</param>
    public static void EmitLoadAsValue(this ISymbol symbol)
    {
        if (!symbol.ContentType.IsByRef)
        {
            symbol.EmitLoadContent();
            return;
        }

        var elementType = symbol.ContentType.GetElementType()!;

        var code = symbol.Context.Code;
        
        // Handle class types.
        if (!elementType.IsValueType)
        {
            symbol.EmitLoadContent();
            code.Emit(OpCodes.Ldind_Ref);
            return;
        }

        // Handle struct types.
        if (!elementType.IsPrimitive)
        {
            symbol.EmitLoadContent();
            code.Emit(OpCodes.Ldobj, elementType);
            return;
        }
        
        // Handle primitive types.
        symbol.EmitLoadContent();
        
        if (elementType == typeof(bool) || elementType == typeof(sbyte))
            code.Emit(OpCodes.Ldind_I1);
        else if (elementType == typeof(byte))
            code.Emit(OpCodes.Ldind_U1);
        else if (elementType == typeof(short) || elementType == typeof(char))
            code.Emit(OpCodes.Ldind_I2);
        else if (elementType == typeof(ushort))
            code.Emit(OpCodes.Ldind_U2);
        else if (elementType == typeof(int))
            code.Emit(OpCodes.Ldind_I4);
        else if (elementType == typeof(uint))
            code.Emit(OpCodes.Ldind_U4);
        else if (elementType == typeof(long) || elementType == typeof(ulong))
            code.Emit(OpCodes.Ldind_I8);
        else if (elementType == typeof(float))
            code.Emit(OpCodes.Ldind_R4);
        else if (elementType == typeof(double))
            code.Emit(OpCodes.Ldind_R8);
        else if (elementType == typeof(nint) || elementType == typeof(nuint))
            code.Emit(OpCodes.Ldind_I);
        else
            throw new Exception($"Unrecognized primitive type: '{elementType}'.");
    }
    
    /// <summary>
    /// Load this symbol as a reference.
    /// If the symbol is not addressable, its value will be copied to a temporary variable
    /// and the address of that variable will be loaded.
    /// </summary>
    /// <param name="symbol">Symbol to load as a reference.</param>
    public static void EmitLoadAsReference(this ISymbol symbol)
    {
        if (symbol.ContentType.IsByRef)
        {
            symbol.EmitLoadContent();
            return;
        }
        
        if (symbol is IAddressableSymbol addressableSymbol)
        {
            addressableSymbol.EmitLoadAddress();
            return;
        }

        // For non-addressable value types, store the value in a temporary variable and load the address of that.
        var temporaryVariable = symbol.Context.Code.DeclareLocal(symbol.ContentType);
        symbol.EmitLoadContent();
        symbol.Context.Code.Emit(OpCodes.Stloc, temporaryVariable);
        symbol.Context.Code.Emit(OpCodes.Ldloca, temporaryVariable);
    }
    
    /// <summary>
    /// Load this symbol as a parameter for a method call,
    /// the symbol will be loaded as a value or as a reference
    /// depending on the parameter's in/out/ref modifiers.
    /// </summary>
    /// <param name="symbol">Symbol to load.</param>
    /// <param name="parameter">Parameter information.</param>
    public static void EmitLoadAsParameter(this ISymbol symbol, ParameterInfo parameter)
    {
        if (parameter is {IsIn: false, IsOut: false, ParameterType.IsByRef: false })
            symbol.EmitLoadAsValue();
        else
            symbol.EmitLoadAsReference();
    }
    
    /// <summary>
    /// Load the symbol for invoking instance methods on it.
    /// If this symbol is of a value type, it will be loaded as a reference,
    /// otherwise it will be loaded as a value.
    /// </summary>
    /// <param name="symbol">Symbols to load for method invocations.</param>
    public static void EmitLoadAsTarget(this ISymbol symbol)
    {
        if (!symbol.ContentType.IsValueType)
            symbol.EmitLoadAsValue();
        else
            symbol.EmitLoadAsReference();
    }
    
    /// <summary>
    /// Store the value on the stack into this symbol.
    /// If the symbol is a by-ref type, the value will be stored at the referenced address.
    /// </summary>
    /// <param name="symbol">Symbol to store the value into.</param>
    public static void EmitStoreFromValue(this IAssignableSymbol symbol)
    {
        if (!symbol.ContentType.IsByRef)
        {
            symbol.EmitStoreContent();
            return;
        }
        
        var code = symbol.Context.Code;
        
        var elementType = symbol.ContentType.GetElementType()!;
        
        var temporary = code.DeclareLocal(elementType);
        code.StoreLocal(temporary);
        
        // Handle class types.
        if (!elementType.IsValueType)
        {
            symbol.EmitLoadContent();
            code.Emit(OpCodes.Ldloc, temporary);
            code.Emit(OpCodes.Stind_Ref);
            return;
        }

        // Handle struct types.
        if (!elementType.IsPrimitive)
        {
            symbol.EmitLoadContent();
            code.Emit(OpCodes.Ldloc, temporary);
            code.Emit(OpCodes.Stobj, elementType);
            return;
        }
        
        // Handle primitive types.
        symbol.EmitLoadContent();
        code.Emit(OpCodes.Ldloc, temporary);
        
        if (elementType == typeof(sbyte) || elementType == typeof(byte) ||
            elementType == typeof(bool))
            code.Emit(OpCodes.Stind_I1);
        else if (elementType == typeof(short) || elementType == typeof(ushort) ||
                 elementType == typeof(char))
            code.Emit(OpCodes.Stind_I2);
        else if (elementType == typeof(int) || elementType == typeof(uint))
            code.Emit(OpCodes.Stind_I4);
        else if (elementType == typeof(long) || elementType == typeof(ulong))
            code.Emit(OpCodes.Stind_I8);
        else if (elementType == typeof(float))
            code.Emit(OpCodes.Stind_R4);
        else if (elementType == typeof(double))
            code.Emit(OpCodes.Stind_R8);
        else if (elementType == typeof(nint) || elementType == typeof(nuint))
            code.Emit(OpCodes.Stind_I);
        else
            throw new Exception($"Unrecognized primitive type: '{elementType}'.");
    }
}