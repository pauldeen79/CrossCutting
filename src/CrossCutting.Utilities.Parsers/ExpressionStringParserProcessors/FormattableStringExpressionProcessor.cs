namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors;

internal class FormattableStringExpressionProcessor : IExpressionStringParserProcessor
{
    public Result<object> Process(ExpressionStringParserState state)
    {
        if (state.Input.StartsWith("=\"") && state.Input.EndsWith("\""))
        {
            // ="string value" -> literal, no functions but formattable strings possible
            var formattedStringResult = new FormattableStringParser(state.PlaceholderProcessor).Parse(state.Input.Substring(2, state.Input.Length - 3));
            return formattedStringResult.Status != ResultStatus.Ok
                ? Result<object>.FromExistingResult(formattedStringResult)
                : Result<object>.FromExistingResult(formattedStringResult, result => result);
        }

        return Result<object>.Continue();
    }
}
