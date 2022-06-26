namespace CrossCutting.DataTableDumper.Extensions;

public static class DataTableDumperExtensions
{
    public static string Dump<T>(this IDataTableDumper<T> instance, IEnumerable<T> data) where T : class
        => instance.Dump(data, "_");
}
