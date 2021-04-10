using CrossCutting.DataTableDumper.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossCutting.DataTableDumper
{
    public class DataTableDumper<T> : IDataTableDumper<T>
        where T : class
    {
        private readonly IColumnNameProvider<T> _columnNameProvider;
        private readonly IColumnDataProvider<T> _columnDataProvider;

        public DataTableDumper(IColumnNameProvider<T> columnNameProvider, IColumnDataProvider<T> columnDataProvider)
        {
            _columnNameProvider = columnNameProvider ?? throw new ArgumentNullException(nameof(columnNameProvider));
            _columnDataProvider = columnDataProvider ?? throw new ArgumentNullException(nameof(columnDataProvider));
        }
        public string Dump(IEnumerable<T> data)
        {
            var builder = new StringBuilder();
            var columnNames = _columnNameProvider.Get();
            var columnLengths = GetColumnLengths(columnNames, data);

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
                var columnData = _columnDataProvider.Get(item);
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

        private int[] GetColumnLengths(IReadOnlyCollection<string> columnNames, IEnumerable<T> data)
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
                var columnData = _columnDataProvider.Get(item);
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
}
