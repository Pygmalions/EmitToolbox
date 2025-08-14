using System.Collections;
using System.Reflection.Emit;
using JetBrains.Annotations;

namespace EmitToolbox.Extensions;

public static class EmitEnumerableExtensions
{
    public static void EmitForEach(this ILGenerator code, Type elementType,
        [InstantHandle] Action<ILGenerator> enumeratorLoader,
        [InstantHandle] Action<ILGenerator> enumerationAction)
    {
        // Load the enumerator.
        enumeratorLoader(code);
        code.CallVirtual(typeof(IEnumerable<>).MakeGenericType(elementType)
            .GetMethod(nameof(IEnumerable<object>.GetEnumerator))!);

        // Cache methods of the enumerator.
        var methodMoveNext = typeof(IEnumerator).GetMethod(nameof(IEnumerator.MoveNext))!;
        var methodGetCurrent = typeof(IEnumerator<>).MakeGenericType(elementType)
            .GetProperty(nameof(IEnumerator<object>.Current))!
            .GetMethod!;

        var labelLoopBegin = code.DefineLabel();
        var labelLoopEnd = code.DefineLabel();

        code.MarkLabel(labelLoopBegin);

        // Move next.
        code.Emit(OpCodes.Dup);
        code.CallVirtual(methodMoveNext);
        code.GotoIfFalse(labelLoopEnd);

        // Get current element.
        code.Emit(OpCodes.Dup);
        code.CallVirtual(methodGetCurrent);

        enumerationAction(code);

        code.Goto(labelLoopBegin);

        code.MarkLabel(labelLoopEnd);

        code.Emit(OpCodes.Pop);
    }
}