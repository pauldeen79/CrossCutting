namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public static class Expression
{
    public static void AddPartResult(ExpressionParseResultBuilder result, Result<ExpressionParseResult> itemResult, string prefix, int? counter = null, string? suffix = null)
    {
        result = ArgumentGuard.IsNotNull(result, nameof(result));
        itemResult = ArgumentGuard.IsNotNull(itemResult, nameof(itemResult));
        prefix = ArgumentGuard.IsNotNullOrEmpty(prefix, nameof(prefix));

        if (!string.IsNullOrEmpty(suffix))
        {
            suffix = "." + suffix;
        }

        var counterString = counter.HasValue
            ? $".{counter}"
            : string.Empty;

        result.AddPartResults(new ExpressionParsePartResultBuilder()
            .WithPartName($"{prefix}{counterString}{suffix}")
            .WithResult(itemResult)
            .WithExpressionType(itemResult.Value?.ExpressionType)
            .WithResultType(itemResult.Value?.ResultType)
            .AddPartResults(itemResult.Value?.PartResults.Select(x => x.ToBuilder()) ?? Enumerable.Empty<ExpressionParsePartResultBuilder>())
            .WithSourceExpression(itemResult.Value?.SourceExpression)
            );
    }
}
