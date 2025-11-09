using EmitToolbox.Framework.Extensions;

namespace EmitToolbox.Framework.Symbols;

public class ArgumentSymbol(DynamicMethod context, int index, Type type)
    : IAssignableSymbol, IAddressableSymbol
{
    public DynamicMethod Context { get; } = context;

    public Type ContentType { get; } = type;

    public int Index { get; } = index;

    public void EmitContent()
        => Context.Code.Emit(OpCodes.Ldarg, Index);

    public void EmitAddress()
        => Context.Code.Emit(OpCodes.Ldarga, Index);

    public void Assign(ISymbol other)
    {
        other.EmitForSymbol(this);
        Context.Code.Emit(OpCodes.Starg, Index);
    }

    public void AssignContentFromStack()
        => Context.Code.Emit(OpCodes.Starg, Index);
}

public class ArgumentSymbol<TContent>
    : IAssignableSymbol<TContent>, IAddressableSymbol<TContent>
    where TContent : allows ref struct
{
    private readonly ArgumentSymbol _symbol;

    internal ArgumentSymbol(ArgumentSymbol symbol)
    {
        _symbol = symbol;
    }

    public ArgumentSymbol(DynamicMethod context, int index, ContentModifier? modifier = null)
        : this(new ArgumentSymbol(context, index, modifier.Decorate<TContent>()))
    {
    }

    public DynamicMethod Context => _symbol.Context;

    public Type ContentType => _symbol.ContentType;
    
    public void EmitContent() => _symbol.EmitContent();

    public void Assign(ISymbol<TContent> other) => _symbol.Assign(other);

    public void EmitAddress() => _symbol.EmitAddress();

    public void AssignContentFromStack() => _symbol.AssignContentFromStack();
}

public static class ArgumentSymbolExtensions
{
    extension(DynamicMethod self)
    {
        /// <summary>
        /// Get a symbol for the argument at the specified position.
        /// </summary>
        /// <param name="position">
        /// Position of the argument in the argument list (excluding 'this'), starting from 0.
        /// </param>
        /// <param name="type">Content type of this argument symbol.</param>
        public ArgumentSymbol Argument(int position, Type type)
            => new(self, position + (self.BuildingMethod.IsStatic ? 0 : 1), type);

        /// <summary>
        /// Get a symbol for the argument at the specified position.
        /// </summary>
        /// <param name="position">
        /// Position of the argument in the argument list (excluding 'this'), starting from 0.
        /// </param>
        /// <param name="modifier">
        /// Content type modifier of this argument symbol.
        /// </param>
        /// <typeparam name="TContent">Basic type of this symbol.</typeparam>
        /// <returns>Symbol for the argument at the specified position.</returns>
        public ArgumentSymbol<TContent> Argument<TContent>(int position, ContentModifier? modifier = null)
            where TContent : allows ref struct
            => new(self, position + (self.BuildingMethod.IsStatic ? 0 : 1), modifier);

        public ArgumentSymbol This()
        {
            if (self.BuildingMethod.IsStatic)
                throw new Exception("Cannot get 'this' argument for static method.");
            var type = self.BuildingMethod.DeclaringType
                       ?? throw new Exception("Cannot get 'this' argument for method without declaring type.");
            if (type.IsValueType)
                type = type.MakeByRefType();
            return new ArgumentSymbol(self, 0, type);
        }

        public ArgumentSymbol<TContent> This<TContent>()
            where TContent : allows ref struct
        {
            if (self.BuildingMethod.IsStatic)
                throw new Exception("Cannot get 'this' argument for static method.");
            var type = self.BuildingMethod.DeclaringType
                       ?? throw new Exception("Cannot get 'this' argument for method without declaring type.");
            if (!type.IsAssignableTo(typeof(TContent)))
                throw new Exception(
                    "Cannot get 'this' argument as the specified type for method with incompatible type.");
            return new ArgumentSymbol<TContent>(self, 0, 
                type.IsValueType ? ContentModifier.Reference : ContentModifier.None);
        }
    }
}