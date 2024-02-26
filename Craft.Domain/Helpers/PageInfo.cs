namespace Craft.Domain.Helpers;

public class PageInfo
{
    public uint CurrentPage { get; set; } = 1;
    public uint PageSize { get; set; } = 10;
    public ulong TotalCount { get; set; } = 0;

    /// <summary>
    /// Calculates the starting index of the current page (1-based indexing).
    /// </summary>
    public uint From => ((CurrentPage - 1) * PageSize) + 1;

    /// <summary>
    /// Indicates whether there is a next page available.
    /// </summary>
    public bool HasNextPage => CurrentPage < TotalPages;

    /// <summary>
    /// Indicates whether there is a previous page available.
    /// </summary>
    public bool HasPreviousPage => CurrentPage > 1;

    /// <summary>
    /// Calculates the ending index for the current page, considering available items.
    /// </summary>
    public uint To => (uint)(From + Math.Min(PageSize - 1, TotalCount - From));

    /// <summary>
    /// Calculates the total number of pages based on the total count and page size.
    /// </summary>
    public uint TotalPages => (uint)Math.Ceiling(TotalCount / (double)PageSize);
}
