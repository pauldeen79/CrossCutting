using CrossCutting.Data.Abstractions;
using PDC.Net.Core.Entities;
using CrossCutting.Utilities.QueryEvaluator.Abstractions;
using CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;
using CrossCutting.Common.Results;

namespace DataFramework.ModelFramework.Poc.DatabaseEntityRetrieverProviders
{
    public class CatalogDatabaseEntityRetrieverProvider : IDatabaseEntityRetrieverProvider
    {
        private readonly IDatabaseEntityRetriever<Catalog> _databaseEntityRetriever;

        public CatalogDatabaseEntityRetrieverProvider(IDatabaseEntityRetriever<Catalog> databaseEntityRetriever)
            => _databaseEntityRetriever = databaseEntityRetriever;

        public Result<IDatabaseEntityRetriever<TResult>> Create<TResult>(IQuery query) where TResult : class
        {
            if (typeof(TResult) == typeof(Catalog))
            {
                return Result.Success((IDatabaseEntityRetriever<TResult>)_databaseEntityRetriever);
            }

            return Result.Continue<IDatabaseEntityRetriever<TResult>>();
        }
    }
}
