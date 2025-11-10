using EmitToolbox.Framework.Symbols;
using EmitToolbox.Framework.Symbols.Operations;

namespace EmitToolbox.Framework.Extensions;

public static class EqualityExtensions
{
    private class EqualityByObjectEquals(ISymbol self, ISymbol other) : 
        OperationSymbol<bool>([self, other])
    {
        public override void LoadContent()
        {
            self.EmitAsObject();
            other.EmitAsObject();
            Context.Code.Emit(OpCodes.Callvirt, 
                typeof(object).GetMethod("Equals", [typeof(object)])!);
        }
    }
    
    private class EqualityByReferenceEquals(ISymbol self, ISymbol other) : 
        OperationSymbol<bool>([self, other])
    {
        public override void LoadContent()
        {
            if (self.BasicType.IsValueType || other.BasicType.IsValueType)
            {
                Context.Code.Emit(OpCodes.Ldc_I4_0);
                return;
            }
            
            self.EmitAsObject();
            other.EmitAsObject();
            Context.Code.Emit(OpCodes.Call, 
                typeof(object).GetMethod("ReferenceEquals", 
                    [typeof(object), typeof(object)])!);
        }
    }

    private class EqualityByInstruction(ISymbol self, ISymbol other) :
        OperationSymbol<bool>([self, other])
    {
        public override void LoadContent()
        {
            self.LoadAsValue();
            other.LoadAsValue();
            Context.Code.Emit(OpCodes.Ceq);
        }
    }
    
    extension(ISymbol self)
    {
        /// <summary>
        /// Invoke the 'Equals' method on the content of this symbol:
        /// <br/> 1. If this symbol is a primitive type, then the operation OpCodes.Ceq will be used.
        /// <br/> 2. If this symbol has an overload of 'Equals' with the type of the other symbol,
        /// then this overload will be invoked.
        /// <br/> 3. The 'Equals' method of the 'object' class will be invoked.
        /// </summary>
        /// <param name="other">Another symbol for the 'Equals' method to compare.</param>
        /// <returns>Invocation result.</returns>
        public OperationSymbol<bool> InvokeEquals(ISymbol other)
        {
            if (self.BasicType == other.BasicType && 
                self.BasicType is { IsPrimitive: true })
                return new EqualityByInstruction(self, other);
            
            if (self.BasicType.GetMethod(
                    "op_Equality", BindingFlags.Public | BindingFlags.Static,
                    [self.BasicType, other.BasicType]) is { } operatorMethod)
                return new InvocationOperation<bool>(operatorMethod, null, [self, other]);
            if (self.BasicType.GetMethod(
                    nameof(object.Equals), [other.BasicType]) is { } specializedMethod)
                return new InvocationOperation<bool>(specializedMethod, self, [other]);
            return new EqualityByObjectEquals(self, other);
        }

        /// <summary>
        /// Invoke the 'Equals' method on the content of this symbol.
        /// </summary>
        /// <param name="other">Another symbol for the 'Equals' method to compare.</param>
        /// <returns>Invocation result.</returns>
        public OperationSymbol<bool> InvokeReferenceEquals(ISymbol other)
            => new EqualityByReferenceEquals(self, other);
    }
}