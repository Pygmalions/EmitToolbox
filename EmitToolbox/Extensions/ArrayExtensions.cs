using System.Diagnostics.Contracts;
using EmitToolbox.Symbols;
using EmitToolbox.Symbols.Literals;

namespace EmitToolbox.Extensions;

public static class ArrayExtensions
{
    public class ElementSymbol<TContent>(ISymbol<TContent[]> array, ISymbol<int> index) :
        IAddressableSymbol<TContent>, IAssignableSymbol<TContent>
    {
        public ISymbol<TContent[]> Array { get; } = array;

        public ISymbol<int> Index { get; } = index;
        
        public Type ContentType { get; } = typeof(TContent);

        public DynamicFunction Context { get; } = array.Context;

        public void LoadContent()
        {
            var code = Context.Code;
            Array.LoadAsValue();
            Index.LoadAsValue();
            if (typeof(TContent).IsValueType)
                code.Emit(OpCodes.Ldelem, typeof(TContent));
            else
                code.Emit(OpCodes.Ldelem_Ref);
        }

        public void LoadAddress()
        {
            var code = Context.Code;
            Array.LoadAsValue();
            Index.LoadAsValue();
            code.Emit(OpCodes.Ldelema, typeof(TContent));
        }

        public void AssignContent(ISymbol<TContent> other)
        {
            var code = Context.Code;
            Array.LoadAsValue();
            Index.LoadAsValue();
            other.LoadContent();

            if (typeof(TContent).IsValueType)
                code.Emit(OpCodes.Stelem, typeof(TContent));
            else
                code.Emit(OpCodes.Stelem_Ref);
        }

        public void StoreContent()
        {
            var value = Context.Variable<TContent>();
            value.StoreContent();
            
            AssignContent(value);
        }
    }

    private class GettingArrayLength<TContent>(ISymbol<TContent[]> array)
        : OperationSymbol<int>([array])
    {
        public override void LoadContent()
        {
            array.LoadAsValue();
            Context.Code.Emit(OpCodes.Ldlen);
        }
    }

    extension<TContent>(ISymbol<TContent[]> self)
    {
        public IOperationSymbol<int> Length => new GettingArrayLength<TContent>(self);
        
        [Pure]
        public ElementSymbol<TContent> ElementAt(ISymbol<int> index) => new(self, index);
        
        [Pure]
        public ElementSymbol<TContent> ElementAt(int index) 
            => new(self, new LiteralInteger32Symbol(self.Context, index));
    }

    extension<TElement>(IAssignableSymbol<TElement[]> self)
    {
        public void AssignNew(ISymbol<int> length)
        {
            var code = self.Context.Code;
            if (self.ContentType.IsByRef)
                self.LoadContent();
            length.LoadAsValue();
            code.Emit(OpCodes.Newarr, typeof(TElement));
            if (self.ContentType.IsByRef)
                code.Emit(OpCodes.Stind_Ref);
            else
                self.StoreContent();
        }
        
        public void AssignNew(int length)
            => self.AssignNew(new LiteralInteger32Symbol(self.Context, length));
    }

    extension(DynamicFunction self)
    {
        [Pure]
        public VariableSymbol<TContent[]> NewArray<TContent>(ISymbol<int> length)
        {
            var variable = self.Variable<TContent[]>();
            length.LoadAsValue();
            self.Code.Emit(OpCodes.Newarr, typeof(TContent));
            variable.StoreContent();
            return variable;
        }
        
        [Pure]
        public VariableSymbol<TContent[]> NewArray<TContent>(int length)
            => self.NewArray<TContent>(new LiteralInteger32Symbol(self, length));
    }
}