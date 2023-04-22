namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IAggregator
{
    Result<object> Aggregate(object value1, object value2);
}
