namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors;

public class FormattableStringExpressionProcessor : IExpressionStringParserProcessor
{
    private readonly IFormattableStringParser _parser;

    public FormattableStringExpressionProcessor(IFormattableStringParser parser)
    {
        _parser = parser;
    }

    public int Order => 40;

    public Result<object?> Process(ExpressionStringParserState state)
    {
        if (state.Input.StartsWith("=@\"") && state.Input.EndsWith("\""))
        {
            // =@"string value" -> literal, no functions but formattable strings possible
            var formattedStringResult = _parser.Parse(state.Input.Substring(3, state.Input.Length - 4));
            return formattedStringResult.Status != ResultStatus.Ok
                ? Result<object?>.FromExistingResult(formattedStringResult)
                : Result<object?>.FromExistingResult(formattedStringResult, result => result);
        }
        else if (state.Input.StartsWith("=\"") && state.Input.EndsWith("\""))
        {
            // ="string value" -> literal, no functions and no formattable strings possible
            return Result<object?>.Success(state.Input.Substring(2, state.Input.Length - 3));
        }

        return Result<object?>.Continue();
    }
}
