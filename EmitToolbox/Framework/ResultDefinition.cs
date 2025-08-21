namespace EmitToolbox.Framework;

public record struct ResultDefinition(
    Type Type, IEnumerable<Type>? Attributes = null)
{
    public static ResultDefinition None { get; } = new(typeof(void));

    public static ResultDefinition Value<TResult>() => new(typeof(TResult));
    
    public static ResultDefinition Reference<TResult>() => new(typeof(TResult).MakeByRefType());
    
    public static implicit operator ResultDefinition(Type type) => new(type);
}