namespace CrossCutting.Utilities.Parsers.ExpressionStrings;

public class MathematicExpressionString(IMathematicExpressionEvaluator parser) : IExpressionString
{
    private readonly IMathematicExpressionEvaluator _parser = parser;

    public Result<object?> Evaluate(ExpressionStringEvaluatorState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        // try =1+1 -> mathematic expression, no functions/formattable strings
        var mathResult = _parser.Evaluate(state.Input.Substring(1), state.FormatProvider, state.Context);
        if (mathResult.Status is ResultStatus.Ok or not ResultStatus.NotFound)
        {
            // both success and failure need to be returned.
            // not found can be ignored, we can try formattable string and function in that case
            return mathResult;
        }

        return Result.Continue<object?>();
    }

    public Result<Type> Validate(ExpressionStringEvaluatorState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        // try =1+1 -> mathematic expression, no functions/formattable strings
        var mathResult = _parser.Validate(state.Input.Substring(1), state.FormatProvider, state.Context);
        if (mathResult.Status is ResultStatus.Ok or not ResultStatus.NotFound)
        {
            // both success and failure need to be returned.
            // not found can be ignored, we can try formattable string and function in that case
            return mathResult;
        }

        return Result.Continue<Type>();
    }
}
