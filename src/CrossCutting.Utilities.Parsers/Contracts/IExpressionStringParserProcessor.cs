namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IExpressionStringParserProcessor
{
    int Order { get; }
    Result<object> Process(ExpressionStringParserState state);
}
