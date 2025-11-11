namespace EmitToolbox.Framework;

public class FieldFactory(DynamicType context)
{
    public DynamicField DefineStatic(string name, Type type,
        VisibilityLevel visibility = VisibilityLevel.Public)
    {
        var field = context.Builder.DefineField(name, type,
            FieldAttributes.Static | visibility.ToFieldAttributes());
        return new DynamicField(context, field);
    }

    public DynamicField DefineInstance(string name, Type type,
        VisibilityLevel visibility = VisibilityLevel.Public)
    {
        var field = context.Builder.DefineField(name, type, visibility.ToFieldAttributes());
        return new DynamicField(context, field);
    }
}