using System.Linq.Expressions;
using Craft.Components.Table.Enums;
using Microsoft.AspNetCore.Components;

namespace Craft.Components.Table.Contracts;

public interface IColumn<TableItem>
{
    Alignment Align { get; set; }

    string Caption { get; set; }
    string Class { get; set; }

    bool? DefaultSortColumn { get; set; }
    bool? DefaultSortDescending { get; set; }

    Expression<Func<TableItem, object>> Field { get; set; }
    Expression<Func<TableItem, bool>> Filter { get; set; }

    bool Filterable { get; set; }
    string Format { get; set; }
    bool Hideable { get; set; }
    bool Sortable { get; set; }

    SortDirection SortDirection { get; set; }

    IDataTable<TableItem> Table { get; set; }

    RenderFragment<TableItem> Template { get; set; }
    Type Type { get; set; }

    bool Visible { get; set; }
    string Width { get; set; }

    string Render(TableItem item);

    Task SortByAsync();
}
