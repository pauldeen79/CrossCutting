namespace CrossCutting.Data.Abstractions;

public interface IPagedResult<out T> : IReadOnlyCollection<T>
{
    int TotalRecordCount { get; }
    int Offset { get; }
    int PageSize { get; }
}
