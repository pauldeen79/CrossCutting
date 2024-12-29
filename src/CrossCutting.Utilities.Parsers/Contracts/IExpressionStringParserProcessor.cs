namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IExpressionStringParserProcessor
{
    int Order { get; }

    Result Validate(ExpressionStringParserState state);

    Result<object?> Process(ExpressionStringParserState state);
}
