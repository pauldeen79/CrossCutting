﻿namespace CrossCutting.Data.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCrossCuttingDataCore(this IServiceCollection instance)
        => instance.Chain(x => x.TryAddSingleton<IDatabaseCommandProvider, SelectDatabaseCommandProvider>());
}
