namespace CrossCutting.Utilities.Parsers;

public class FunctionNameAndTypeArguments
{
    public FunctionNameAndTypeArguments(string rawResult, string name, string[] typeArguments)
    {
        ArgumentGuard.IsNotNull(rawResult, nameof(rawResult));
        ArgumentGuard.IsNotNull(name, nameof(name));
        ArgumentGuard.IsNotNull(typeArguments, nameof(typeArguments));

        RawResult = rawResult;
        Name = name;
        TypeArguments = typeArguments;
    }

    public string RawResult { get; }
    public string Name { get; }
    public IReadOnlyCollection<string> TypeArguments { get; }
}
