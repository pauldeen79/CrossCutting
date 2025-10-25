namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;

public class IndexerExpressionComponent : IExpressionComponent
{
    private static readonly Regex _indexerRegex = new Regex(@"(?<Expression>[^\[\]]+)\[(?<Index>\d+)\]", RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
    private const string Index = nameof(Index);

    public int Order => 60;

    public async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var match = _indexerRegex.Match(context.Expression);
        if (!match.Success)
        {
            return Result.Continue<object?>();
        }

        var results = await new AsyncResultDictionaryBuilder()
            .Add(Constants.Expression, () => context.EvaluateTypedAsync<IEnumerable>(match.Groups[Constants.Expression].Value, token))
            .Add(Index, () => context.EvaluateTypedAsync<int>(match.Groups[Index].Value, token))
            .BuildAsync()
            .ConfigureAwait(false);

        return Result.Success(results.GetValue<IEnumerable>(Constants.Expression).OfType<object?>().ElementAtOrDefault(results.GetValue<int>(Index)));
    }

    public async Task<ExpressionParseResult> ParseAsync(ExpressionEvaluatorContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var result = new ExpressionParseResultBuilder()
            .WithStatus(ResultStatus.Ok)
            .WithExpressionComponentType(typeof(DotExpressionComponent))
            .WithSourceExpression(context.Expression);

        var match = _indexerRegex.Match(context.Expression);
        if (!match.Success)
        {
            return result.WithStatus(ResultStatus.Continue);
        }

        var expressionResult = await context.ParseAsync(match.Groups[Constants.Expression].Value, token).ConfigureAwait(false);
        var indexResult = await context.ParseAsync(match.Groups[Index].Value, token).ConfigureAwait(false);

        var parseResult = new ResultDictionaryBuilder()
            .Add(Constants.Expression, () => expressionResult)
            .Add(Index, () => indexResult)
            .Build();

        var error = parseResult.GetError();
        if (error is not null)
        {
            result.FillFromResult(error);
        }
        else
        {
            result.WithResultType(expressionResult.ResultType!.GetElementType());
        }

        return result;
    }
}
