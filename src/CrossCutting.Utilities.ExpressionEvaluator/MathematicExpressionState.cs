namespace CrossCutting.Utilities.ExpressionEvaluator;

public class MathematicExpressionState
{
    public string Remainder { get; set; }
    public ExpressionEvaluatorContext Context { get; }
    public ICollection<Result<object?>> Results { get; } = [];
    public IExpressionEvaluator ExpressionEvaluator { get; }

    public int Position { get; private set; }
    public IReadOnlyCollection<AggregatorInfo> Indexes { get; private set; }
    public IReadOnlyCollection<int> PreviousIndexes { get; private set; }
    public string LeftPart { get; private set; }
    public Result<object?> LeftPartResult { get; private set; }
    public Result LeftPartValidationResult { get; private set; }
    public IReadOnlyCollection<int> NextIndexes { get; private set; }
    public string RightPart { get; private set; }
    public Result<object?> RightPartResult { get; private set; }
    public Result RightPartValidationResult { get; private set; }

    public MathematicExpressionState(
        ExpressionEvaluatorContext context,
        IExpressionEvaluator expressionEvaluator,
        Func<ExpressionEvaluatorContext, Result<object?>> parseDelegate)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        ArgumentGuard.IsNotNull(parseDelegate, nameof(parseDelegate));
        ArgumentGuard.IsNotNull(expressionEvaluator, nameof(expressionEvaluator));

        Context = context;
        Remainder = context.Expression;
        ExpressionEvaluator = expressionEvaluator;

        Position = -1;
        Indexes = [];
        PreviousIndexes = [];
        LeftPart = string.Empty;
        LeftPartResult = Result.NoContent<object?>();
        LeftPartValidationResult = Result.NoContent();
        NextIndexes = [];
        RightPart = string.Empty;
        RightPartResult = Result.NoContent<object?>();
        RightPartValidationResult = Result.NoContent();
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

    internal void SetPreviousIndexes(int[] aggregatorPositions, IExpressionEvaluator expressionEvaluator)
    {
        PreviousIndexes = aggregatorPositions;
        LeftPart = GetLeftPart();
        LeftPartResult = GetPartResult(LeftPart, expressionEvaluator);
    }

    internal void SetNextIndexes(int[] aggregatorPositions, IExpressionEvaluator expressionEvaluator)
    {
        NextIndexes = aggregatorPositions;
        RightPart = GetRightPart();
        RightPartResult = GetPartResult(RightPart, expressionEvaluator);
    }

    internal Result<object?> PerformAggregation()
    {
        var aggregateResult = Indexes.First().Aggregator.Aggregate(LeftPartResult.Value!, RightPartResult.Value!, Context.Settings.FormatProvider);

        if (aggregateResult.IsSuccessful())
        {
            AddResult(aggregateResult);
        }

        return aggregateResult;
    }

    private void AddResult(Result<object?> aggregateResult)
    {
        Remainder = string.Concat
        (
            Remainder.Substring
            (
                0,
                PreviousIndexes.Count > 0
                    ? PreviousIndexes.First() + 1
                    : 0
            ),
            FormattableString.Invariant($"{MathematicExpressionEvaluator.TemporaryDelimiter}{Results.Count}{MathematicExpressionEvaluator.TemporaryDelimiter}"),
                NextIndexes.Count > 0
                    ? Remainder.Substring(NextIndexes.First())
                    : string.Empty
        );
        Results.Add(aggregateResult);
    }

    private string GetLeftPart()
        => PreviousIndexes.Count > 0
            ? Remainder.Substring(PreviousIndexes.First() + 1, Position - PreviousIndexes.First() - 1).Trim()
            : Remainder.Substring(0, Position).Trim();

    private string GetRightPart()
        => NextIndexes.Count > 0
            ? Remainder.Substring(Position + 1, NextIndexes.First() - Position - 1).Trim()
            : Remainder.Substring(Position + 1).Trim();

    private Result<object?> GetPartResult(string part, IExpressionEvaluator expressionEvaluator)
        => part.StartsWith(MathematicExpressionEvaluator.TemporaryDelimiter) && part.EndsWith(MathematicExpressionEvaluator.TemporaryDelimiter)
            ? Results.ElementAt(int.Parse(part.Substring(MathematicExpressionEvaluator.TemporaryDelimiter.Length, part.Length - (MathematicExpressionEvaluator.TemporaryDelimiter.Length * 2)), Context.Settings.FormatProvider))
            : expressionEvaluator.Evaluate(Context); //TODO: create sub context, add part
}
