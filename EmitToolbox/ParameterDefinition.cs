namespace EmitToolbox;

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
        => new (typeof(TParameter).MakeByRefType(), ParameterModifier.None, name, attributes);
    
    public static ParameterDefinition Pointer<TParameter>(
        string? name = null,
        Type[]? attributes = null)
        => new (typeof(TParameter).MakePointerType(), ParameterModifier.None, name, attributes);
    
    public static ParameterDefinition In<TParameter>(
        string? name = null,
        Type[]? attributes = null)
        => new (typeof(TParameter), ParameterModifier.In, name, attributes);
    
    public static ParameterDefinition Out<TParameter>(
        string? name = null,
        Type[]? attributes = null)
        => new (typeof(TParameter), ParameterModifier.Out, name, attributes);
    
    public static implicit operator ParameterDefinition(Type type)
        => new (type);

    public static implicit operator Type(ParameterDefinition parameter)
        => parameter.Type;
    
    public static implicit operator ParameterDefinition(ParameterInfo parameter)
        => new (parameter.ParameterType);
}

public static class ParameterDefinitionExtensions
{
    extension(IEnumerable<ParameterDefinition> self)
    {
        public IEnumerable<Type[]> ToRequiredCustomModifiers()
            => self.Select(parameter => parameter.Modifier.ToCustomAttributes());

        public IEnumerable<Type[]> ToOptionalCustomModifiers()
            => self.Select(parameter => parameter.Attributes ?? Type.EmptyTypes);
        
        /// <summary>
        /// Select the parameter types of these parameter definitions.
        /// </summary>
        public IEnumerable<Type> SelectTypes()
            => self.Select(parameter => parameter.Type);
    }
    
    extension(IEnumerable<ParameterInfo> self)
    {
        public IEnumerable<ParameterDefinition> ToDefinitions()
        {
            return self.Select(parameter =>
            {
                if (parameter.IsIn)
                    return new ParameterDefinition(parameter.ParameterType, ParameterModifier.In);
                if (parameter.IsOut)
                    return new ParameterDefinition(parameter.ParameterType, ParameterModifier.Out);
                return new ParameterDefinition(parameter.ParameterType);
            });
        }
    }
}