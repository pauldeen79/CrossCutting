namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors;

public class MathematicExpressionProcessor(IMathematicExpressionParser parser) : IExpressionStringParserProcessor
{
    private readonly IMathematicExpressionParser _parser = parser;

    public int Order => 500;

    public Result<object?> Process(ExpressionStringParserState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        // try =1+1 -> mathematic expression, no functions/formattable strings
        var mathResult = _parser.Parse(state.Input.Substring(1), state.FormatProvider, state.Context);
        if (mathResult.Status is ResultStatus.Ok or not ResultStatus.NotFound)
        {
            // both success and failure need to be returned.
            // not found can be ignored, we can try formattable string and function in that case
            return mathResult;
        }

        return Result.Continue<object?>();
    }

    public Result Validate(ExpressionStringParserState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        // try =1+1 -> mathematic expression, no functions/formattable strings
        var mathResult = _parser.Parse(state.Input.Substring(1), state.FormatProvider, state.Context);
        if (mathResult.Status is ResultStatus.Ok or not ResultStatus.NotFound)
        {
            // both success and failure need to be returned.
            // not found can be ignored, we can try formattable string and function in that case
            return mathResult;
        }

        return Result.Continue();
    }
}
