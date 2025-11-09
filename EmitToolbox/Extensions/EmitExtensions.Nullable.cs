namespace EmitToolbox.Extensions;

public static class EmitNullableExtensions
{
    extension(ILGenerator code)
    {
        public void ToNullable<TContent>() where TContent : struct
            => ToNullable(code, typeof(TContent));

        public void NullableHasValue<TContent>() where TContent : struct
            => NullableHasValue(code, typeof(TContent));

        public void NullableGetValue<TContent>() where TContent : struct
            => NullableGetValue(code, typeof(TContent));

        public void ToNullable(Type content)
            => code.NewObject(typeof(Nullable<>).MakeGenericType(content).GetConstructor([content])!);

        public void NullableHasValue(Type content)
            => code.Call(typeof(Nullable<>).MakeGenericType(content).GetProperty("HasValue")!.GetMethod!);

        public void NullableGetValue(Type content)
            => code.Call(typeof(Nullable<>).MakeGenericType(content).GetProperty("Value")!.GetMethod!);
    }
}