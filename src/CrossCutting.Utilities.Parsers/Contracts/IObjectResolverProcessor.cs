namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IObjectResolverProcessor
{
    Result<T> Resolve<T>(object? sourceObject);
}
