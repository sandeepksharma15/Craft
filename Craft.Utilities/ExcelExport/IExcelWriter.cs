namespace Craft.Utilities.ExcelExport;

public interface IExcelWriter
{
    Stream WriteToStream<T>(IList<T> data);
}
