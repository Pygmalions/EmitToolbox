namespace EmitToolbox.Extensions;

public static class EmitStringExtensions
{
    public static void CompareString(this ILGenerator code)
    {
        code.Call(typeof(string).GetMethod("Equals",
            BindingFlags.Static | BindingFlags.Public,
            [typeof(string), typeof(string)])!);
    }
}