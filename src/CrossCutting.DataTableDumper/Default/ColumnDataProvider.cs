namespace CrossCutting.DataTableDumper.Default;

public class ColumnDataProvider<T> : IColumnDataProvider<T>
    where T : class
{
    public IReadOnlyCollection<string> Get(T item, string escapeValue)
    {
        var result = new List<string>();
        foreach (var property in TypeDescriptor.GetProperties(typeof(T)).Cast<PropertyDescriptor>())
        {
            result.Add((property.GetValue(item)?.ToString()).EscapePipes(escapeValue) ?? string.Empty);
        }

        return result;
    }
}
