namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;

public class IndexerExpressionComponent : IExpressionComponent
{
    private static readonly Regex _indexerRegex = new Regex(@"(?<Expression>[^\[\]]+)\[(?<Index>\d+)\]", RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
    private const string Index = nameof(Index);
    private const string Expression = nameof(Expression);

    public int Order => 31;

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var match = _indexerRegex.Match(context.Expression);
        if (!match.Success)
        {
            return Result.Continue<object?>();
        }

        return new ResultDictionaryBuilder<object?>()
            .Add(Expression, () => context.EvaluateTyped<IEnumerable>(match.Groups[Expression].Value))
            .Add(Index, () => context.EvaluateTyped<int>(match.Groups[Index].Value))
            .Build()
            .OnSuccess(results => Result.Success(results[Expression].CastValueAs<IEnumerable>().OfType<object?>().ElementAtOrDefault(results[Index].CastValueAs<int>())));
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
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

        var parseResult = new ResultDictionaryBuilder<Type>()
            .Add(Expression, () => context.Parse(match.Groups[Expression].Value))
            .Add(Index, () => context.Parse(match.Groups[Index].Value))
            .Build()
            .OnFailure(innerResult => result.FillFromResult(innerResult))
            .OnSuccess(results => results[Expression]);

        return result.WithResultType(parseResult.Value?.GetElementType());
    }
}
