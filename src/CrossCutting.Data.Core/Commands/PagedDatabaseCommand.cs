using CrossCutting.Data.Abstractions;

namespace CrossCutting.Data.Core.Commands
{
    public class PagedDatabaseCommand : IPagedDatabaseCommand
    {
        public IDatabaseCommand DataCommand { get; }
        public IDatabaseCommand RecordCountCommand { get; }
        public int Offset { get; }
        public int PageSize { get; }

        public PagedDatabaseCommand(IDatabaseCommand dataCommand,
                                    IDatabaseCommand recordCountCommand,
                                    int offset,
                                    int pageSize)
        {
            DataCommand = dataCommand;
            RecordCountCommand = recordCountCommand;
            Offset = offset;
            PageSize = pageSize;
        }
    }
}
