namespace CrossCutting.Utilities.QueryEvaluator.Abstractions;

public interface IQueryParser<T> where T : IQueryBuilder
{
    T Parse(T builder, string queryString);
}
