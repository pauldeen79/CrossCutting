namespace CrossCutting.Data.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCrossCuttingDataCore(this IServiceCollection instance)
        => instance.AddSingleton<IDatabaseCommandProvider, SelectDatabaseCommandProvider>();
}
