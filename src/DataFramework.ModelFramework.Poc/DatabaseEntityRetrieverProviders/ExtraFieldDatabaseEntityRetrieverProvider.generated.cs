using CrossCutting.Data.Abstractions;
using PDC.Net.Core.Entities;
using CrossCutting.Utilities.QueryEvaluator.Abstractions;
using CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;
using CrossCutting.Common.Results;

namespace DataFramework.ModelFramework.Poc.DatabaseEntityRetrieverProviders
{
    public class ExtraFieldDatabaseEntityRetrieverProvider : IDatabaseEntityRetrieverProvider
    {
        private readonly IDatabaseEntityRetriever<ExtraField> _databaseEntityRetriever;

        public ExtraFieldDatabaseEntityRetrieverProvider(IDatabaseEntityRetriever<ExtraField> databaseEntityRetriever)
            => _databaseEntityRetriever = databaseEntityRetriever;

        public Result<IDatabaseEntityRetriever<TResult>> Create<TResult>(IQuery query) where TResult : class
        {
            if (typeof(TResult) == typeof(ExtraField))
            {
                return Result.Success((IDatabaseEntityRetriever<TResult>)_databaseEntityRetriever);
            }

            return Result.Continue<IDatabaseEntityRetriever<TResult>>();
        }
    }
}
