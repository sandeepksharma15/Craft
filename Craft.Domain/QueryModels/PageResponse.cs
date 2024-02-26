using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Craft.Domain.QueryModels;

/// <summary>
/// Represents a paginated response containing a collection of items and pagination information.
/// </summary>
/// <typeparam name="T">The type of items in the collection.</typeparam>
[Serializable]
public class PageResponse<T> : PaginationInfo, ISerializable where T : class
{
    /// <summary>
    /// The collection of items in the current page.
    /// </summary>
    public IEnumerable<T> Items { get; }

    [JsonConstructor]
    public PageResponse(IEnumerable<T> items, long totalCount, int currentPage, int pageSize)
    {
        Items = items ?? [];
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public PageResponse(SerializationInfo info, StreamingContext context)
    {
        Items = (IEnumerable<T>)info.GetValue(nameof(Items), typeof(IEnumerable<T>));
        CurrentPage = info.GetInt32(nameof(CurrentPage));
        PageSize = info.GetInt32(nameof(PageSize));
        TotalCount = info.GetInt64(nameof(TotalCount));
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(Items), Items);
        info.AddValue(nameof(CurrentPage), CurrentPage);
        info.AddValue(nameof(PageSize), PageSize);
        info.AddValue(nameof(TotalCount), TotalCount);
    }
}
