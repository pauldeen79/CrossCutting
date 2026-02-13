using System.CodeDom.Compiler;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Sql;
using CrossCutting.Data.Sql.Extensions;
using CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;
using CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Extensions;
using DataFramework.ModelFramework.Poc.DatabaseCommandEntityProviders;
using DataFramework.ModelFramework.Poc.DatabaseCommandProviders;
using DataFramework.ModelFramework.Poc.DatabaseEntityRetrieverProviders;
using DataFramework.ModelFramework.Poc.DatabaseEntityRetrieverSettingsProviders;
using DataFramework.ModelFramework.Poc.EntityMappers;
using DataFramework.ModelFramework.Poc.PagedDatabaseEntityRetrieverSettingsProviders;
using DataFramework.ModelFramework.Poc.QueryFieldInfoProviders;
using DataFramework.ModelFramework.Poc.Repositories;
using Microsoft.Extensions.DependencyInjection;
using PDC.Net.Core.Entities;

namespace DataFramework.ModelFramework.Poc.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        [GeneratedCode(@"DataFramework.ModelFramework.Generators.Repositories.RepositoryGenerator", @"1.0.0.0")]
        public static IServiceCollection AddPdcNet(this IServiceCollection instance)
        {
            return instance
                //findall/findallpaged:
                .AddSingleton<IDatabaseEntityRetriever<Catalog>, DatabaseEntityRetriever<Catalog>>()

                //add/update/delete:
                .AddScoped<IDatabaseCommandProcessor<Catalog>, DatabaseCommandProcessor<Catalog, CatalogBuilder>>()
                .AddScoped<IDatabaseCommandEntityProvider<Catalog, CatalogBuilder>, CatalogDatabaseCommandEntityProvider>()
                .AddSingleton<IDatabaseCommandProvider<Catalog>, CatalogCommandProvider>()

                //find:
                .AddSingleton<IDatabaseCommandProvider<CatalogIdentity>, CatalogIdentityCommandProvider>()

                //query:
                .AddSingleton<IQueryFieldInfoProvider, CatalogQueryFieldInfoProvider>()
                .AddSingleton<IDatabaseEntityRetrieverProvider, CatalogDatabaseEntityRetrieverProvider>()
                .AddSingleton<IDatabaseEntityRetrieverSettingsProvider, CatalogDatabaseEntityRetrieverSettingsProvider>()
                .AddSingleton<IPagedDatabaseEntityRetrieverSettingsProvider, CatalogPagedDatabaseEntityRetrieverSettingsProvider>()

                //find/query:
                .AddSingleton<IDatabaseEntityMapper<Catalog>, CatalogEntityMapper>()

                //repository:
                .AddScoped<ICatalogRepository, CatalogRepository>()

                //findall/findallpaged:
                .AddSingleton<IDatabaseEntityRetriever<ExtraField>, DatabaseEntityRetriever<ExtraField>>()

                //add/update/delete:
                .AddScoped<IDatabaseCommandProcessor<ExtraField>, DatabaseCommandProcessor<ExtraField, ExtraFieldBuilder>>()
                .AddScoped<IDatabaseCommandEntityProvider<ExtraField, ExtraFieldBuilder>, ExtraFieldDatabaseCommandEntityProvider>()
                .AddSingleton<IDatabaseCommandProvider<ExtraField>, ExtraFieldCommandProvider>()

                //find:
                .AddSingleton<IDatabaseCommandProvider<ExtraFieldIdentity>, ExtraFieldIdentityCommandProvider>()

                //query:
                .AddSingleton<IQueryFieldInfoProvider, ExtraFieldQueryFieldInfoProvider>()
                .AddSingleton<IDatabaseEntityRetrieverProvider, ExtraFieldDatabaseEntityRetrieverProvider>()
                .AddSingleton<IDatabaseEntityRetrieverSettingsProvider, ExtraFieldDatabaseEntityRetrieverSettingsProvider>()
                .AddSingleton<IPagedDatabaseEntityRetrieverSettingsProvider, ExtraFieldPagedDatabaseEntityRetrieverSettingsProvider>()

                //find/query:
                .AddSingleton<IDatabaseEntityMapper<ExtraField>, ExtraFieldEntityMapper>()

                //repository:
                .AddScoped<IExtraFieldRepository, ExtraFieldRepository>();
        }
    }
}
