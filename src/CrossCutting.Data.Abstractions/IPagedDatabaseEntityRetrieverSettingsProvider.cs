namespace CrossCutting.Data.Abstractions;

public interface IPagedDatabaseEntityRetrieverSettingsProvider
{
    Result<IPagedDatabaseEntityRetrieverSettings> Get<TSource>();
}
