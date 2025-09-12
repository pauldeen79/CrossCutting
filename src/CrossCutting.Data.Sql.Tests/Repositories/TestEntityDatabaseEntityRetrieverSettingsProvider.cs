namespace CrossCutting.Data.Sql.Tests.Repositories;

public class TestEntityDatabaseEntityRetrieverSettingsProvider : IPagedDatabaseEntityRetrieverSettingsProvider, IDatabaseEntityRetrieverSettingsProvider
{
    Result<IPagedDatabaseEntityRetrieverSettings> IPagedDatabaseEntityRetrieverSettingsProvider.Get<TSource>()
    {
        if (typeof(TSource) == typeof(TestEntityIdentity) || typeof(TSource) == typeof(TestEntity))
        {
            return Result.Success<IPagedDatabaseEntityRetrieverSettings>(new TestEntityDatabaseEntityRetrieverSettings());
        }

        return Result.Continue<IPagedDatabaseEntityRetrieverSettings>();
    }

    Result<IDatabaseEntityRetrieverSettings> IDatabaseEntityRetrieverSettingsProvider.Get<TSource>()
    {
        if (typeof(TSource) == typeof(TestEntityIdentity) || typeof(TSource) == typeof(TestEntity))
        {
            return Result.Success<IDatabaseEntityRetrieverSettings>(new TestEntityDatabaseEntityRetrieverSettings());
        }

        return Result.Continue<IDatabaseEntityRetrieverSettings>();
    }
}
