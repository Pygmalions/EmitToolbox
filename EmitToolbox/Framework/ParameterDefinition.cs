namespace EmitToolbox.Framework;

public record struct ParameterDefinition(
    Type Type,
    ParameterModifier Modifier = ParameterModifier.None,
    string? Name = null,
    Type[]? Attributes = null)
{
    public static ParameterDefinition Value<TParameter>(
        string? name = null,
        Type[]? attributes = null)
        => new (typeof(TParameter), ParameterModifier.None, name, attributes);
    
    public static ParameterDefinition Reference<TParameter>(
        string? name = null,
        Type[]? attributes = null)
        => new (typeof(TParameter), ParameterModifier.Ref, name, attributes);
    
    public static ParameterDefinition In<TParameter>(
        string? name = null,
        Type[]? attributes = null)
        => new (typeof(TParameter), ParameterModifier.In, name, attributes);
    
    public static ParameterDefinition Out<TParameter>(
        string? name = null,
        Type[]? attributes = null)
        => new (typeof(TParameter), ParameterModifier.Out, name, attributes);
}