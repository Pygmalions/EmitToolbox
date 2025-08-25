namespace EmitToolbox.Framework;

public partial class TypeBuildingContext
{
    public class FieldBuilder
    {
        private readonly TypeBuildingContext _context;
        
        internal FieldBuilder(TypeBuildingContext context)
        {
            _context = context;
        }
        
        public InstanceFieldBuildingContext<TField> Instance<TField>(
            string name, VisibilityLevel visibility = VisibilityLevel.Public)
        {
            var attributes =  visibility switch
            {
                VisibilityLevel.Public => FieldAttributes.Public,
                VisibilityLevel.Private => FieldAttributes.Private,
                VisibilityLevel.Protected => FieldAttributes.Family,
                VisibilityLevel.Internal => FieldAttributes.Assembly,
                VisibilityLevel.ProtectedInternal => FieldAttributes.FamORAssem,
                _ => throw new ArgumentOutOfRangeException(nameof(visibility), visibility, null)
            };
            var fieldBuilder = _context.TypeBuilder.DefineField(name, typeof(TField), attributes);
        
            return new InstanceFieldBuildingContext<TField>(_context, fieldBuilder);
        }
        
        public StaticFieldBuildingContext<TField> Static<TField>(
            string name, VisibilityLevel visibility = VisibilityLevel.Public)
        {
            var attributes = FieldAttributes.Static | visibility switch
            {
                VisibilityLevel.Public => FieldAttributes.Public,
                VisibilityLevel.Private => FieldAttributes.Private,
                VisibilityLevel.Protected => FieldAttributes.Family,
                VisibilityLevel.Internal => FieldAttributes.Assembly,
                VisibilityLevel.ProtectedInternal => FieldAttributes.FamORAssem,
                _ => throw new ArgumentOutOfRangeException(nameof(visibility), visibility, null)
            };
            var fieldBuilder = _context.TypeBuilder.DefineField(name, typeof(TField), attributes);
        
            return new StaticFieldBuildingContext<TField>(_context, fieldBuilder);
        }
    }

    public class PropertyBuilder
    {
        private readonly TypeBuildingContext _context;
        
        internal PropertyBuilder(TypeBuildingContext context)
        {
            _context = context;
        }
        
        public InstancePropertyBuildingContext<TProperty> Instance<TProperty>(
            string name, VisibilityLevel visibility = VisibilityLevel.Public,
            bool isReference = false, MethodModifier modifier = MethodModifier.None)
        {
            var propertyBuilder = _context.TypeBuilder.DefineProperty(name,
                PropertyAttributes.None, 
                isReference ? typeof(TProperty).MakeByRefType() : typeof(TProperty),
                Type.EmptyTypes);
            return new InstancePropertyBuildingContext<TProperty>(
                _context, propertyBuilder, isReference, visibility, modifier);
        }
        
        public StaticPropertyBuildingContext<TProperty> Static<TProperty>(
            string name, VisibilityLevel visibility = VisibilityLevel.Public,
            bool isReference = false)
        {
            var propertyBuilder = _context.TypeBuilder.DefineProperty(name,
                PropertyAttributes.None, 
                isReference ? typeof(TProperty).MakeByRefType() : typeof(TProperty),
                Type.EmptyTypes);
            return new StaticPropertyBuildingContext<TProperty>(
                _context, propertyBuilder, isReference, visibility);
        }
    }
    
    public FieldBuilder Fields { get; }
    
    public PropertyBuilder Properties { get; }
}