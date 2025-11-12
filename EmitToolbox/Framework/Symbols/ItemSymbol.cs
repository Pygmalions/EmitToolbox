using EmitToolbox.Framework.Extensions;

namespace EmitToolbox.Framework.Symbols;

public class ItemSymbol<TElement>(ISymbol target, ISymbol<int> index) : 
    IAssignableSymbol<TElement>
{
    public Type ContentType { get; } = typeof(TElement);
        
    public DynamicFunction Context { get; } = target.Context;
        
    public void LoadContent()
    {
        target
            .Invoke<TElement>(target.BasicType.GetMethod("get_Item")!, [index])
            .LoadContent();
    }

    public void StoreContent()
    {
        var value = Context.Variable<TElement>();
        value.StoreContent();

        target.Invoke(target.BasicType.GetMethod("set_Item")!, [index, value]);
    }

    public void AssignContent(ISymbol<TElement> value)
    {
        target.Invoke(target.BasicType.GetMethod("set_Item")!, [index, value]);
    }
}