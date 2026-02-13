using CrossCutting.Common.Results;
using CrossCutting.Data.Abstractions;
using DataFramework.ModelFramework.Poc.PagedDatabaseEntityRetrieverSettings;
using PDC.Net.Core.Queries;

namespace DataFramework.ModelFramework.Poc.PagedDatabaseEntityRetrieverSettingsProviders
{
    public class ExtraFieldPagedDatabaseEntityRetrieverSettingsProvider : IPagedDatabaseEntityRetrieverSettingsProvider
    {
        public Result<IPagedDatabaseEntityRetrieverSettings> Get<TSource>()
        {
            if (typeof(TSource) == typeof(ExtraFieldQuery))
            {
                return Result.Success<IPagedDatabaseEntityRetrieverSettings>(new ExtraFieldPagedEntityRetrieverSettings());
            }

            return Result.Continue<IPagedDatabaseEntityRetrieverSettings>();
        }
    }
}
