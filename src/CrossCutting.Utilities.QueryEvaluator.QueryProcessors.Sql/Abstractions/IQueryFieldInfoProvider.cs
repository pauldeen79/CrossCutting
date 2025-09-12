namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;

public interface IQueryFieldInfoProvider
{
    Result<IQueryFieldInfo> Create(IQuery query);
}
