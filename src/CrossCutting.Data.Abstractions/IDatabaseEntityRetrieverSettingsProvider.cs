namespace CrossCutting.Data.Abstractions;

public interface IDatabaseEntityRetrieverSettingsProvider
{
    bool TryGet<TSource>(out IDatabaseEntityRetrieverSettings? settings);
}
