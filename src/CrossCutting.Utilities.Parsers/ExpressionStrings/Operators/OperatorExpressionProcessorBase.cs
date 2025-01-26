namespace CrossCutting.Utilities.Parsers.ExpressionStrings.Operators;

public abstract class OperatorExpressionProcessorBase : IExpressionString
{
    protected abstract string Sign { get; }

    public Result<object?> Evaluate(ExpressionStringEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        if (context.Input.IndexOf(Sign) == -1)
        {
            return Result.Continue<object?>();
        }

        var split = context.Input.Substring(1).SplitDelimited(Sign, '\"', true, true);
        if (split.Length != 2)
        {
            return Result.Continue<object?>();
        }

        var leftResult = context.Parser.Evaluate($"={split[0]}", context.FormatProvider, context.Context, context.FormattableStringParser);
        if (!leftResult.IsSuccessful())
        {
            return leftResult;
        }

        var rightResult = context.Parser.Evaluate($"={split[1]}", context.FormatProvider, context.Context, context.FormattableStringParser);
        if (!rightResult.IsSuccessful())
        {
            return rightResult;
        }

        return Result.FromExistingResult<object?>(PerformOperator(leftResult.Value, rightResult.Value));
    }

    public Result<Type> Validate(ExpressionStringEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        if (context.Input.IndexOf(Sign) == -1)
        {
            return Result.Continue<Type>();
        }

        var split = context.Input.Substring(1).SplitDelimited(Sign, '\"', true, true);
        if (split.Length != 2)
        {
            return Result.Continue<Type>();
        }

        var leftResult = context.Parser.Validate($"={split[0]}", context.FormatProvider, context.Context, context.FormattableStringParser);
        if (!leftResult.IsSuccessful())
        {
            return leftResult;
        }

        var rightResult = context.Parser.Validate($"={split[1]}", context.FormatProvider, context.Context, context.FormattableStringParser);
        if (!rightResult.IsSuccessful())
        {
            return rightResult;
        }

        return Result.Success(typeof(bool));
    }

    protected abstract Result<bool> PerformOperator(object? leftValue, object? rightValue);
}
