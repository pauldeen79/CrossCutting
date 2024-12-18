﻿namespace CrossCutting.DataTableDumper;

public class DataTableDumper<T>(IColumnNameProvider columnNameProvider, IColumnDataProvider<T> columnDataProvider) : IDataTableDumper<T>
    where T : class
{
    private readonly IColumnNameProvider _columnNameProvider = columnNameProvider;
    private readonly IColumnDataProvider<T> _columnDataProvider = columnDataProvider;

    public string Dump(IEnumerable<T> data, string escapeValue)
    {
        var builder = new StringBuilder();
        var columnNames = _columnNameProvider.Get<T>();
        var columnLengths = GetColumnLengths(columnNames, data, escapeValue);

        builder.Append("|");
        foreach (var column in columnNames.Select((x, index) => new { Length = columnLengths[index], Name = x }))
        {
            builder.Append(" ")
                   .Append(column.Name.PadRight(column.Length, ' '))
                   .Append(" |");
        }
        builder.AppendLine();

        foreach (var item in data)
        {
            builder.Append("|");
            var columnData = _columnDataProvider.Get(item, escapeValue);
            foreach (var column in columnData.Select((x, index) => new { Length = columnLengths[index], Value = x }))
            {
                builder.Append(" ")
                       .Append(column.Value.PadRight(column.Length, ' '))
                       .Append(" |");
            }
            builder.AppendLine();
        }

        return builder.ToString();
    }

    private int[] GetColumnLengths(IReadOnlyCollection<string> columnNames, IEnumerable<T> data, string escapeValue)
    {
        var columnLengths = new int[columnNames.Count];

        foreach (var column in columnNames.Select((x, index) => new { Length = x?.Length ?? 0, Index = index }))
        {
            if (column.Length > columnLengths[column.Index])
            {
                columnLengths[column.Index] = column.Length;
            }
        }

        foreach (var item in data)
        {
            var columnData = _columnDataProvider.Get(item, escapeValue);
            foreach (var column in columnData.Select((x, index) => new { Length = x?.Length ?? 0, Index = index }))
            {
                if (column.Length > columnLengths[column.Index])
                {
                    columnLengths[column.Index] = column.Length;
                }
            }
        }

        return columnLengths;
    }
}
