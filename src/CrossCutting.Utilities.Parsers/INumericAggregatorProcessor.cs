namespace CrossCutting.Utilities.Parsers;

public interface INumericAggregatorProcessor<T>
{
    Result<object> Aggregate(object firstValue, object secondValue, Func<T, T, object> aggregatorDelegate);
}
