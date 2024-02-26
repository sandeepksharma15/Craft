using System.Text.Json.Serialization;

namespace Craft.Domain.Helpers;

/// <summary>
/// Represents a paginated response containing a collection of items and pagination information.
/// </summary>
/// <typeparam name="T">The type of items in the collection.</typeparam>
[Serializable]
public class PageResponse<T> : PaginationInfo where T : class
{
    /// <summary>
    /// The collection of items in the current page.
    /// </summary>
    public IEnumerable<T> Items { get; }

    [JsonConstructor]
    public PageResponse(IEnumerable<T> items, ulong totalCount, uint currentPage, uint pageSize)
    {
        Items = items ?? [];
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
}
