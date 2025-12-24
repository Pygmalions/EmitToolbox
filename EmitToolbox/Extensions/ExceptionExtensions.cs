using System.Linq.Expressions;
using EmitToolbox.Symbols;
using EmitToolbox.Symbols.Literals;

namespace EmitToolbox.Extensions;

public static class ExceptionExtensions
{
    extension<TException>(ISymbol<TException> self)
    {
        public void Throw()
        {
            self.LoadAsValue();
            self.Context.Code.Emit(OpCodes.Throw);
        }
    }
    
    extension(DynamicFunction self)
    {
        public void ThrowException<TException>() where TException : Exception
        {
            var constructor = typeof(TException).GetConstructor(Type.EmptyTypes)
                              ?? throw new ArgumentException(
                                  $"Cannot find a parameterless constructor for '{typeof(TException)}'.");
            self.ThrowException(constructor);
        }
        
        public void ThrowException<TException>(ISymbol<string> message) where TException : Exception
        {
            var constructor = typeof(TException).GetConstructor([typeof(string)])
                ?? throw new ArgumentException(
                    $"Cannot find a constructor for '{typeof(TException)}' that only takes a message string.");
            self.ThrowException(constructor, [message]);
        }

        public void ThrowException<TException>(string message) where TException : Exception
            => self.ThrowException<TException>(new LiteralStringSymbol(self, message));

        public void ThrowException()
            => self.ThrowException<Exception>();
        
        public void ThrowException(ISymbol<string> message)
            => self.ThrowException<Exception>(message);
        
        public void ThrowException(string message)
            => self.ThrowException(new LiteralStringSymbol(self, message));

        public void ThrowException(ConstructorInfo constructor, IEnumerable<ISymbol>? arguments = null)
        {
            var code = self.Code;
            arguments?.LoadForParameters(constructor.GetParameters());
            code.Emit(OpCodes.Newobj, constructor);
            code.Emit(OpCodes.Throw);
        }
        
        public void ThrowException<TException>(
            Expression<Func<TException>> constructorSelector,
            IEnumerable<ISymbol>? arguments = null) where TException : Exception
        {
            if (constructorSelector.Body is not NewExpression expression)
                throw new ArgumentException(
                    "Specified expression is not a 'NewExpression'.", nameof(constructorSelector));
            if (expression.Constructor is null)
                throw new ArgumentException(
                    "Specified expression contains a null constructor.", nameof(constructorSelector));
            self.ThrowException(expression.Constructor, arguments);
        }
    }
}