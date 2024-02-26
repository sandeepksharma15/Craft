namespace Craft.Domain.QueryModels;

public class PaginationInfo
{
    private long _totalCount;
    private int _pageSize;
    private int _currentPage;

    /// <summary>
    /// The current page number (1-based indexing).
    /// </summary>
    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 1, nameof(value));
            _currentPage = value;
        }
    }

    /// <summary>
    /// The number of items per page.
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value,1,nameof(value));
            _pageSize = value;
        }
    }

    /// <summary>
    /// The total number of items in the data set.
    /// </summary>
    public long TotalCount
    {
        get => _totalCount;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 0, nameof(value));
            _totalCount = value;
        }
    }

    /// <summary>
    /// Calculates the starting index of the current page (1-based indexing).
    /// </summary>
    public int From => ((CurrentPage - 1) * PageSize) + 1;

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
    public int To => (int)(From + Math.Min(PageSize - 1, TotalCount - From));

    /// <summary>
    /// Calculates the total number of pages based on the total count and page size.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
