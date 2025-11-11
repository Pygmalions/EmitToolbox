namespace EmitToolbox.Framework;

public class PropertyFactory(DynamicType context)
{
    public DynamicProperty Define(string name, Type type)
    {
        var property = context.Builder.DefineProperty(name, PropertyAttributes.None, type, null);
        return new DynamicProperty(context, property);
    }

    public InstanceDynamicProperty<TProperty> DefineInstance<TProperty>(
        string name, ContentModifier? modifier = null)
    {
        var property = context.Builder.DefineProperty(
            name, PropertyAttributes.None, modifier.Decorate<TProperty>(), null);
        return new InstanceDynamicProperty<TProperty>(context, property);
    }
    
    public StaticDynamicProperty<TProperty> DefineStatic<TProperty>(
        string name, ContentModifier? modifier = null)
    {
        var property = context.Builder.DefineProperty(
            name, PropertyAttributes.None, modifier.Decorate<TProperty>(), null);
        return new StaticDynamicProperty<TProperty>(context, property);
    }
}