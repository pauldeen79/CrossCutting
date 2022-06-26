namespace CrossCutting.DataTableDumper.Default;

public class ColumnNameProvider : IColumnNameProvider
{
    public IReadOnlyCollection<string> Get<T>() where T : class
    {
        var result = new List<string>();
        foreach (var property in TypeDescriptor.GetProperties(typeof(T)).Cast<PropertyDescriptor>())
        {
            result.Add(property.Name);
        }

        return result;
    }
}
