using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Data.Abstractions;

namespace CrossCutting.Data.Sql
{
    internal class PagedResult<T> : IPagedResult<T>
    {
        private readonly IReadOnlyCollection<T> _records;

        public int TotalRecordCount { get; }

        public int Count => _records.Count;

        public PagedResult(IEnumerable<T> records, int totalRecordCount)
        {
            TotalRecordCount = totalRecordCount;
            _records = new List<T>(records ?? Enumerable.Empty<T>()).AsReadOnly();
        }

        public IEnumerator<T> GetEnumerator() => _records.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _records.GetEnumerator();
    }
}
