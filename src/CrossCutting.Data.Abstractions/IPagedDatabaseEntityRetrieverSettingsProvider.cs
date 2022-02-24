namespace CrossCutting.Data.Abstractions;

public interface IPagedDatabaseEntityRetrieverSettingsProvider
{
    bool TryGet<TSource>(out IPagedDatabaseEntityRetrieverSettings? settings);
}
