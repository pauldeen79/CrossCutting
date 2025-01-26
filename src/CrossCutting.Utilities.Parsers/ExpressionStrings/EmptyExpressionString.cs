namespace CrossCutting.Utilities.Parsers.ExpressionStrings;

public class EmptyExpressionString : IExpressionString
{
    public Result<object?> Evaluate(ExpressionStringEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        if (string.IsNullOrEmpty(context.Input))
        {
            return Result.Success<object?>(string.Empty);
        }

        return Result.Continue<object?>();
    }

    public Result<Type> Validate(ExpressionStringEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        if (string.IsNullOrEmpty(context.Input))
        {
            return Result.Success(typeof(string));
        }

        return Result.Continue<Type>();
    }
}
