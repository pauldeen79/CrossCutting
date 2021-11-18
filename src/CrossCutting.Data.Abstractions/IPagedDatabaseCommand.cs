namespace CrossCutting.Data.Abstractions
{
    public interface IPagedDatabaseCommand
    {
        IDatabaseCommand DataCommand { get; }
        IDatabaseCommand RecordCountCommand { get; }
        int Offset { get; }
        int PageSize { get; }
    }
}
