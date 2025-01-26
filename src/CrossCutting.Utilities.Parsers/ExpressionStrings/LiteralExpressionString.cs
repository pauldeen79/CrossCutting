namespace CrossCutting.Utilities.Parsers.ExpressionStrings;

public class LiteralExpressionString : IExpressionString
{
    public Result<object?> Evaluate(ExpressionStringEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        if (context.Input.StartsWith("\'="))
        {
            // escaped expression string
            return Result.Success<object?>(context.Input.Substring(1));
        }

        if (!context.Input.StartsWith("="))
        {
            // literal
            return Result.Success<object?>(context.Input);
        }

        return Result.Continue<object?>();
    }

    public Result<Type> Validate(ExpressionStringEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        if (context.Input.StartsWith("\'="))
        {
            // escaped expression string
            return Result.Success(typeof(string));
        }

        if (!context.Input.StartsWith("="))
        {
            // literal
            return Result.Success(typeof(string));
        }

        return Result.Continue<Type>();
    }
}
