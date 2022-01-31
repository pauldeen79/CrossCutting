namespace CrossCutting.Data.Core;

public class PagedResult<T> : IPagedResult<T>
{
    private readonly IReadOnlyCollection<T> _records;

    public int TotalRecordCount { get; }

    public int Offset { get; }

    public int PageSize { get; }

    public int Count => _records.Count;

    public PagedResult(IEnumerable<T> records, int totalRecordCount, int offset, int pageSize)
    {
        TotalRecordCount = totalRecordCount;
        Offset = offset;
        PageSize = pageSize;
        _records = new List<T>(records).AsReadOnly();
    }

    public IEnumerator<T> GetEnumerator() => _records.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _records.GetEnumerator();
}
