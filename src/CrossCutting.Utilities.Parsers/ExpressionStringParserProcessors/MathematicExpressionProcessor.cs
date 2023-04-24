namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors;

public class MathematicExpressionProcessor : IExpressionStringParserProcessor
{
    private readonly IMathematicExpressionParser _parser;

    public MathematicExpressionProcessor(IMathematicExpressionParser parser)
    {
        _parser = parser;
    }

    public int Order => 50;

    public Result<object?> Process(ExpressionStringParserState state)
    {
        // try =1+1 -> mathematic expression, no functions/formattable strings
        var mathResult = _parser.Parse(state.Input.Substring(1), state.FormatProvider);
        if (mathResult.Status == ResultStatus.Ok || mathResult.Status != ResultStatus.NotFound)
        {
            // both success and failure need to be returned. not found can be ignored, we can try formattable string and function in that case
            return mathResult;
        }

        return Result<object?>.Continue();
    }
}
