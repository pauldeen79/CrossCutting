namespace CrossCutting.Data.Abstractions;

#pragma warning disable CA1710 // Identifiers should have correct suffix
public interface IPagedResult<out T> : IReadOnlyCollection<T>
#pragma warning restore CA1710 // Identifiers should have correct suffix
{
    int TotalRecordCount { get; }
    int Offset { get; }
    int PageSize { get; }
}
