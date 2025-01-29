namespace CrossCutting.Utilities.Parsers.ExpressionStrings;

public class MathematicExpressionString(IMathematicExpressionEvaluator evaluator) : IExpressionString
{
    private readonly IMathematicExpressionEvaluator _evaluator = evaluator;

    public Result<object?> Evaluate(ExpressionStringEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        // try =1+1 -> mathematic expression, no functions/formattable strings
        var mathResult = _evaluator.Evaluate(context.Input.Substring(1), context.Settings.FormatProvider, context.Context);
        if (mathResult.Status is ResultStatus.Ok or not ResultStatus.NotFound)
        {
            // both success and failure need to be returned.
            // not found can be ignored, we can try formattable string and function in that case
            return mathResult;
        }

        return Result.Continue<object?>();
    }

    public Result<Type> Validate(ExpressionStringEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        // try =1+1 -> mathematic expression, no functions/formattable strings
        var mathResult = _evaluator.Validate(context.Input.Substring(1), context.Settings.FormatProvider, context.Context);
        if (mathResult.Status is ResultStatus.Ok or not ResultStatus.NotFound)
        {
            // both success and failure need to be returned.
            // not found can be ignored, we can try formattable string and function in that case
            return mathResult;
        }

        return Result.Continue<Type>();
    }
}
