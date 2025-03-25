namespace CrossCutting.Utilities.ExpressionEvaluator.Builders;

public partial class ExpressionParseResultBuilder
{
    public void AddPartResult(Result<ExpressionParseResult> itemResult, string prefix, int? counter = null, string? suffix = null)
    {
        itemResult = ArgumentGuard.IsNotNull(itemResult, nameof(itemResult));
        prefix = ArgumentGuard.IsNotNullOrEmpty(prefix, nameof(prefix));

        if (!string.IsNullOrEmpty(suffix))
        {
            suffix = "." + suffix;
        }

        var counterString = counter.HasValue
            ? $".{counter}"
            : string.Empty;

        AddPartResults(new ExpressionParsePartResultBuilder()
            .WithPartName($"{prefix}{counterString}{suffix}")
            .WithResult(itemResult)
            .WithExpressionType(itemResult.Value?.ExpressionType)
            .WithResultType(itemResult.Value?.ResultType)
            .AddPartResults(itemResult.Value?.PartResults.Select(x => x.ToBuilder()) ?? Enumerable.Empty<ExpressionParsePartResultBuilder>())
            .WithSourceExpression(itemResult.Value?.SourceExpression)
            );
    }

    public Result<ExpressionParseResult> CreateParseResult()
        => PartResults.Any(x => !x.Result.IsSuccessful())
            ? Result.Invalid<ExpressionParseResult>("Parsing of the expression failed, see inner results for details", PartResults.Select(x => x.Result))
            : Result.Success(Build());
}
