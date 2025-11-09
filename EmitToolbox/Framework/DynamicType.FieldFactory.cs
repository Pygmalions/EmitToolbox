namespace EmitToolbox.Framework;

public class FieldFactory(DynamicType context)
{
    public DynamicField DefineStatic(Type type, string name,
        VisibilityLevel visibility = VisibilityLevel.Public)
    {
        var field = context.Builder.DefineField(name, type, 
            FieldAttributes.Static | visibility.ToFieldAttributes());
        return new DynamicField(context, field);
    }
    
    public DynamicField DefineInstance(Type type, string name,
        VisibilityLevel visibility = VisibilityLevel.Public)
    {
        var field = context.Builder.DefineField(name, type, visibility.ToFieldAttributes());
        return new DynamicField(context, field);
    }
}