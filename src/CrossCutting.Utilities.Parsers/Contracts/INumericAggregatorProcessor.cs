namespace CrossCutting.Utilities.Parsers.Contracts;

public interface INumericAggregatorProcessor<T>
{
    Result<object?> Aggregate(object firstValue, object secondValue, Func<T, T, object?> aggregatorDelegate);
}
