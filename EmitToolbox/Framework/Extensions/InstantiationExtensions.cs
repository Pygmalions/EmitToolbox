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
            params IEnumerable<ISymbol> arguments)
            where TContent : allows ref struct
        {
            var type = typeof(TContent);
            var code = self.Code;
            var variable = self.Variable<TContent>();
            if (type.IsValueType)
                variable.LoadAddress();
            foreach (var (symbol, parameter) in arguments.Zip(constructor.GetParameters()))
                symbol.LoadForParameter(parameter);
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
            params IEnumerable<ISymbol> arguments)
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
        
        /// <summary>
        /// Instantiate a new instance at the specified symbol using the default constructor.
        /// </summary>
        /// <param name="target">Target symbol to instantiate an instance at.</param>
        /// <typeparam name="TContent">Type to instantiate.</typeparam>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the specified type does not have a default constructor.
        /// </exception>
        public void EmplaceNew<TContent>(IAssignableSymbol<TContent> target)
            where TContent : allows ref struct
        {
            var constructor = typeof(TContent).GetConstructor(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                Type.EmptyTypes);
            if (constructor is null)
                throw new InvalidOperationException(
                    $"Type '{typeof(TContent)}' does not have a default constructor.");
            self.EmplaceNew(target, constructor);
        }
        
        /// <summary>
        /// Instantiate a new instance at the specified symbol using the specified constructor.
        /// </summary>
        /// <param name="target">Target symbol to instantiate an instance at.</param>
        /// <param name="constructorSelector">Expression selector for the constructor to use.</param>
        /// <param name="arguments">Arguments to pass to the constructor.</param>
        /// <typeparam name="TContent">Type to instantiate.</typeparam>
        /// <exception cref="ArgumentException">
        /// Thrown when the specified expression is not a 'NewExpression' or contains a null constructor.
        /// </exception>
        public void EmplaceNew<TContent>(
            IAssignableSymbol<TContent> target,
            Expression<Func<TContent>> constructorSelector,
            params IEnumerable<ISymbol> arguments)
            where TContent : allows ref struct
        {
            if (constructorSelector.Body is not NewExpression { } expression)
                throw new ArgumentException(
                    "Specified expression is not a 'NewExpression'.", nameof(constructorSelector));
            if (expression.Constructor is null)
                throw new ArgumentException(
                    "Specified expression contains a null constructor.", nameof(constructorSelector));
            self.EmplaceNew(target, expression.Constructor, arguments);
        }
        
        /// <summary>
        /// Instantiate a new instance at the specified symbol using the specified constructor.
        /// </summary>
        /// <param name="target">Target symbol to instantiate an instance at.</param>
        /// <param name="constructor">Constructor to use for instantiation.</param>
        /// <param name="arguments">Arguments to pass to the constructor.</param>
        /// <typeparam name="TContent">Type to instantiate.</typeparam>
        public void EmplaceNew<TContent>(
            IAssignableSymbol<TContent> target,
            ConstructorInfo constructor,
            params IEnumerable<ISymbol> arguments)
            where TContent : allows ref struct
        {
            var type = typeof(TContent);
            var code = self.Code;
            
            if (type.IsValueType && target is IAddressableSymbol addressable)
            {
                addressable.LoadAddress();
                foreach (var (symbol, parameter) in arguments.Zip(constructor.GetParameters()))
                    symbol.LoadForParameter(parameter);
                code.Emit(OpCodes.Call, constructor);
                return;
            }
            
            foreach (var (symbol, parameter) in arguments.Zip(constructor.GetParameters()))
                symbol.LoadForParameter(parameter);
            // Assign the new object instance on the heap, or the new struct instance on the stack.
            code.Emit(OpCodes.Newobj, constructor);
            target.StoreContent();
        }
    }
}