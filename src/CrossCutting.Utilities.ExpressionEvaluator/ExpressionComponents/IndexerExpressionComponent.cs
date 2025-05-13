namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;

public class IndexerExpressionComponent : IExpressionComponent
{
    private static readonly Regex _indexerRegex = new Regex(@"(?<Expression>[^\[\]]+)\[(?<Index>\d+)\]", RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
    private const string Index = nameof(Index);
    private const string Expression = nameof(Expression);

    public int Order => 60;

    public async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var match = _indexerRegex.Match(context.Expression);
        if (!match.Success)
        {
            return Result.Continue<object?>();
        }

        var results = await new AsyncResultDictionaryBuilder()
            .Add(Expression, context.EvaluateTypedAsync<IEnumerable>(match.Groups[Expression].Value))
            .Add(Index, context.EvaluateTypedAsync<int>(match.Groups[Index].Value))
            .Build()
            .ConfigureAwait(false);

        return Result.Success(results.GetValue<IEnumerable>(Expression).OfType<object?>().ElementAtOrDefault(results.GetValue<int>(Index)));
    }

    public async Task<ExpressionParseResult> ParseAsync(ExpressionEvaluatorContext context)
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

        var expressionResult = await context.ParseAsync(match.Groups[Expression].Value).ConfigureAwait(false);
        var indexResult = await context.ParseAsync(match.Groups[Index].Value).ConfigureAwait(false);

        var parseResult = new ResultDictionaryBuilder()
            .Add(Expression, () => expressionResult)
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
