using System.Diagnostics;

namespace EmitToolbox.Extensions;

public static class EmitDebuggerExtensions
{
    public static void Debug(this ILGenerator code,
        Dictionary<string, LocalBuilder> variables)
        => code.Debug(null, variables);
    
    public static void Debug(this ILGenerator code, string? message,
        Dictionary<string, LocalBuilder> variables)
    {
        code.LoadLiteral(message ?? string.Empty);
        code.NewObject(typeof(Dictionary<string, object>).GetConstructor(Type.EmptyTypes)!);
        foreach (var (name, variable) in variables)
        {
            code.Duplicate();
            code.LoadLiteral(name);
            code.Emit(OpCodes.Ldloc, variable);
            if (variable.LocalType.IsValueType)
                code.Box(variable.LocalType);
            code.Call(typeof(Dictionary<string, object>).GetMethod("Add",
                [typeof(string), typeof(object)])!);
        }
        code.Call(typeof(EmitDebuggerExtensions).GetMethod(nameof(OnDebugInvoked))!);
    }

    [Obsolete("This method is only for emitting call instructions.")]
    public static void OnDebugInvoked(string message, Dictionary<string, object> variables)
    {
        Debugger.Break();
    }
}