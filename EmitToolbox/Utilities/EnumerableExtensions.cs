namespace EmitToolbox.Utilities;

public static class EnumerableExtensions
{
    /// <summary>
    /// Zip two sequences together, ensuring that both sequences have the same length.
    /// </summary>
    /// <returns>Sequence of element pairs from the two specified sequences.</returns>
    /// <exception cref="Exception">
    /// Thrown when one sequence has fewer elements than the other.
    /// </exception>
    public static IEnumerable<(TFirst, TSecond)> StrictlyZip<TFirst, TSecond>(
        this IEnumerable<TFirst> first, IEnumerable<TSecond> second)
    {
        using var firstEnumerator = first.GetEnumerator();
        using var secondEnumerator = second.GetEnumerator();
        while (firstEnumerator.MoveNext())
        {
            if (!secondEnumerator.MoveNext())
                throw new Exception("Insufficient elements in the second sequence.");
            yield return (firstEnumerator.Current, secondEnumerator.Current);
        }
        if (secondEnumerator.MoveNext())
            throw new Exception("Insufficient elements in the first sequence.");
    }
}