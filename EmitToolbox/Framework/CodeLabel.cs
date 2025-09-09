namespace EmitToolbox.Framework;

public class CodeLabel(DynamicMethod context)
{
    private readonly Label _label = context.Code.DefineLabel();
    
    public bool IsMarked { get; private set; }
    
    public void Mark()
    {
        if (IsMarked)
            throw new InvalidOperationException("This label is already marked.");
        IsMarked = true;
        context.Code.MarkLabel(_label);
    }

    public void Jump()
    {
        if (!IsMarked)
            throw new InvalidOperationException("This label has not already marked.");
        context.Code.Emit(OpCodes.Br, _label);
    }
}