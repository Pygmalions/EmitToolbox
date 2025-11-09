using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework.Extensions;

public static class ArrayExtensions
{
    public class ElementSymbol<TContent>(ISymbol<TContent[]> array, ISymbol<int> index) :
        IAddressableSymbol<TContent>, IAssignableSymbol<TContent>
    {
        public ISymbol<TContent[]> Array { get; } = array;

        public ISymbol<int> Index { get; } = index;
        public Type ContentType { get; } = typeof(TContent);

        public DynamicMethod Context { get; } = array.Context;

        public void EmitContent()
        {
            var code = Context.Code;
            Array.EmitAsValue();
            Index.EmitAsValue();
            if (typeof(TContent).IsValueType)
                code.Emit(OpCodes.Ldelem, typeof(TContent));
            else
                code.Emit(OpCodes.Ldelem_Ref);
        }

        public void EmitAddress()
        {
            var code = Context.Code;
            Array.EmitAsValue();
            Index.EmitAsValue();
            code.Emit(OpCodes.Ldelema, typeof(TContent));
        }

        public void Assign(ISymbol<TContent> other)
        {
            var code = Context.Code;
            Array.EmitAsValue();
            Index.EmitAsValue();
            other.EmitContent();

            if (typeof(TContent).IsValueType)
                code.Emit(OpCodes.Stelem, typeof(TContent));
            else
                code.Emit(OpCodes.Stelem_Ref);
        }
    }

    private class GettingArrayLength<TContent>(ISymbol<TContent[]> array)
        : OperationSymbol<int>([array])
    {
        public override void EmitContent()
        {
            array.EmitAsValue();
            Context.Code.Emit(OpCodes.Ldlen);
        }
    }

    extension<TContent>(ISymbol<TContent[]> self)
    {
        public OperationSymbol<int> Length => new GettingArrayLength<TContent>(self);
        public ElementSymbol<TContent> ElementAt(ISymbol<int> index) => new(self, index);
    }

    extension(DynamicMethod self)
    {
        public VariableSymbol<TContent[]> NewArray<TContent>(ISymbol<int> length)
        {
            var variable = self.Variable<TContent[]>();
            length.EmitAsValue();
            self.Code.Emit(OpCodes.Newarr, typeof(TContent));
            variable.AssignContentFromStack();
            return variable;
        }
    }
}