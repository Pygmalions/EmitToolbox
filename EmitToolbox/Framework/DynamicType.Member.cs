namespace EmitToolbox.Framework;

public partial class DynamicType
{
    public class DynamicFieldBuilder
    {
        private readonly DynamicType _context;
        
        internal DynamicFieldBuilder(DynamicType context)
        {
            _context = context;
        }
        
        public InstanceDynamicField DefineInstance(string name, Type type, VisibilityLevel visibility = VisibilityLevel.Public)
        {
            var attributes = visibility.ToFieldAttributes();
            var fieldBuilder = _context.TypeBuilder.DefineField(name, type, attributes);
        
            return new InstanceDynamicField(_context, fieldBuilder);
        }
        
        public StaticDynamicField DefineStatic(string name, Type type, VisibilityLevel visibility = VisibilityLevel.Public)
        {
            var attributes = FieldAttributes.Static | visibility.ToFieldAttributes();
            var fieldBuilder = _context.TypeBuilder.DefineField(name, type, attributes);
        
            return new StaticDynamicField(_context, fieldBuilder);
        }
    }

    public class DynamicPropertyBuilder
    {
        private readonly DynamicType _context;
        
        internal DynamicPropertyBuilder(DynamicType context)
        {
            _context = context;
        }
        
        public InstanceDynamicProperty DefineInstance(
            string name, Type type, VisibilityLevel visibility = VisibilityLevel.Public,
            MethodModifier modifier = MethodModifier.None)
        {
            var propertyBuilder = _context.TypeBuilder.DefineProperty(
                name, PropertyAttributes.None, type, Type.EmptyTypes);
            return new InstanceDynamicProperty(_context, propertyBuilder, visibility, modifier);
        }
        
        public StaticDynamicProperty DefineStatic(
            string name, Type type, VisibilityLevel visibility = VisibilityLevel.Public)
        {
            var propertyBuilder = _context.TypeBuilder.DefineProperty(
                name, PropertyAttributes.None, type, Type.EmptyTypes);
            return new StaticDynamicProperty(_context, propertyBuilder, visibility);
        }
    }
    
    public DynamicFieldBuilder FieldBuilder { get; }
    
    public DynamicPropertyBuilder PropertyBuilder { get; }
}