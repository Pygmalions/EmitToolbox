namespace EmitToolbox.Framework;

public abstract class ContentModifier
{
    private ContentModifier()
    {
    }

    /// <summary>
    /// Get a none modifier; this modifier does nothing to the modified type.
    /// </summary>
    public static ContentModifier None => ContentNoneModifier.Instance;

    /// <summary>
    /// Get a reference modifier; this modifier makes the modified type a reference type.
    /// </summary>
    public static ContentModifier Reference => ContentReferenceModifier.Instance;

    /// <summary>
    /// Create a pointer modifier with the specified level.
    /// </summary>
    /// <param name="level">Pointer level, e.g., 1 for T*, 2 for T**, etc.</param>
    /// <returns>Pointer modifier with the specified level.</returns>
    public static ContentModifier Pointer(int level = 1) => new ContentPointerModifier(level);

    /// <summary>
    /// Create a corresponding modifier from the specified type.
    /// </summary>
    /// <param name="type">Type to parse modifier from.</param>
    /// <returns>
    /// This modifier can decorate the basic type of the specified type to itself. 
    /// </returns>
    public static ContentModifier Parse(Type type)
    {
        if (type.IsByRef)
            return Reference;
        return type.IsPointer ? Pointer(type.GetPointerLevel()) : None;
    }
    
    public abstract Type Decorate(Type type);

    public sealed class ContentNoneModifier : ContentModifier
    {
        internal static readonly ContentNoneModifier Instance = new();

        private ContentNoneModifier()
        {
        }

        public override Type Decorate(Type type) => type.BasicType;
    }

    public sealed class ContentReferenceModifier : ContentModifier
    {
        internal static readonly ContentReferenceModifier Instance = new();

        internal ContentReferenceModifier()
        {
        }

        public override Type Decorate(Type type) => type.BasicType.MakeByRefType();
    }

    public sealed class ContentPointerModifier : ContentModifier
    {
        internal ContentPointerModifier(int level)
        {
            Level = level;
        }

        public int Level { get; }

        public override Type Decorate(Type type)
        {
            type = type.BasicType;
            for (var index = 0; index < Level; ++index)
                type = type.MakePointerType();
            return type;
        }
    }
}

public static class ValueModifierExtensions
{
    extension(Type type)
    {
        /// <summary>
        /// If this type is a by-reference type or a pointer type,
        /// return the basic type of the type;
        /// otherwise, return the type itself.
        /// </summary>
        public Type BasicType
        {
            get
            {
                if (type.IsByRef)
                    return type.GetElementType()!;
                while (type.IsPointer)
                    type = type.GetElementType()!;
                return type;
            }
        }

        /// <summary>
        /// Get the pointer level of this type.
        /// </summary>
        /// <returns>
        /// Zero if this type is not a pointer type;
        /// otherwise, the pointer level of this type.
        /// </returns>
        public int GetPointerLevel()
        {
            var level = 0;
            while (type.IsPointer)
            {
                type = type.GetElementType()!;
                ++level;
            }

            return level;
        }
    }

    /// <summary>
    /// Decorate the basic type of this type with the specified modifier.
    /// </summary>
    /// <param name="modifier">
    /// Modifier to apply to the basic type of the specified type;
    /// if it is null, a <see cref="ContentModifier.ContentNoneModifier"/> will be used.
    /// </param>
    /// <typeparam name="TType">Type to be decorated.</typeparam>
    /// <returns>Decorated basic type of the specified type.</returns>
    public static Type Decorate<TType>(this ContentModifier? modifier) where TType : allows ref struct
        => (modifier ?? ContentModifier.None).Decorate(typeof(TType));
}