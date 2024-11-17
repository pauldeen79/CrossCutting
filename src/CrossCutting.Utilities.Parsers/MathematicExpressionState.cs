namespace CrossCutting.Utilities.Parsers;

public class MathematicExpressionState
{
    public string Input { get; }
    public string Remainder { get; set; }
    public IFormatProvider FormatProvider { get; }
    public object? Context { get; }
    public ICollection<Result<object?>> Results { get; } = [];
    public Func<string, IFormatProvider, object?, Result<object?>> ParseDelegate { get; }

    public int Position { get; private set; }
    public IReadOnlyCollection<AggregatorInfo> Indexes { get; private set; }
    public IReadOnlyCollection<int> PreviousIndexes { get; private set; }
    public string LeftPart { get; private set; }
    public Result<object?> LeftPartResult { get; private set; }
    public IReadOnlyCollection<int> NextIndexes { get; private set; }
    public string RightPart { get; private set; }
    public Result<object?> RightPartResult { get; private set; }

    public MathematicExpressionState(
        string input,
        IFormatProvider formatProvider,
        object? context,
        Func<string, IFormatProvider, object?, Result<object?>> parseDelegate)
    {
        ArgumentGuard.IsNotNull(input, nameof(input));
        ArgumentGuard.IsNotNull(formatProvider, nameof(formatProvider));
        ArgumentGuard.IsNotNull(parseDelegate, nameof(parseDelegate));

        Input = input;
        FormatProvider = formatProvider;
        Context = context;
        Remainder = input;
        ParseDelegate = parseDelegate;

        Position = -1;
        Indexes = [];
        PreviousIndexes = [];
        LeftPart = string.Empty;
        LeftPartResult = Result.NoContent<object?>();
        NextIndexes = [];
        RightPart = string.Empty;
        RightPartResult = Result.NoContent<object?>();
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

    internal void SetPreviousIndexes(int[] aggregatorPositions, IExpressionParser expressionParser)
    {
        PreviousIndexes = aggregatorPositions;
        LeftPart = GetLeftPart();
        LeftPartResult = GetPartResult(LeftPart, expressionParser);
    }

    internal void SetNextIndexes(int[] aggregatorPositions, IExpressionParser expressionParser)
    {
        NextIndexes = aggregatorPositions;
        RightPart = GetRightPart();
        RightPartResult = GetPartResult(RightPart, expressionParser);
    }

    internal Result<object?> PerformAggregation()
    {
        var aggregateResult = Indexes.First().Aggregator.Aggregate(LeftPartResult.Value!, RightPartResult.Value!, FormatProvider);

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
            FormattableString.Invariant($"{MathematicExpressionParser.TemporaryDelimiter}{Results.Count}{MathematicExpressionParser.TemporaryDelimiter}"),
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

    private Result<object?> GetPartResult(string part, IExpressionParser expressionParser)
        => part.StartsWith(MathematicExpressionParser.TemporaryDelimiter) && part.EndsWith(MathematicExpressionParser.TemporaryDelimiter)
            ? Results.ElementAt(int.Parse(part.Substring(MathematicExpressionParser.TemporaryDelimiter.Length, part.Length - (MathematicExpressionParser.TemporaryDelimiter.Length * 2)), CultureInfo.InvariantCulture))
            : expressionParser.Parse(part, FormatProvider, Context);
}
