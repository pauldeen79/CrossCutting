namespace CrossCutting.Data.Core;

public class PagedResult<T>(IEnumerable<T> records, int totalRecordCount, int offset, int pageSize) : IPagedResult<T>
{
    private readonly IReadOnlyCollection<T> _records = new List<T>(records).AsReadOnly();

    public int TotalRecordCount { get; } = totalRecordCount;

    public int Offset { get; } = offset;

    public int PageSize { get; } = pageSize;

    public int Count => _records.Count;

    public IEnumerator<T> GetEnumerator() => _records.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _records.GetEnumerator();
}
