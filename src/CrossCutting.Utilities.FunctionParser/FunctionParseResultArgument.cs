namespace CrossCutting.Utilities.FunctionParser;

public abstract record FunctionParseResultArgument
{
}

public sealed record LiteralArgument : FunctionParseResultArgument
{
    public LiteralArgument(string value)
    {
        Value = value;
    }

    public string Value { get; }
}

public sealed record FunctionArgument : FunctionParseResultArgument
{
    public FunctionArgument(FunctionParseResult function)
    {
        Function = function;
    }

    public FunctionParseResult Function { get; }
}
