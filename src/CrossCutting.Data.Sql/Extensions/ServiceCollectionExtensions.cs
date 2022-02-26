namespace CrossCutting.Data.Sql.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCrossCuttingDataSql(this IServiceCollection instance)
        => instance.AddSingleton<IPagedDatabaseCommandProvider, PagedSelectDatabaseCommandProvider>();
}
