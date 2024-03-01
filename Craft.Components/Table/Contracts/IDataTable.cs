using Craft.Components.UiBuilder;
using Craft.Components.Table.Enums;

namespace Craft.Components.Table.Contracts;

public interface IDataTable
{
    int PageSize { get; }
    int PageIndex { get; }
    bool Pagination { get; set; }

    public bool Hoverable { get; set; }
    public bool Striped { get; set; }
    public bool Bordered { get; set; }
    public bool FullWidth { get; set; }

    bool AddButton { get; set; }
    string AddUri { get; set; }
    bool BackButton { get; set; }
    bool EditIcon { get; set; }
    string EditUri { get; set; }
    bool DeleteIcon { get; set; }

    string TableBodyClass { get; set; }
    string TableClass { get; set; }
    string TableFooterClass { get; set; }
    string TableHeaderClass { get; set; }

    PageHeader PageHeaderRef { get; set; }

    SelectionType SelectionType { get; set; }

    bool ShowSearchBar { get; set; }
    bool ShowTableFooter { get; set; }

    void AddEntity();

    Task DeleteEntityAsync(string id);

    void EditEntity(string url);

    void GoBack();

    Task PageUpdate();

    void RefreshTable();

    Task SetPageSizeAsync(int pageSize);
}

public interface IDataTable<T> : IDataTable
{
    List<IColumn<T>> Columns { get; }

    Action<T> RowClickAction { get; set; }

    List<T> SelectedItems { get; }

    void AddColumn(IColumn<T> column);

    void RemoveColumn(IColumn<T> column);

    Task SortTableAsync(IColumn<T> column);
}
