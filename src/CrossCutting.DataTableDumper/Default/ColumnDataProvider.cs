using CrossCutting.DataTableDumper.Abstractions;
using CrossCutting.DataTableDumper.Extensions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CrossCutting.DataTableDumper.Default
{
    public class ColumnDataProvider<T> : IColumnDataProvider<T>
        where T : class
    {
        public IReadOnlyCollection<string> Get(T item)
        {
            var result = new List<string>();
            foreach (var property in TypeDescriptor.GetProperties(typeof(T)).Cast<PropertyDescriptor>())
            {
                result.Add((property.GetValue(item)?.ToString() ?? string.Empty).EscapePipes());
            }

            return result;
        }
    }
}
