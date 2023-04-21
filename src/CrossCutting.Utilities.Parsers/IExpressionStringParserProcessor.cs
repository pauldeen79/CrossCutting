namespace CrossCutting.Utilities.Parsers;

internal interface IExpressionStringParserProcessor
{
    Result<object> Process(ExpressionStringParserState state);
}
