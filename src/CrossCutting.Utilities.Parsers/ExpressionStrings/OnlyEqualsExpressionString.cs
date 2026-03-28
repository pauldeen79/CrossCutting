namespace CrossCutting.Utilities.Parsers.ExpressionStrings;

public class OnlyEqualsExpressionString : IExpressionString
{
    public Result<object?> Evaluate(ExpressionStringEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        if (context.Input == "=")
        {
            return Result.Success<object?>(context.Input);
        }

        return Result.Continue<object?>();
    }

    public Result<Type> Validate(ExpressionStringEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        if (context.Input == "=")
        {
            return Result.Success(typeof(string));
        }

        return Result.Continue<Type>();
    }
}
