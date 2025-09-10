namespace CrossCutting.Data.Abstractions;

public interface IDatabaseEntityRetrieverSettingsProvider
{
    Result<IDatabaseEntityRetrieverSettings> Get<TSource>();
}
