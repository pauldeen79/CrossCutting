namespace CrossCutting.Utilities.Parsers.ExpressionStrings;

public class PipedExpressionString : IExpressionString
{
    public Result<object?> Evaluate(ExpressionStringEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return BaseProcessor.SplitDelimited(context, '|', split =>
        {
            var resultValue = context.Context;
            foreach (var item in split)
            {
                var result = context.Evaluator.Evaluate($"={item}", context.Settings, resultValue, context.FormattableStringParser);
                if (!result.IsSuccessful())
                {
                    return result;
                }
                resultValue = result.Value;
            }

            return Result.Success(resultValue);
        });
    }

    public Result<Type> Validate(ExpressionStringEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return BaseProcessor.SplitDelimited
        (
            context,
            '|',
            BaseProcessor.Parse(context)
        );
    }
}
