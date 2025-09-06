namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;

public interface IQueryWrapper
{
    IQuery Query { get; }
    object? Context { get; }
}
