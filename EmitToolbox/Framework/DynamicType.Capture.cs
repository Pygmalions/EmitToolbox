using EmitToolbox.Extensions;

namespace EmitToolbox.Framework;

public partial class DynamicType
{
    private FieldInfo _fieldCapturedObjects;
    
    private readonly List<object> _listCapturedObjects = [];
    
    /// <summary>
    /// Capture a compile-time object to be used at runtime.
    /// </summary>
    /// <param name="value">Object value to capture.</param>
    /// <returns>Symbol of the captured object.</returns>
    public CapturedObject CaptureObject(object value)
    {
        var index = _listCapturedObjects.Count;
        _listCapturedObjects.Add(value);
        return new CapturedObject(_fieldCapturedObjects, value.GetType(), index);
    }
    
    public readonly struct CapturedObject(FieldInfo listField, Type objectType, int index)
    {
        private static readonly MethodInfo MethodGetItem = 
            typeof(List<object>).GetMethod("get_Item", [typeof(int)])!;
        
        public class CapturedObjectSymbol(FieldInfo listField) : ISymbol<object>
        {
            public required int Index { get; init; }
            
            public required DynamicMethod Context { get; init;}
            
            public required Type ContentType { get; init;}
            
            public void EmitLoadContent(DynamicMethod method)
            {
                method.Code.Emit(OpCodes.Ldsfld, listField);
                method.Code.LoadLiteral(Index);
                method.Code.Call(MethodGetItem);
            }
        }
        
        public CapturedObjectSymbol SymbolOf(DynamicMethod context, Type? type = null)
            => new(listField)
            {
                Index = index,
                Context = context,
                ContentType = type ?? objectType
            };
    }
}