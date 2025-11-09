namespace EmitToolbox.Framework.Utilities;

public static class NumberMetadata<TNumber>
{
    public static readonly Lazy<bool> IsUnsigned = new(() =>
        {
            var type = typeof(TNumber);
            if (type == typeof(byte))
                return true;
            if (type == typeof(ushort))
                return true;
            if (type == typeof(uint))
                return true;
            if (type == typeof(ulong))
                return true;
            return false;
        }
    );
}