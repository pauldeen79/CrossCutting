namespace CrossCutting.DataTableDumper.Abstractions;

public interface IColumnNameProvider
{
    IReadOnlyCollection<string> Get<T>()
        where T : class;
}
