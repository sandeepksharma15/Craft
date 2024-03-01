using System.Globalization;
using System.Linq.Expressions;
using Craft.Extensions.Expressions;
using Craft.Components.Table.Contracts;
using Craft.Components.Table.Enums;
using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace Craft.Components.Table;

public partial class Column<TableItem> : IColumn<TableItem>
{
    private bool _visible;
    private string _caption;
    private Func<TableItem, object> renderCompiled;

    [CascadingParameter(Name = "DataTable")] public IDataTable<TableItem> Table { get; set; }

    [Parameter] public Alignment Align { get; set; }
    [Parameter] public string Caption { get; set; }
    [Parameter] public string Class { get; set; }
    [Parameter] public bool? DefaultSortColumn { get; set; }
    [Parameter] public bool? DefaultSortDescending { get; set; }
    [Parameter] public Expression<Func<TableItem, object>> Field { get; set; }
    [Parameter] public Expression<Func<TableItem, bool>> Filter { get; set; }
    [Parameter] public bool Filterable { get; set; }
    [Parameter] public string Format { get; set; }
    [Parameter] public bool Hideable { get; set; }
    [Parameter] public bool Sortable { get; set; }
    [Parameter] public SortDirection SortDirection { get; set; }
    [Parameter] public RenderFragment<TableItem> Template { get; set; }
    [Parameter] public Type Type { get; set; }

    [Parameter] public bool Visible { get; set; } = true;

    [Parameter] public string Width { get; set; }

    //protected override async Task OnParametersSetAsync()
    //{
    //    bool isRefreshRequired = false;

    //    await base.OnParametersSetAsync();

    //    if (_caption is null)
    //    {
    //        _caption = Caption ?? Field.GetMemberName();
    //        isRefreshRequired = true;
    //    }

    //    if (_visible != Visible)
    //    {
    //        _visible = Visible;
    //        isRefreshRequired = true;
    //    }

    //    if (isRefreshRequired)
    //        Table.RefreshTable();
    //}

    protected override void OnInitialized()
    {
        base.OnInitialized();

        Table.AddColumn(this);

        if (DefaultSortColumn.HasValue)
            SortDirection = SortDirection.Ascending;

        if (DefaultSortDescending.HasValue)
            SortDirection = SortDirection.Descending;
    }

    protected override void OnParametersSet()
    {
        bool isRefreshRequired = false;

        if ((Sortable && Field == null) || (Filterable && Field == null))
            throw new InvalidOperationException($"Column {Caption} Property parameter is null");

        if (Caption == null && Field == null)
            throw new InvalidOperationException("A Column has both Caption and Property parameters null");

        if (Type == null)
            Type = Field.GetMemberType();

        if (_caption is null)
        {
            _caption = Caption ?? Field.GetMemberName();
            isRefreshRequired = true;
        }

        if (_visible != Visible)
        {
            _visible = Visible;
            isRefreshRequired = true;
        }

        if (isRefreshRequired)
            Table.RefreshTable();
    }

    public string Render(TableItem item)
    {
        object value = null;

        if (item == null || Field == null)
            return string.Empty;

        renderCompiled ??= Field.Compile();

        try
        {
            value = renderCompiled.Invoke(item);
        }
        catch (NullReferenceException) { }

        if (value == null)
            return string.Empty;

        if (Field?.GetMemberType() == typeof(bool))
            return (bool)value ? "Yes" : "No";

        if (Field?.GetMemberType().IsEnum == true)
            return ((Enum)value).GetDescription();

        if (string.IsNullOrEmpty(Format))
            return value.ToString();

        return string.Format(CultureInfo.CurrentCulture, $"{{0:{Format}}}", value);
    }

    public Task SortByAsync()
    {
        throw new NotImplementedException();
    }
}
