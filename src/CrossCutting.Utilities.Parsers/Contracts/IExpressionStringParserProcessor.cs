namespace CrossCutting.Utilities.Parsers.Contracts;

internal interface IExpressionStringParserProcessor
{
    Result<object> Process(ExpressionStringParserState state);
}
