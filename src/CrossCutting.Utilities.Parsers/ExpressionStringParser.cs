namespace CrossCutting.Utilities.Parsers;

public static class ExpressionStringParser
{
    public static Result<object> Parse(
        string input,
        Func<string, Result<object>> parseExpressionDelegate,
        Func<string, Result<string>> placeholderDelegate,
        Func<FunctionParseResult, Result<object>> parseFunctionDelegate)
    {
        if (string.IsNullOrEmpty(input))
        {
            return Result<object>.Success(string.Empty);
        }

        if (!input.StartsWith("="))
        {
            return Result<object>.Success(input);
        }

        if (input.Length == 1)
        {
            return Result<object>.Success(input);
        }

        if (input.StartsWith("=\"") && input.EndsWith("\""))
        {
            // ="string value" -> literal, no functions but formattable strings possible
            var formattedStringResult = FormattableStringParser.Parse(input.Substring(2, input.Length - 3), placeholderDelegate);
            return formattedStringResult.Status != ResultStatus.Ok
                ? Result<object>.FromExistingResult(formattedStringResult)
                : Result<object>.FromExistingResult(formattedStringResult, result => result);
        }

        // try =1+1 -> mathematic expresison, no functions/formattable strings
        var mathResult = MathematicExpressionParser.Parse(input.Substring(1), parseExpressionDelegate);
        if (mathResult.Status == ResultStatus.Ok || mathResult.Status != ResultStatus.NotFound)
        {
            // both success and failure need to be returned. not found can be ignored, we can try formattable string and function in that case
            return mathResult;
        }

        // =something else, we can try function
        var functionResult = FunctionParser.Parse(input.Substring(1));
        return functionResult.Status switch
        {
            ResultStatus.Ok => parseFunctionDelegate(functionResult.Value!),
            //ResultStatus.NotFound => Result<object>.Success(input),
            ResultStatus.NotSupported => Result<object>.FromExistingResult(functionResult),
            _ => Result<object>.Success(input)
        };
    }
}
