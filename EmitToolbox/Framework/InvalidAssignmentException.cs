using System.Diagnostics;

namespace EmitToolbox.Framework;

public class InvalidAssignmentException(Type fromType, Type toType) : Exception(
    BuildMessage(fromType, toType))
{
    private static string BuildMessage(Type assignorType, Type assigneeType)
        => $"Cannot assign symbol of type '{assignorType}' to symbol of type '{assigneeType}'.";
    
    [StackTraceHidden, DebuggerStepThrough]
    public static void Examine(Type fromType, Type toType)
    {
        if (!fromType.IsAssignableTo(toType))
            throw new InvalidAssignmentException(fromType, toType);
    }
    
    [StackTraceHidden, DebuggerStepThrough]
    public static void Examine(ISymbol fromSymbol, IAssignableSymbol toSymbol)
    {
        var fromType = fromSymbol.ValueType;
        if (fromType.IsByRef)
            fromType = fromType.GetElementType()!;
        var toType = toSymbol.ValueType;
        if (toType.IsByRef)
            toType = toType.GetElementType()!;
        
        if (!fromType.IsAssignableTo(toType))
            throw new InvalidAssignmentException(fromType, toType);
    }
}