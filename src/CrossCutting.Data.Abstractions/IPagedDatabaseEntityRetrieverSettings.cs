namespace CrossCutting.Data.Abstractions
{
    public interface IPagedDatabaseEntityRetrieverSettings : IDatabaseEntityRetrieverSettings
    {
        int? OverridePageSize { get; }
    }
}
