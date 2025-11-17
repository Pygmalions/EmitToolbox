using EmitToolbox.Framework.Symbols;
using EmitToolbox.Framework.Utilities;
using JetBrains.Annotations;

namespace EmitToolbox.Framework.Extensions;

public static class ConsoleExtensions
{
    public readonly struct ConsoleFacade(DynamicFunction context)
    {
        public void Write(ISymbol<object?> value)
            => context.Invoke(() => Console.Write(Any<object?>.Value), [value]);

        public void Write(ISymbol<string> message)
            => context.Invoke(() => Console.Write(Any<string>.Value), [message]);

        public void Write(ISymbol<string> template, ISymbol argument0)
            => context.Invoke(() => Console.Write(Any<string>.Value, Any<object?>.Value),
                [template, argument0.ToObject()]);

        public void Write(ISymbol<string> template, ISymbol argument0, ISymbol argument1)
            => context.Invoke(() => Console.Write(Any<string>.Value, Any<object?>.Value, Any<object?>.Value),
                [template, argument0.ToObject(), argument1.ToObject()]);

        public void Write(ISymbol<string> template,
            ISymbol argument0, ISymbol argument1, ISymbol argument2)
            => context.Invoke(() => Console.Write(
                    Any<string>.Value, Any<object?>.Value, Any<object?>.Value, Any<object?>.Value),
                [template, argument0.ToObject(), argument1.ToObject(), argument2.ToObject()]);
        
        public void Write([StructuredMessageTemplate] string template, ISymbol argument0)
            => context.Invoke(() => Console.Write(Any<string>.Value, Any<object?>.Value),
                [context.Value(template), argument0.ToObject()]);

        public void Write([StructuredMessageTemplate] string template, ISymbol argument0, ISymbol argument1)
            => context.Invoke(() => Console.Write(Any<string>.Value, Any<object?>.Value, Any<object?>.Value),
                [context.Value(template), argument0.ToObject(), argument1.ToObject()]);

        public void Write([StructuredMessageTemplate] string template,
            ISymbol argument0, ISymbol argument1, ISymbol argument2)
            => context.Invoke(() => Console.Write(
                    Any<string>.Value, Any<object?>.Value, Any<object?>.Value, Any<object?>.Value),
                [context.Value(template), argument0.ToObject(), argument1.ToObject(), argument2.ToObject()]);
        
        public void WriteLine(ISymbol<object?> value)
            => context.Invoke(() => Console.WriteLine(Any<object?>.Value), [value]);
        
        public void WriteLine(ISymbol<string> message)
            => context.Invoke(() => Console.WriteLine(Any<string>.Value), [message]);

        public void WriteLine(ISymbol<string> template, ISymbol argument0)
            => context.Invoke(() => Console.WriteLine(Any<string>.Value, Any<object?>.Value),
                [template, argument0.ToObject()]);

        public void WriteLine(ISymbol<string> template, ISymbol argument0, ISymbol argument1)
            => context.Invoke(() => Console.WriteLine(Any<string>.Value, Any<object?>.Value, Any<object?>.Value),
                [template, argument0.ToObject(), argument1.ToObject()]);

        public void WriteLine(ISymbol<string> template,
            ISymbol argument0, ISymbol argument1, ISymbol argument2)
            => context.Invoke(() => Console.WriteLine(
                    Any<string>.Value, Any<object?>.Value, Any<object?>.Value, Any<object?>.Value),
                [template, argument0.ToObject(), argument1.ToObject(), argument2.ToObject()]);
        
        public void WriteLine([StructuredMessageTemplate] string template, ISymbol argument0)
            => context.Invoke(() => Console.WriteLine(Any<string>.Value, Any<object?>.Value),
                [context.Value(template), argument0.ToObject()]);

        public void WriteLine([StructuredMessageTemplate] string template, ISymbol argument0, ISymbol argument1)
            => context.Invoke(() => Console.WriteLine(Any<string>.Value, Any<object?>.Value, Any<object?>.Value),
                [context.Value(template), argument0.ToObject(), argument1.ToObject()]);

        public void WriteLine([StructuredMessageTemplate] string template,
            ISymbol argument0, ISymbol argument1, ISymbol argument2)
            => context.Invoke(() => Console.WriteLine(
                    Any<string>.Value, Any<object?>.Value, Any<object?>.Value, Any<object?>.Value),
                [context.Value(template), argument0.ToObject(), argument1.ToObject(), argument2.ToObject()]);

        [System.Diagnostics.Contracts.Pure]
        public IOperationSymbol<int> Read()
            => context.Invoke(() => Console.Read());
        
        [System.Diagnostics.Contracts.Pure]
        public IOperationSymbol<string?> ReadLine()
            => context.Invoke(() => Console.ReadLine());
        
        public void Clear()
            => context.Invoke(() => Console.Clear());
    }

    extension(DynamicFunction self)
    {
        public ConsoleFacade Console => new(self);
    }
}