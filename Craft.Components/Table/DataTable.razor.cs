using Craft.Components.Generic;
using Craft.Components.UiBuilder;
using Craft.Domain.Contracts;
using Craft.Domain.HttpServices;
using Craft.QuerySpec.Core;
using Craft.Components.Table.Contracts;
using Craft.Components.Table.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MudBlazor;
using Craft.Domain.Helpers;
using Craft.Utilities.Builders;
using Craft.Components.Navigations;
using Craft.QuerySpec.Contracts;

namespace Craft.Components.Table;

public partial class DataTable<TableItem> : IDataTable<TableItem>
    where TableItem : class, IEntity, IModel
{
    private List<TableItem> _entities;
    private PageInfo _pagenInfo { get; set; }
    private IQuery<TableItem> _tableQuery { get; set; }

    private readonly List<IColumn<TableItem>> _sortedColumns = [];
    private readonly List<IColumn<TableItem>> _filteredColumns = [];

    private readonly RenderFragment _renderColumnHeaders;
    private readonly RenderFragment _renderTableBody;
    private readonly RenderFragment _renderPageFooter;
    private readonly RenderFragment _renderPageButtons;
    private readonly RenderFragment _renderPageDropdown;
    private readonly RenderFragment _renderPagination;
    private int _pageSize;
    private int _pageIndex;

    private int CurrentPageSize
    {
        get { return _pageSize; }
        set
        {
            _pageSize = value;
            Task.Run(async () => await SetPageSizeAsync(_pageSize));
        }
    }

    private int CurrentPageIndex
    {
        get { return _pageIndex; }
        set
        {
            _pageIndex = value;
            Task.Run(async () => await UpdateTableAsync());
        }
    }

    [CascadingParameter] public HandleError? handleError { get; set; }
    [Inject] private ILogger<IDataTable<TableItem>> _logger { get; set; }
    [Inject] private NavigationManager _navigationManager { get; set; }
    [Inject] private ISnackbar _snackbar { get; set; }

    [Parameter] public RenderFragment ChildContent { get; set; }
    [Parameter] public IHttpService<TableItem> DataStore { get; set; }

    [Parameter] public int PageSize { get; set; } = 10;
    [Parameter] public int PageIndex { get; set; } = 1;
    [Parameter] public bool Pagination { get; set; } = true;

    [Parameter] public bool Hoverable { get; set; }
    [Parameter] public bool Striped { get; set; }
    [Parameter] public bool Bordered { get; set; }
    [Parameter] public bool Outlined { get; set; }
    [Parameter] public bool FullWidth { get; set; }

    [Parameter] public bool AddButton { get; set; }
    [Parameter] public string AddUri { get; set; } = string.Empty;
    [Parameter] public string AddButtonName { get; set; } = "Add New";
    [Parameter] public string AddButtonIcon { get; set; } = "Icons.Material.Outlined.AddCircle";

    [Parameter] public bool BackButton { get; set; }
    [Parameter] public string BackButtonIcon { get; set; } = "Icons.Material.Outlined.ArrowBack";
    [Parameter] public bool EditIcon { get; set; }
    [Parameter] public string EditUri { get; set; } = string.Empty;
    [Parameter] public bool DeleteIcon { get; set; }
    [Parameter] public bool ReadIcon { get; set; } = true;
    [Parameter] public string ReadURI { get; set; } = string.Empty;

    [Parameter] public string TableBodyClass { get; set; }
    [Parameter] public string TableClass { get; set; }
    [Parameter] public string TableFooterClass { get; set; }
    [Parameter] public string TableHeaderClass { get; set; }

    [Parameter] public PageHeader PageHeaderRef { get; set; }

    /// <summary>
    /// <para>
    /// Optionally defines a value for @key on each rendered row. Typically this should be used to specify a
    /// unique identifier, such as a primary key value, for each data item.
    /// </para>
    /// <para>
    /// This allows the grid to preserve the association between row elements and data items based on their
    /// unique identifiers, even when the TGridItem instances are replaced by new copies (for
    /// example, after a new query against the underlying data store).
    /// </para>
    /// <para>If not set, the @key will be the TableItem instance itself.</para>
    /// </summary>
    [Parameter] public Func<TableItem, object> ItemKey { get; set; } = x => x!;

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object> UnknownParameters { get; set; }

    public List<IColumn<TableItem>> Columns { get; } = [];

    public Action<TableItem> RowClickAction { get; set; }
    public List<TableItem> SelectedItems { get; }
    public SelectionType SelectionType { get; set; }
    public bool ShowSearchBar { get; set; }
    public bool ShowTableFooter { get; set; }

    public DataTable()
    {
        Columns = [];

        _tableQuery = new Query<TableItem>();

        _pageIndex = PageIndex;
        _pageSize = PageSize;

        _renderColumnHeaders = RenderColumnHeaders;
        _renderTableBody = RenderTableBody;
        _renderPageFooter = RenderPageFooter;
        _renderPageButtons = RenderPageButtons;
        _renderPageDropdown = RenderPageDropdown;
        _renderPagination = RenderPagination;
    }

    protected override async Task OnParametersSetAsync()
    {
        await UpdateTableAsync();
    }

    public void AddColumn(IColumn<TableItem> column)
    {
        column.Table = this;
        column.Type ??= column.Field.GetType();
        Columns.Add(column);

        if (column.DefaultSortColumn == true && column.DefaultSortDescending == true)
            _tableQuery.OrderByDescending(column.Field);
        else if (column.DefaultSortColumn == true)
            _tableQuery.OrderBy(column.Field);

        RefreshTable();
    }

    protected string ClassName =>
        new CssBuilder("table")
            .AddClass("table-striped", Striped)
            .AddClass("table-" + (Bordered ? "bordered" : "borderless"))
            .AddClass("table-hover", Hoverable)
            .AddClass("table-full-width", FullWidth)
            .AddClass(TableClass)
        .Build();

    public void AddEntity()
        => _navigationManager.NavigateTo(AddUri);

    public Task DeleteEntityAsync(string id)
    {
        throw new NotImplementedException();
    }

    public void EditEntity(string url)
        => _navigationManager.NavigateTo(url);

    public void GoBack()
        => _navigationManager?.GoBack();

    public Task PageUpdate()
    {
        throw new NotImplementedException();
    }

    public void RefreshTable()
        => InvokeAsync(StateHasChanged);

    public void RemoveColumn(IColumn<TableItem> column)
    {
        throw new NotImplementedException();
    }

    public async Task SetPageSizeAsync(int pageSize)
        => await UpdateTableAsync();

    public async Task SortTableAsync(IColumn<TableItem> column)
    {
        if (column.Sortable && column.Field != null)
        {
            switch (column.SortDirection)
            {
                case Craft.Components.Table.Enums.SortDirection.None:
                    column.SortDirection = Craft.Components.Table.Enums.SortDirection.Ascending;
                    _sortedColumns.Add(column);
                    break;
                case Craft.Components.Table.Enums.SortDirection.Ascending:
                    column.SortDirection = Craft.Components.Table.Enums.SortDirection.Descending;
                    break;
                case Craft.Components.Table.Enums.SortDirection.Descending:
                    column.SortDirection= Craft.Components.Table.Enums.SortDirection.None;
                    _sortedColumns.Remove(column);
                    break;
            }

            //_tableQuery.OrderBuilder.Clear();

            //foreach (var item in _sortedColumns)
            //{
            //    if (_tableQuery.OrderBuilder.OrderExpressions.Count == 0)
            //    {
            //        if (item.SortDirection == Enums.SortDirection.Ascending)
            //            _tableQuery.OrderBy(item.Field);
            //        else
            //            _tableQuery.OrderByDescending(item.Field);
            //    }
            //    else
            //    {
            //        if (item.SortDirection == Enums.SortDirection.Ascending)
            //            _tableQuery.ThenBy(item.Field);
            //        else
            //            _tableQuery.ThenByDescending(item.Field);
            //    }
            //}

            await UpdateTableAsync();
        }
    }

    public async Task UpdateTableAsync()
    {
        await GetTableDataAsync();

        RefreshTable();
    }

    private async Task GetTableDataAsync()
    {
        try
        {
            if (_tableQuery.IsWithoutOrder())
                _tableQuery.OrderBy(x => x.Id);

            _logger.LogDebug($"PageIndex: [{CurrentPageIndex}] PageSize: [{CurrentPageSize}]");

            _tableQuery.SetPage(CurrentPageIndex, CurrentPageSize);

            var result = await DataStore.GetPagedListAsync(_tableQuery);

            _entities = [.. result.Items];

            _pagenInfo = new PageInfo
            {
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[DataTable] Type: [\"{typeof(TableItem).GetClassName()}\"] There was trouble getting the data");
            handleError?.ProcessError(ex);
        }
    }
}
