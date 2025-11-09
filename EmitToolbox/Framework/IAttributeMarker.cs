namespace EmitToolbox.Framework;

public interface IAttributeMarker<out TSelf> where TSelf : IAttributeMarker<TSelf>
{
    /// <summary>
    /// Mark an attribute on the building object.
    /// </summary>
    /// <param name="attributeBuilder">Builder of the custom attribute to mark.</param>
    /// <returns>This builder.</returns>
    TSelf MarkAttribute(CustomAttributeBuilder attributeBuilder);
}

public static class AttributeMarkerExtensions
{
    extension<TMarker>(TMarker self) where TMarker : IAttributeMarker<TMarker>
    {
        /// <summary>
        /// Mark an attribute on the building object.
        /// </summary>
        /// <param name="attributeType">Type of the attribute to mark.</param>
        /// <returns>This builder.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the specified attribute type does not have a parameterless constructor.
        /// </exception>
        public TMarker MarkAttribute(Type attributeType)
        {
            var constructor = attributeType.GetConstructor(Type.EmptyTypes)
                ?? throw new ArgumentException(
                    "Specified attribute type does not have a parameterless constructor.", nameof(attributeType));
            self.MarkAttribute(new CustomAttributeBuilder(constructor, []));
            return self;
        }

        /// <summary>
        /// Mark an attribute on the building object.
        /// </summary>
        /// <typeparam name="TAttribute">Type of the attribute to mark.</typeparam>
        /// <returns>This builder.</returns>
        public TMarker MarkAttribute<TAttribute>() where TAttribute : Attribute
            => self.MarkAttribute(typeof(TAttribute));
    }
}