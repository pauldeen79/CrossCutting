namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;

public interface IQueryContext : IContextContainer
{
    IQuery Query { get; }
}

public interface IContextContainer
{
    object? Context { get; }
}