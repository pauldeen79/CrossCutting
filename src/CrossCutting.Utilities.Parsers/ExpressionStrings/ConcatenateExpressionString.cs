namespace CrossCutting.Utilities.Parsers.ExpressionStrings;

public class ConcatenateExpressionString : IExpressionString
{
    public Result<object?> Evaluate(ExpressionStringEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return BaseProcessor.SplitDelimited(context, '&', split =>
        {
            var builder = new StringBuilder();
            foreach (var item in split)
            {
                var result = context.Evaluator.Evaluate($"={item}", context.Settings, context.Context, context.FormattableStringParser);
                if (!result.IsSuccessful())
                {
                    return result;
                }

                builder.Append(result.Value.ToString(context.Settings.FormatProvider));
            }

            return Result.Success<object?>(builder.ToString());
        });
    }

    public Result<Type> Validate(ExpressionStringEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return BaseProcessor.SplitDelimited
        (
            context,
            '&',
            BaseProcessor.Parse(context)
        );
    }
}
