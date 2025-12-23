using EmitToolbox.Builders;
using EmitToolbox.Symbols;

namespace EmitToolbox.Extensions;

public static class EnumerableExtensions
{
    extension<TElement>(ISymbol<IEnumerable<TElement>> self)
    {
        public void ForEach(Action<ISymbol<TElement>> action)
        {
            var enumerator =
                self.Invoke(target => target.GetEnumerator())
                    .ToSymbol();

            var succeeded =
                enumerator.Invoke(target => target.MoveNext());
            var element =
                enumerator.GetPropertyValue(target => target.Current);

            using var loop = self.Context.While(succeeded);
            action(element);
        }
    }
}