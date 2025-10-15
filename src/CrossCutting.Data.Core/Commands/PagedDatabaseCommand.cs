namespace CrossCutting.Data.Core.Commands;

public class PagedDatabaseCommand(IDatabaseCommand dataCommand,
                                  IDatabaseCommand recordCountCommand,
                                  int offset,
                                  int pageSize) : IPagedDatabaseCommand
{
    public IDatabaseCommand DataCommand { get; } = dataCommand;
    public IDatabaseCommand RecordCountCommand { get; } = recordCountCommand;
    public int Offset { get; } = offset;
    public int PageSize { get; } = pageSize;
}
