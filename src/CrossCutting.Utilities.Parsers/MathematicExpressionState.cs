namespace CrossCutting.Utilities.Parsers;

internal class MathematicExpressionState
{
    internal string Input { get; }
    internal string Remainder { get; set; }
    internal IFormatProvider FormatProvider { get; }
    internal List<Result<object>> Results { get; } = new();
    internal IExpressionParser ExpressionParser { get; }
    internal Func<string, IFormatProvider, Result<object>> ParseDelegate { get; }

    internal int Position { get; private set; }
    internal AggregatorInfo[] Indexes { get; private set; }
    internal int[] PreviousIndexes { get; private set; }
    internal string LeftPart { get; private set; }
    internal Result<object> LeftPartResult { get; private set; }
    internal int[] NextIndexes { get; private set; }
    internal string RightPart { get; private set; }
    internal Result<object> RightPartResult { get; private set; }

    internal MathematicExpressionState(
        string input,
        IFormatProvider formatProvider,
        IExpressionParser expressionParser,
        Func<string, IFormatProvider, Result<object>> parseDelegate)
    {
        Input = input;
        FormatProvider = formatProvider;
        Remainder = input;
        ExpressionParser = expressionParser;
        ParseDelegate = parseDelegate;

        Position = -1;
        Indexes = Array.Empty<AggregatorInfo>();
        PreviousIndexes = Array.Empty<int>();
        LeftPart = string.Empty;
        LeftPartResult = Result<object>.NoContent();
        NextIndexes = Array.Empty<int>();
        RightPart = string.Empty;
        RightPartResult = Result<object>.NoContent();
    }

    internal void SetPosition(IGrouping<int, AggregatorBase> aggregators)
    {
        Indexes = aggregators
            .Select(x => new AggregatorInfo(x, Remainder.IndexOf(x.Character)))
            .Where(x => x.Index > -1)
            .OrderBy(x => x.Index)
            .ToArray();
        Position = Indexes.Any()
            ? Indexes.First().Index
            : -1;
    }

    internal void SetPreviousIndexes(int[] aggregatorPositions)
    {
        PreviousIndexes = aggregatorPositions;
        LeftPart = GetLeftPart();
        LeftPartResult = GetPartResult(LeftPart);
    }

    internal void SetNextIndexes(int[] aggregatorPositions)
    {
        NextIndexes = aggregatorPositions;
        RightPart = GetRightPart();
        RightPartResult = GetPartResult(RightPart);
    }

    internal Result<object> PerformAggregation()
    {
        var aggregateResult = Indexes.First().Aggregator.Aggregate(LeftPartResult.Value!, RightPartResult.Value!);

        if (aggregateResult.IsSuccessful())
        {
            AddResult(aggregateResult);
        }

        return aggregateResult;
    }

    private void AddResult(Result<object> aggregateResult)
    {
        Remainder = string.Concat
        (
            Remainder.Substring
            (
                0,
                PreviousIndexes.Any()
                    ? PreviousIndexes.First() + 1
                    : 0
            ),
            FormattableString.Invariant($"{Parsers.MathematicExpressionParser.TemporaryDelimiter}{Results.Count}{Parsers.MathematicExpressionParser.TemporaryDelimiter}"),
            (
                NextIndexes.Any()
                    ? Remainder.Substring(NextIndexes.First())
                    : string.Empty
            )
        );
        Results.Add(aggregateResult);
    }

    private string GetLeftPart()
        => PreviousIndexes.Any()
            ? Remainder.Substring(PreviousIndexes.First() + 1, Position - PreviousIndexes.First() - 1).Trim()
            : Remainder.Substring(0, Position).Trim();

    private string GetRightPart()
        => NextIndexes.Any()
            ? Remainder.Substring(Position + 1, NextIndexes.First() - Position - 1).Trim()
            : Remainder.Substring(Position + 1).Trim();

    private Result<object> GetPartResult(string part)
        => part.StartsWith(Parsers.MathematicExpressionParser.TemporaryDelimiter) && part.EndsWith(Parsers.MathematicExpressionParser.TemporaryDelimiter)
            ? Results[int.Parse(part.Substring(Parsers.MathematicExpressionParser.TemporaryDelimiter.Length, part.Length - (Parsers.MathematicExpressionParser.TemporaryDelimiter.Length * 2)), CultureInfo.InvariantCulture)]
            : ExpressionParser.Parse(part, FormatProvider);
}
