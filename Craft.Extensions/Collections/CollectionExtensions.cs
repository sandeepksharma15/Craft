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

    public static bool AddIfNotContains<T>(this ICollection<T> source, T item)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        if (source.Contains(item)) return false;

        source.Add(item);

        return true;
    }

    public static IEnumerable<T> AddIfNotContains<T>(this ICollection<T> source, IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        List<T> addedItems = [];

        foreach (T item in items)
        {
            if (source.Contains(item))
                continue;

            source.Add(item);
            addedItems.Add(item);
        }

        return addedItems;
    }

    public static bool AddIfNotContains<T>(this ICollection<T> source, Func<T, bool> predicate, Func<T> itemFactory)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));
        ArgumentNullException.ThrowIfNull(itemFactory, nameof(itemFactory));

        if (source.Any(predicate)) return false;

        source.Add(itemFactory());

        return true;
    }

    public static IList<T> RemoveAll<T>(this ICollection<T> source, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

        List<T> items = source.Where(predicate).ToList();

        foreach (var item in items)
            source.Remove(item);

        return items;
    }

    public static void RemoveAll<T>(this ICollection<T> source, IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        foreach (var item in items)
            source.Remove(item);
    }
}
