namespace CrossCutting.DataTableDumper.Default;

public class ColumnNameProvider<T> : IColumnNameProvider<T>
    where T : class
{
    public IReadOnlyCollection<string> Get()
    {
        var result = new List<string>();
        foreach (var property in TypeDescriptor.GetProperties(typeof(T)).Cast<PropertyDescriptor>())
        {
            result.Add(property.Name);
        }

        return result;
    }
}
