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
        public override void EmitContent()
        {
            target.EmitAddress();
            Context.Code.Emit(OpCodes.Initobj, typeof(TContent));
            target.EmitAddress();
            foreach (var (argument, parameter) in arguments.Zip(constructor.GetParameters()))
                argument.EmitForParameter(parameter);
            Context.Code.Emit(OpCodes.Call, constructor);
            target.EmitAddress();
        }
    }
    
    public class InstantiatingClass<TContent>(
        DynamicMethod context,
        ConstructorInfo constructor, 
        IReadOnlyCollection<ISymbol> arguments) : OperationSymbol<TContent>(context)
        where TContent : allows ref struct
    {
        public override void EmitContent()
        {
            foreach (var (argument, parameter) in arguments.Zip(constructor.GetParameters()))
                argument.EmitForParameter(parameter);
            Context.Code.Emit(OpCodes.Newobj, constructor);
        }
    }
    
    extension(DynamicMethod self)
    {
        public VariableSymbol<TContent> New<TContent>()
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
        {
            var type = typeof(TContent);
            var code = self.Code;
            var variable = self.Variable<TContent>();
            if (type.IsValueType)
                variable.EmitAddress();
            foreach (var (symbol, parameter) in arguments.Zip(constructor.GetParameters()))
                symbol.EmitForParameter(parameter);
            if (type.IsValueType)
            {
                code.Emit(OpCodes.Call, constructor);
                return variable;
            }
            code.Emit(OpCodes.Newobj, constructor);
            variable.AssignContentFromStack();
            return variable;
        }

        public VariableSymbol<TContent> New<TContent>(
            Expression<Func<TContent>> constructorSelector,
            params IEnumerable<ISymbol> arguments)
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
}