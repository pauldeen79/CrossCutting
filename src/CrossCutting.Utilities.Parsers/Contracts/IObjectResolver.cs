namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IObjectResolver
{
    Result<T> Resolve<T>(object? sourceObject);
}
