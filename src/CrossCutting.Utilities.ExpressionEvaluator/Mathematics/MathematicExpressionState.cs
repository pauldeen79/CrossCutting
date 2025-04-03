namespace CrossCutting.Utilities.ExpressionEvaluator.Mathematics;

public class MathematicExpressionState
{
    public string Remainder { get; set; }
    public ExpressionEvaluatorContext Context { get; }
    public ICollection<Result<object?>> Results { get; } = [];
    public ICollection<ExpressionParseResult> ParseResults { get; } = [];

    public int Position { get; private set; }
    public IReadOnlyCollection<AggregatorInfo> Indexes { get; private set; }
    public IReadOnlyCollection<int> PreviousIndexes { get; private set; }
    public string LeftPart { get; private set; }
    public Result<object?> LeftPartResult { get; private set; }
    public ExpressionParseResult LeftPartValidationResult { get; private set; }
    public IReadOnlyCollection<int> NextIndexes { get; private set; }
    public string RightPart { get; private set; }
    public Result<object?> RightPartResult { get; private set; }
    public ExpressionParseResult RightPartValidationResult { get; private set; }

    public MathematicExpressionState(
        ExpressionEvaluatorContext context,
        Func<ExpressionEvaluatorContext, Result<object?>> parseDelegate)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        ArgumentGuard.IsNotNull(parseDelegate, nameof(parseDelegate));

        Context = context;
        Remainder = context.Expression;

        Position = -1;
        Indexes = [];
        PreviousIndexes = [];
        LeftPart = string.Empty;
        LeftPartResult = Result.NoContent<object?>();
        LeftPartValidationResult = new ExpressionParseResultBuilder().WithExpressionType(GetType()).WithStatus(ResultStatus.NoContent);
        NextIndexes = [];
        RightPart = string.Empty;
        RightPartResult = Result.NoContent<object?>();
        RightPartValidationResult = new ExpressionParseResultBuilder().WithExpressionType(GetType()).WithStatus(ResultStatus.NoContent);
    }

    internal void SetPosition(IGrouping<int, AggregatorBase> aggregators)
    {
        Indexes = [.. aggregators
            .Select(x => new AggregatorInfo(x, Remainder.IndexOf(x.Character)))
            .Where(x => x.Index > -1)
            .OrderBy(x => x.Index)];
        Position = Indexes.Count > 0
            ? Indexes.First().Index
            : -1;
    }

    internal void SetPreviousIndexes(int[] aggregatorPositions, bool validateOnly)
    {
        PreviousIndexes = aggregatorPositions;
        LeftPart = GetLeftPart();
        if (validateOnly)
        {
            LeftPartValidationResult = GetPartParseResult(LeftPart);
        }
        else
        {
            LeftPartResult = GetPartEvaluationResult(LeftPart);
        }
    }

    internal void SetNextIndexes(int[] aggregatorPositions, bool validateOnly)
    {
        NextIndexes = aggregatorPositions;
        RightPart = GetRightPart();
        if (validateOnly)
        {
            RightPartValidationResult = GetPartParseResult(RightPart);
        }
        else
        {
            RightPartResult = GetPartEvaluationResult(RightPart);
        }
    }

    internal Result<object?> PerformAggregation(bool validateOnly, Result<Type> validationResult)
    {
        var aggregateResult = validateOnly
            ? validationResult.Transform<object?>(x => x)
            : Indexes.First().Aggregator.Aggregate(LeftPartResult.Value!, RightPartResult.Value!, Context.Settings.FormatProvider);

        if (aggregateResult.IsSuccessful())
        {
            var count = validateOnly
                ? ParseResults.Count
                : Results.Count;

            Remainder = string.Concat
            (
                Remainder.Substring
                (
                    0,
                    PreviousIndexes.Count > 0
                        ? PreviousIndexes.First() + 1
                        : 0
                ),
                FormattableString.Invariant($"{MathematicExpression.TemporaryDelimiter}{count}{MathematicExpression.TemporaryDelimiter}"),
                NextIndexes.Count > 0
                    ? Remainder.Substring(NextIndexes.First())
                    : string.Empty
            );

            if (!validateOnly)
            {
                Results.Add(aggregateResult);
            }
            else
            {
                ParseResults.Add(new ExpressionParseResultBuilder().WithExpressionType(GetType()).WithResultType(LeftPartValidationResult.ResultType));
            }
        }

        return aggregateResult;
    }

    private string GetLeftPart()
        => PreviousIndexes.Count > 0
            ? Remainder.Substring(PreviousIndexes.First() + 1, Position - PreviousIndexes.First() - 1).Trim()
            : Remainder.Substring(0, Position).Trim();

    private string GetRightPart()
        => NextIndexes.Count > 0
            ? Remainder.Substring(Position + 1, NextIndexes.First() - Position - 1).Trim()
            : Remainder.Substring(Position + 1).Trim();

    private Result<object?> GetPartEvaluationResult(string part)
        => part.StartsWith(MathematicExpression.TemporaryDelimiter) && part.EndsWith(MathematicExpression.TemporaryDelimiter)
            ? Results.ElementAt(int.Parse(part.Substring(MathematicExpression.TemporaryDelimiter.Length, part.Length - (MathematicExpression.TemporaryDelimiter.Length * 2)), Context.Settings.FormatProvider))
            : Context.Evaluate(part);

    private ExpressionParseResult GetPartParseResult(string part)
        => part.StartsWith(MathematicExpression.TemporaryDelimiter) && part.EndsWith(MathematicExpression.TemporaryDelimiter)
            ? ParseResults.ElementAt(int.Parse(part.Substring(MathematicExpression.TemporaryDelimiter.Length, part.Length - (MathematicExpression.TemporaryDelimiter.Length * 2)), Context.Settings.FormatProvider))
            : Context.Parse(part);
}
