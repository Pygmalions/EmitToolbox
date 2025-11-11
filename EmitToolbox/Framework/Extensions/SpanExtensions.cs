using System.Runtime.CompilerServices;
using EmitToolbox.Framework.Symbols;

namespace EmitToolbox.Framework.Extensions;

public static class SpanExtensions
{
    public class SpanItemReference<TElement>(ISymbol<Span<TElement>> target, ISymbol<int> index)
        : OperationSymbol<TElement>([target], ContentModifier.Reference) where TElement : struct
    {
        public override void LoadContent()
        {
            target.LoadAsTarget();
            index.LoadAsValue();
            Context.Code.Emit(OpCodes.Call, typeof(Span<TElement>).GetMethod("get_Item",
                BindingFlags.Public | BindingFlags.Instance,
                [typeof(int)])!);
        }
    }
    
    extension(DynamicFunction self)
    {
        public VariableSymbol<Span<TContent>> StackAllocate<TContent>(ISymbol<int> length)
            where TContent : struct
        {
            var code = self.Code;
            
            var variable = self.Variable<Span<TContent>>();
            
            (length * self.Value(Unsafe.SizeOf<TContent>()))
                .ToUIntPtr()
                .LoadContent();
            code.Emit(OpCodes.Conv_U);
            code.Emit(OpCodes.Localloc);
            length.LoadContent();
            // 'OpCodes.Newobj' is used here to creating instances of value types on the stack.
            // This cannot be replaced with 'OpCodes.Call <.ctor>'.
            code.Emit(OpCodes.Newobj, 
                typeof(Span<int>).GetConstructor([typeof(void*), typeof(int)])!);
            variable.StoreContent();
            return variable;
        }
    }
    
    extension<TElement>(ISymbol<Span<TElement>> self) where TElement : struct
    {
        public SpanItemReference<TElement> ElementAt(ISymbol<int> index)
            => new(self, index);
    }
}