using System.Collections.Generic;

namespace CrossCutting.Data.Abstractions
{
    public interface IPagedResult<out T> : IReadOnlyCollection<T>
    {
        int TotalRecordCount { get; }
    }
}
