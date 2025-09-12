namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;

public interface IQueryContext
{
    IQuery Query { get; }
    object? Context { get; }
}
