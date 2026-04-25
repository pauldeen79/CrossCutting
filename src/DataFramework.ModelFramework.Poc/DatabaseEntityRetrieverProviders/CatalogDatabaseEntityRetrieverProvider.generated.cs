using CrossCutting.Common.Results;
using CrossCutting.Data.Abstractions;
using CrossCutting.Utilities.QueryEvaluator.Abstractions;
using CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;
using PDC.Net.Core.Entities;

namespace DataFramework.ModelFramework.Poc.DatabaseEntityRetrieverProviders
{
    public class CatalogDatabaseEntityRetrieverProvider : IDatabaseEntityRetrieverProvider
    {
        private readonly IDatabaseEntityRetriever<Catalog> _databaseEntityRetriever;

        public CatalogDatabaseEntityRetrieverProvider(IDatabaseEntityRetriever<Catalog> databaseEntityRetriever)
            => _databaseEntityRetriever = databaseEntityRetriever;

        public Result<IDatabaseEntityRetriever<TResult>> Create<TResult>(object query) where TResult : class
        {
            if (typeof(TResult) == typeof(Catalog))
            {
                return Result.Success((IDatabaseEntityRetriever<TResult>)_databaseEntityRetriever);
            }

            return Result.Continue<IDatabaseEntityRetriever<TResult>>();
        }
    }
}
