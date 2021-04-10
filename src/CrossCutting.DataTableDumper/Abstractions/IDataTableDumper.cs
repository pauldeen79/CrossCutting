using System.Collections.Generic;

namespace CrossCutting.DataTableDumper.Abstractions
{
    public interface IDataTableDumper<in T>
        where T : class
    {
        string Dump(IEnumerable<T> data);
    }
}
