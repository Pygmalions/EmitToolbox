using System.Linq.Expressions;
using EmitToolbox.Extensions;

namespace EmitToolbox.Symbols;

/// <summary>
/// Represents a field symbol in the dynamic method context, providing access to field operations.
/// </summary>
/// <param name="context">The dynamic method context.</param>
/// <param name="field">The field information.</param>
/// <param name="instance">The instance symbol for non-static fields.</param>
public class FieldSymbol(DynamicFunction context, FieldInfo field, ISymbol? instance = null)
    : IAddressableSymbol, IAssignableSymbol
{
    public Type ContentType { get; } = field.FieldType;

    public DynamicFunction Context { get; } = context;

    public ISymbol? Instance { get; } = instance;

    public void LoadContent()
    {
        if (Instance is null)
        {
            Context.Code.Emit(OpCodes.Ldsfld, field);
            return;
        }

        Instance.LoadAsTarget();
        Context.Code.Emit(OpCodes.Ldfld, field);
    }

    public void LoadAddress()
    {
        if (Instance is null)
        {
            Context.Code.Emit(OpCodes.Ldsflda, field);
            return;
        }

        Instance.LoadAsTarget();
        Context.Code.Emit(OpCodes.Ldflda, field);
    }

    public void StoreContent()
    {
        var value = Context.Variable(ContentType);
        AssignContent(value);
    }

    public void AssignContent(ISymbol other)
    {
        if (Instance is null)
        {
            other.LoadForSymbol(this);
            Context.Code.Emit(OpCodes.Stsfld, field);
            return;
        }

        Instance.LoadAsTarget();
        other.LoadForSymbol(this);
        Context.Code.Emit(OpCodes.Stfld, field);
    }

    public FieldSymbol<TContent> AsSymbol<TContent>()
    {
        return !this.BasicType.IsAssignableTo(typeof(TContent))
            ? throw new InvalidCastException($"Type '{this.BasicType}' is not assignable to '{typeof(TContent)}'.")
            : new FieldSymbol<TContent>(this);
    }
}

/// <summary>
/// Represents a strongly typed field symbol in the dynamic method context.
/// </summary>
/// <typeparam name="TContent">The type of the field content.</typeparam>
public class FieldSymbol<TContent> :
    IAssignableSymbol<TContent>, IAddressableSymbol<TContent>
{
    private readonly FieldSymbol _symbol;

    internal FieldSymbol(FieldSymbol symbol)
    {
        _symbol = symbol;
    }

    public FieldSymbol(DynamicFunction context, FieldInfo field, ISymbol? instance = null)
        : this(new FieldSymbol(context, field, instance))
    {
    }

    public DynamicFunction Context => _symbol.Context;

    public Type ContentType => _symbol.ContentType;

    public void LoadContent() => _symbol.LoadContent();

    public void LoadAddress() => _symbol.LoadAddress();

    public void StoreContent() => _symbol.StoreContent();

    public void AssignContent(ISymbol<TContent> other) => _symbol.AssignContent(other);
}

public static class FieldSymbolExtensions
{
    extension(ISymbol self)
    {
        /// <summary>
        /// Create a field symbol and using this symbol as its target instance.
        /// </summary>
        /// <param name="field">Metadata of the field.</param>
        /// <returns>Field symbol bound to this symbol.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the specified field is static.
        /// </exception>
        public FieldSymbol Field(FieldInfo field)
        {
            return field.IsStatic
                ? throw new InvalidOperationException($"Cannot access static field '{field}' on an instance.")
                : new FieldSymbol(self.Context, field, self);
        }

        /// <summary>
        /// Create a field symbol and using this symbol as its target instance.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="field">Metadata of the field.</param>
        /// <returns>Field symbol bound to this symbol.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the specified field is static,
        /// or if the field type is not assignable to the specified type.
        /// </exception>
        public FieldSymbol<TField> Field<TField>(FieldInfo field)
        {
            if (!field.FieldType.BasicType.IsAssignableTo(typeof(TField)))
                throw new InvalidCastException(
                    "Field type is not assignable to the specified representation type.");
            return field.IsStatic
                ? throw new InvalidOperationException($"Cannot access static field '{field}' on an instance.")
                : new FieldSymbol<TField>(self.Context, field, self);
        }
    }

    extension<TContent>(ISymbol<TContent> self)
    {
        /// <summary>
        /// Create a strongly typed instance field symbol using a lambda expression selector.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="selector">The lambda expression selecting the field.</param>
        /// <returns>A strongly typed field symbol bound to this symbol for the selected field.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the selector is not a field access or when attempting to access a static field.
        /// </exception>
        public FieldSymbol<TField> Field<TField>(Expression<Func<TContent, TField>> selector)
        {
            if (selector.Body is not MemberExpression { Member: FieldInfo field })
                throw new InvalidOperationException("The selector expression is not a field access.");
            return field.IsStatic
                ? throw new InvalidOperationException($"Cannot access static field '{field}' on an instance.")
                : new FieldSymbol<TField>(self.Context, field, self);
        }
    }

    extension(DynamicFunction self)
    {
        /// <summary>
        /// Create a field symbol for accessing a static field.
        /// </summary>
        /// <param name="field">The field information.</param>
        /// <returns>A field symbol for the specified field.</returns>
        /// <exception cref="InvalidOperationException">Thrown when attempting to access a static field.</exception>
        public FieldSymbol Field(FieldInfo field)
        {
            return !field.IsStatic
                ? throw new InvalidOperationException(
                    $"Cannot access instance field '{field}' without specifying a target instance.")
                : new FieldSymbol(self, field);
        }

        /// <summary>
        /// Create a strongly typed field symbol for accessing a static field.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="field">The field information.</param>
        /// <returns>A strongly typed field symbol for the specified field.</returns>
        /// <exception cref="InvalidCastException">
        /// Thrown when the field type is not assignable to the specified type.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when attempting to access a static field.
        /// </exception>
#nullable disable
        public FieldSymbol<TField> Field<TField>(FieldInfo field)
#nullable restore
        {
            if (!field.FieldType.BasicType.IsAssignableTo(typeof(TField)))
                throw new InvalidCastException(
                    "Field type is not assignable to the specified representation type.");
            return !field.IsStatic
                ? throw new InvalidOperationException(
                    $"Cannot access instance field '{field}' without specifying a target instance.")
                : new FieldSymbol<TField>(self, field);
        }

        /// <summary>
        /// Create a strongly typed field symbol for a static field using a lambda expression selector.
        /// </summary>
        /// <typeparam name="TField">The type of the field.</typeparam>
        /// <param name="selector">The lambda expression selecting the static field.</param>
        /// <returns>A strongly typed field symbol for the selected static field.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the selector is not a field access or when attempting to access an instance field.
        /// </exception>
#nullable disable
        public FieldSymbol<TField> Field<TField>(Expression<Func<TField>> selector)
#nullable restore
        {
            if (selector.Body is not MemberExpression { Member: FieldInfo field })
                throw new InvalidOperationException("The selector expression is not a field access.");
            return !field.IsStatic
                ? throw new InvalidOperationException(
                    $"Cannot access instance field '{field}' without specifying a target instance.")
                : new FieldSymbol<TField>(self, field);
        }
    }
}