namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors;

internal class MathematicExpressionProcessor : IExpressionStringParserProcessor
{
    public Result<object> Process(ExpressionStringParserState state)
    {
        // try =1+1 -> mathematic expression, no functions/formattable strings
        var mathResult = MathematicExpressionParser.Parse(state.Input.Substring(1), state.FormatProvider, state.ParseExpressionDelegate);
        if (mathResult.Status == ResultStatus.Ok || mathResult.Status != ResultStatus.NotFound)
        {
            // both success and failure need to be returned. not found can be ignored, we can try formattable string and function in that case
            return mathResult;
        }

        return Result<object>.Continue();
    }
}
