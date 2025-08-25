namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;

public interface IDatabaseEntityRetrieverProvider
{
    Result<IDatabaseEntityRetriever<TResult>> Create<TResult>(IQuery query) where TResult : class;
}
