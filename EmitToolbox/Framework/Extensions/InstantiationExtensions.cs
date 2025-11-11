using System.Linq.Expressions;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework.Extensions;

public static class InstantiationExtensions
{
    public class InstantiatingStruct<TContent>(
        IAddressableSymbol<TContent> target,
        ConstructorInfo constructor,
        IReadOnlyCollection<ISymbol> arguments) : OperationSymbol<TContent>([target])
        where TContent : allows ref struct
    {
        public override void LoadContent()
        {
            target.LoadAddress();
            Context.Code.Emit(OpCodes.Initobj, typeof(TContent));
            target.LoadAddress();
            foreach (var (argument, parameter) in arguments.Zip(constructor.GetParameters()))
                argument.LoadForParameter(parameter);
            Context.Code.Emit(OpCodes.Call, constructor);
            target.LoadAddress();
        }
    }

    public class InstantiatingClass<TContent>(
        DynamicFunction context,
        ConstructorInfo constructor,
        IReadOnlyCollection<ISymbol> arguments) : OperationSymbol<TContent>(context)
        where TContent : allows ref struct
    {
        public override void LoadContent()
        {
            foreach (var (argument, parameter) in arguments.Zip(constructor.GetParameters()))
                argument.LoadForParameter(parameter);
            Context.Code.Emit(OpCodes.Newobj, constructor);
        }
    }

    extension(DynamicFunction self)
    {
        public VariableSymbol<TContent> New<TContent>() where TContent : allows ref struct
        {
            var constructor = typeof(TContent).GetConstructor(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                Type.EmptyTypes);
            return constructor == null
                ? throw new InvalidOperationException(
                    $"Type '{typeof(TContent)}' does not have a default constructor.")
                : self.New<TContent>(constructor);
        }

        public VariableSymbol<TContent> New<TContent>(
            ConstructorInfo constructor,
            IEnumerable<ISymbol>? arguments = null)
            where TContent : allows ref struct
        {
            var type = typeof(TContent);
            var code = self.Code;
            var variable = self.Variable<TContent>();
            if (type.IsValueType)
                variable.LoadAddress();
            if (arguments != null)
            {
                foreach (var (symbol, parameter) in arguments.Zip(constructor.GetParameters()))
                    symbol.LoadForParameter(parameter);
            }
            if (type.IsValueType)
            {
                code.Emit(OpCodes.Call, constructor);
                return variable;
            }

            code.Emit(OpCodes.Newobj, constructor);
            variable.StoreContent();
            return variable;
        }

        public VariableSymbol<TContent> New<TContent>(
            Expression<Func<TContent>> constructorSelector,
            IEnumerable<ISymbol>? arguments = null)
            where TContent : allows ref struct
        {
            if (constructorSelector.Body is not NewExpression { } expression)
                throw new ArgumentException(
                    "Specified expression is not a 'NewExpression'.", nameof(constructorSelector));
            if (expression.Constructor is null)
                throw new ArgumentException(
                    "Specified expression contains a null constructor.", nameof(constructorSelector));
            return self.New<TContent>(expression.Constructor, arguments);
        }
    }

    extension(IAssignableSymbol self)
    {
        /// <summary>
        /// Instantiate a new instance using the default constructor.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the basic type of this symbol does not have a default constructor,
        /// or it is abstract or an interface.
        /// </exception>
        public void EmplaceNew()
        {
            if (self.BasicType.IsAbstract || self.BasicType.IsInterface)
                throw new InvalidOperationException(
                    $"Cannot instantiate content type '{self.BasicType}': " +
                    $"it is abstract or an interface.");
            var constructor = self.BasicType.GetConstructor(Type.EmptyTypes);
            if (constructor is null)
                throw new InvalidOperationException(
                    $"Cannot instantiate content type '{self.BasicType}': " +
                    $"it does not have a public parameterless constructor.");
            self.EmplaceNew(constructor);
        }
        
        /// <summary>
        /// Instantiate a new instance using the specified constructor.
        /// </summary>
        /// <param name="constructor">Constructor to use for instantiation.</param>
        /// <param name="arguments">Arguments to pass to the constructor.</param>
        public void EmplaceNew(
            ConstructorInfo constructor,
            IEnumerable<ISymbol>? arguments = null)
        {
            var type = constructor.DeclaringType
                ?? throw new ArgumentException("Specified constructor does not have a declaring type.");
            if (self.ContentType.IsByRef)
                throw new InvalidOperationException("Cannot instantiate an instance at a by-ref symbol.");
            var code = self.Context.Code;
            if (type.IsValueType && self is IAddressableSymbol addressable)
            {
                addressable.LoadAddress();
                if (arguments != null)
                {
                    foreach (var (symbol, parameter) in arguments.Zip(constructor.GetParameters()))
                        symbol.LoadForParameter(parameter);
                }
                code.Emit(OpCodes.Call, constructor);
                return;
            }

            if (arguments != null)
            {
                foreach (var (symbol, parameter) in arguments.Zip(constructor.GetParameters()))
                    symbol.LoadForParameter(parameter);
            }
            // Assign the new object instance on the heap, or the new struct instance on the stack.
            code.Emit(OpCodes.Newobj, constructor);
            self.StoreContent();
        }
    }
    
    extension<TContent>(IAssignableSymbol<TContent> self) where TContent : allows ref struct
    {
        /// <summary>
        /// Instantiate a new instance at the specified symbol using the specified constructor.
        /// </summary>
        /// <param name="constructorSelector">Expression selector for the constructor to use.</param>
        /// <param name="arguments">Arguments to pass to the constructor.</param>
        /// <typeparam name="TContent">Type to instantiate.</typeparam>
        /// <exception cref="ArgumentException">
        /// Thrown when the specified expression is not a 'NewExpression' or contains a null constructor.
        /// </exception>
        public void EmplaceNew(
            Expression<Func<TContent>> constructorSelector,
            IEnumerable<ISymbol>? arguments = null)
        {
            if (constructorSelector.Body is not NewExpression { } expression)
                throw new ArgumentException(
                    "Specified expression is not a 'NewExpression'.", nameof(constructorSelector));
            if (expression.Constructor is null)
                throw new ArgumentException(
                    "Specified expression contains a null constructor.", nameof(constructorSelector));
            self.EmplaceNew(expression.Constructor, arguments);
        }
    }
}