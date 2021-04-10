using System.Collections.Generic;

namespace CrossCutting.DataTableDumper.Abstractions
{
    public interface IColumnNameProvider<in T>
        where T : class
    {
        IReadOnlyCollection<string> Get();
    }
}
