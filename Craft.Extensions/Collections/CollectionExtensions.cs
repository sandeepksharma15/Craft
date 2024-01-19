namespace System.Collections.Generic;

public static class CollectionExtensions
{
    /// <summary>
    /// Determines whether the specified collection is null or empty. Offers a convenient way to check for both conditions in a single expression, improving readability and reducing code verbosity.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="source">The collection to check for null or emptiness.</param>
    /// <returns>True if the collection is null or contains no elements, false otherwise.</returns>
    public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        => source == null || source.Count == 0;
}
