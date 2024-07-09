using System.Reflection;
using System.Reflection.Emit;

namespace EmitToolbox.Framework.Utilities;

public static class EmitExtension
{
    public static void EmitFieldInfo(this ILGenerator code, FieldInfo field)
    {
        code.Emit(OpCodes.Ldtoken, field);
        code.Emit(OpCodes.Call, 
            typeof(FieldInfo).GetMethod(nameof(FieldInfo.GetFieldFromHandle), [typeof(RuntimeFieldHandle)])!);
    }

    public static void EmitPropertyInfo(this ILGenerator code, PropertyInfo property)
    {
        code.Emit(OpCodes.Ldtoken, property.DeclaringType!);
        code.Emit(OpCodes.Call, typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle))!);
        code.Emit(OpCodes.Ldstr, property.Name);
        code.Emit(OpCodes.Ldc_I4_S, 
            (int)(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
        code.Emit(OpCodes.Call, 
            typeof(Type).GetMethod(nameof(Type.GetProperty), [typeof(string), typeof(BindingFlags)])!);
    }

    public static void EmitMethodInfo(this ILGenerator code, MethodInfo method)
    {
        code.Emit(OpCodes.Ldtoken, method);
        code.Emit(OpCodes.Call, 
            typeof(MethodInfo).GetMethod(nameof(MethodInfo.GetMethodFromHandle))!);
    }

    public static void EmitIf(this ILGenerator code, 
        Action<ILGenerator> predicate,
        Action<ILGenerator>? whenTrue = null, Action<ILGenerator>? whenFalse = null)
    {
        predicate(code);
        
        var labelFalse = code.DefineLabel();
        var labelEnd = code.DefineLabel();
        
        code.Emit(OpCodes.Brfalse, labelFalse);
        whenTrue?.Invoke(code);
        code.Emit(OpCodes.Br, labelEnd);
        code.MarkLabel(labelFalse);
        whenFalse?.Invoke(code);
        code.MarkLabel(labelEnd);
    }
}