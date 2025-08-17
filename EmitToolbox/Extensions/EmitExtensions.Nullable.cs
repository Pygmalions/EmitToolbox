namespace EmitToolbox.Extensions;

public static class EmitNullableExtensions
{
    public static void ToNullable<TContent>(this ILGenerator code) where TContent : struct
        => ToNullable(code, typeof(TContent));

    public static void NullableHasValue<TContent>(this ILGenerator code) where TContent : struct
        => NullableHasValue(code, typeof(TContent));

    public static void NullableGetValue<TContent>(this ILGenerator code) where TContent : struct
        => NullableGetValue(code, typeof(TContent));
    
    public static void ToNullable(this ILGenerator code, Type content)
        => code.NewObject(typeof(Nullable<>).MakeGenericType(content).GetConstructor([content])!);
    
    public static void NullableHasValue(this ILGenerator code, Type content)
        => code.Call(typeof(Nullable<>).MakeGenericType(content).GetProperty("HasValue")!.GetMethod!);
    
    public static void NullableGetValue(this ILGenerator code, Type content)
        => code.Call(typeof(Nullable<>).MakeGenericType(content).GetProperty("Value")!.GetMethod!);
}