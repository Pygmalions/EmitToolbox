using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework.Extensions;

public static class InstantiationExtensions
{
    extension(DynamicFunction self)
    {
        [Pure]
        public VariableSymbol New(ConstructorInfo constructor, IEnumerable<ISymbol>? arguments = null)
        {
            var type = constructor.DeclaringType ?? 
                       throw new ArgumentException("Specified constructor does not have a declaring type.");
            var code = self.Code;
            var variable = self.Variable(type);
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

        /// <summary>
        /// Instantiate a new instance using the default constructor,
        /// and store it in a local variable.
        /// </summary>
        /// <typeparam name="TContent">Type to instantiate.</typeparam>
        /// <returns>Variable symbol which holds the created instance.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the specified type does not have a default constructor.
        /// </exception>
        [Pure]
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

        /// <summary>
        /// Instantiate a new instance using the specified constructor,
        /// and store it in a local variable.
        /// </summary>
        /// <typeparam name="TContent">Type to instantiate.</typeparam>
        /// <param name="constructor">Constructor to use for instantiation.</param>
        /// <param name="arguments">Arguments to pass to the constructor.</param>
        /// <returns>Variable symbol which holds the created instance.</returns>
        [Pure]
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
        
        /// <summary>
        /// Instantiate a new instance using the constructor specified by an expression,
        /// and store it in a local variable.
        /// </summary>
        /// <typeparam name="TContent">Type to instantiate.</typeparam>
        /// <param name="constructorSelector">Expression that selects the constructor to use for instantiation.</param>
        /// <param name="arguments">Arguments to pass to the constructor.</param>
        /// <returns>Variable symbol which holds the created instance.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the specified expression is not a 'NewExpression' or contains a null constructor.
        /// </exception>
        [Pure]
        public VariableSymbol<TContent> New<TContent>(
            Expression<Func<TContent>> constructorSelector,
            IEnumerable<ISymbol>? arguments = null)
            where TContent : allows ref struct
        {
            if (constructorSelector.Body is not NewExpression expression)
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
        /// If this symbol holds a reference,
        /// then the new instance will be stored at the address pointed by the reference.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the basic type of this symbol does not have a default constructor,
        /// or it is abstract or an interface.
        /// </exception>
        public void AssignNew()
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
            self.AssignNew(constructor);
        }
        
        /// <summary>
        /// Instantiate a new instance using the specified constructor.
        /// If this symbol holds a reference,
        /// then the new instance will be stored at the address pointed by the reference.
        /// </summary>
        /// <param name="constructor">Constructor to use for instantiation.</param>
        /// <param name="arguments">Arguments to pass to the constructor.</param>
        /// <param name="inplace">
        /// If true, the instantiation will be forced to perform in-place:
        /// the new instance is constructed at the same address of the previous instance held by this symbol,
        /// as a consequence, the previous instance itself will be overwritten.
        /// If it is null, then it will be true for value types and false for reference types:
        /// reference types will always be instantiated at a new memory address.
        /// </param>
        public void AssignNew(
            ConstructorInfo constructor, 
            IEnumerable<ISymbol>? arguments = null, 
            bool? inplace = null)
        {
            var type = constructor.DeclaringType
                ?? throw new ArgumentException("Specified constructor does not have a declaring type.");
            inplace ??= type.IsValueType;
            
            if (type.IsPrimitive || type == typeof(string) || type.IsEnum || 
                (type.IsArray && type.GetArrayRank() == 1) || type.IsGenericTypeDefinition)
                throw new InvalidOperationException(
                    "Cannot perform instantiation on primitive types, strings, enums, one-dimension arrays " +
                    "or generic type definitions.");
            
            var code = self.Context.Code;
            arguments ??= [];
            
            if (inplace.Value)
            {
                self.LoadAsTarget();
                foreach (var (symbol, parameter) in arguments.Zip(constructor.GetParameters()))
                    symbol.LoadForParameter(parameter);
                code.Emit(OpCodes.Call, constructor);
                return;
            }

            if (self.ContentType.IsByRef)
                self.LoadContent();
            
            foreach (var (symbol, parameter) in arguments.Zip(constructor.GetParameters()))
                symbol.LoadForParameter(parameter);
            // Assign the new object instance on the heap, or the new struct instance on the stack.
            code.Emit(OpCodes.Newobj, constructor);

            // If the type is a value type and this symbol holds a reference,
            // then it will match the value-can-load-as-reference condition.
            if (self.ContentType.IsByRef)
            {
                if (type.IsValueType)
                    code.Emit(OpCodes.Stobj, type);
                else
                    code.Emit(OpCodes.Stind_Ref);
            }
            else
                self.StoreContent();
        }
    }
    
    extension<TContent>(IAssignableSymbol<TContent> self) where TContent : allows ref struct
    {
        /// <summary>
        /// Instantiate a new instance at the specified symbol using the specified constructor.
        /// If this symbol holds a reference,
        /// then the new instance will be stored at the address pointed by the reference.
        /// </summary>
        /// <param name="constructorSelector">Expression selector for the constructor to use.</param>
        /// <param name="arguments">Arguments to pass to the constructor.</param>
        /// <typeparam name="TContent">Type to instantiate.</typeparam>
        /// <param name="inplace">
        /// If true, the instantiation will be forced to perform in-place:
        /// the new instance is constructed at the same address of the previous instance held by this symbol,
        /// as a consequence, the previous instance itself will be overwritten.
        /// If it is null, then it will be true for value types and false for reference types:
        /// reference types will always be instantiated at a new memory address.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when the specified expression is not a 'NewExpression' or contains a null constructor.
        /// </exception>
        public void AssignNew(
            Expression<Func<TContent>> constructorSelector,
            IEnumerable<ISymbol>? arguments = null,
            bool? inplace = null)
        {
            if (constructorSelector.Body is not NewExpression expression)
                throw new ArgumentException(
                    "Specified expression is not a 'NewExpression'.", nameof(constructorSelector));
            if (expression.Constructor is null)
                throw new ArgumentException(
                    "Specified expression contains a null constructor.", nameof(constructorSelector));
            self.AssignNew(expression.Constructor, arguments, inplace);
        }
    }

    private static void Initialize(IAddressableSymbol target)
    {
        var type = target.BasicType;
        if (!type.IsValueType)
            throw new InvalidOperationException($"Cannot initialize a symbol of a non-value type '{type}'.");
        target.LoadAsReference();
        target.Context.Code.Emit(OpCodes.Initobj, target.BasicType);
    }
    
    public static void Initialize<TContent>(this IAddressableSymbol<TContent> self) where TContent : struct
        => Initialize((IAddressableSymbol)self);
    
    public static void Initialize<TContent>(this IAddressableSymbol<TContent?> self) where TContent : struct
        => Initialize((IAddressableSymbol)self);
}