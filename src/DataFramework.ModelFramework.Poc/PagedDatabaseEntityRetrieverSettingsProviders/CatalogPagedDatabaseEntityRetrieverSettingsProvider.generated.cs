using CrossCutting.Common.Results;
using CrossCutting.Data.Abstractions;
using DataFramework.ModelFramework.Poc.PagedDatabaseEntityRetrieverSettings;
using PDC.Net.Core.Entities;
using PDC.Net.Core.Queries;

namespace DataFramework.ModelFramework.Poc.PagedDatabaseEntityRetrieverSettingsProviders
{
    public class CatalogPagedDatabaseEntityRetrieverSettingsProvider : IPagedDatabaseEntityRetrieverSettingsProvider
    {
        public Result<IPagedDatabaseEntityRetrieverSettings> Get<TSource>()
        {
            if (typeof(TSource) == typeof(CatalogIdentity) || typeof(TSource) == typeof(CatalogQuery))
            {
                return Result.Success<IPagedDatabaseEntityRetrieverSettings>(new CatalogPagedEntityRetrieverSettings());
            }

            return Result.Continue<IPagedDatabaseEntityRetrieverSettings>();
        }
    }
}
