namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;

public interface IQueryFieldInfoFactory
{
    IQueryFieldInfo Create(IQuery query);
}
