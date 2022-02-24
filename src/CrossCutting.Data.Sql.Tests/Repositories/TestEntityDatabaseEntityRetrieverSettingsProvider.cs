namespace CrossCutting.Data.Sql.Tests.Repositories;

public class TestEntityDatabaseEntityRetrieverSettingsProvider : IPagedDatabaseEntityRetrieverSettingsProvider, IDatabaseEntityRetrieverSettingsProvider
{
    public bool TryGet<TSource>(out IPagedDatabaseEntityRetrieverSettings? settings)
    {
        if (typeof(TSource) == typeof(TestEntityIdentity) || typeof(TSource) == typeof(TestEntity))
        {
            settings = new TestEntityDatabaseEntityRetrieverSettings();
            return true;
        }

        settings = null;
        return false;
    }

    public bool TryGet<TSource>(out IDatabaseEntityRetrieverSettings? settings)
    {
        if (typeof(TSource) == typeof(TestEntityIdentity) || typeof(TSource) == typeof(TestEntity))
        {
            settings = new TestEntityDatabaseEntityRetrieverSettings();
            return true;
        }

        settings = null;
        return false;
    }
}
