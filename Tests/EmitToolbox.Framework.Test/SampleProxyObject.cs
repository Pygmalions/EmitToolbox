namespace EmitToolbox.Framework.Test;

public class SampleProxyObject
{
    public int NumberField = 0;

    public virtual int NumberProperty
    {
        get => NumberField;
        set => NumberField = value;
    }
    
    public virtual int Add(int a, int b)
    {
        return a + b;
    }
}