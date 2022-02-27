namespace CrossCutting.Data.Sql.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCrossCuttingDataSql(this IServiceCollection instance)
        => instance.AddCrossCuttingDataCore()
                   .Chain(x => x.TryAddSingleton<IPagedDatabaseCommandProvider, PagedSelectDatabaseCommandProvider>());
}
