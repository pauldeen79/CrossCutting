namespace CrossCutting.Utilities.Parsers;

internal class MathematicExpressionState
{
    internal string Input { get; }
    internal string Remainder { get; set; }
    internal List<Result<object>> Results { get; } = new();
    internal Func<string, Result<object>> ParseExpressionDelegate { get; }
    internal Func<string, Func<string, Result<object>>, Result<object>> ParseDelegate { get; }

    internal int Position { get; private set; }
    internal AggregatorInfo[] Indexes { get; private set; }
    internal AggregatorPosition[] PreviousIndexes { get; private set; }
    internal string LeftPart { get; private set; }
    internal Result<object> LeftPartResult { get; private set; }
    internal AggregatorPosition[] NextIndexes { get; private set; }
    internal string RightPart { get; private set; }
    internal Result<object> RightPartResult { get; private set; }

    internal MathematicExpressionState(string input, Func<string, Result<object>> parseExpressionDelegate, Func<string, Func<string, Result<object>>, Result<object>> parseDelegate)
    {
        Input = input;
        Remainder = input;
        ParseExpressionDelegate = parseExpressionDelegate;
        ParseDelegate = parseDelegate;

        Position = -1;
        Indexes = Array.Empty<AggregatorInfo>();
        PreviousIndexes = Array.Empty<AggregatorPosition>();
        LeftPart = string.Empty;
        LeftPartResult = Result<object>.NoContent();
        NextIndexes = Array.Empty<AggregatorPosition>();
        RightPart = string.Empty;
        RightPartResult = Result<object>.NoContent();
    }

    internal void SetPosition(IGrouping<int, Aggregator> aggregators)
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

    internal void SetPreviousIndexes(AggregatorPosition[] aggregatorPositions)
    {
        PreviousIndexes = aggregatorPositions;
        LeftPart = GetLeftPart();
        LeftPartResult = GetPartResult(LeftPart);
    }

    internal void SetNextIndexes(AggregatorPosition[] aggregatorPositions)
    {
        NextIndexes = aggregatorPositions;
        RightPart = GetRightPart();
        RightPartResult = GetPartResult(RightPart);
    }

    internal void AddResult(Result<object> aggregateResult)
    {
        Remainder = string.Concat
        (
            Remainder.Substring
            (
                0,
                PreviousIndexes.Any()
                    ? PreviousIndexes.First().Position + 1
                    : 0
            ),
            FormattableString.Invariant($"{MathematicExpressionParser.TemporaryDelimiter}{Results.Count}{MathematicExpressionParser.TemporaryDelimiter}"),
            (
                NextIndexes.Any()
                    ? Remainder.Substring(NextIndexes.First().Position)
                    : string.Empty
            )
        );
        Results.Add(aggregateResult);
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

    private string GetLeftPart()
        => PreviousIndexes.Any()
            ? Remainder.Substring(PreviousIndexes.First().Position + 1, Position - PreviousIndexes.First().Position - 1).Trim()
            : Remainder.Substring(0, Position).Trim();

    private string GetRightPart()
        => NextIndexes.Any()
            ? Remainder.Substring(Position + 1, NextIndexes.First().Position - Position - 1).Trim()
            : Remainder.Substring(Position + 1).Trim();

    private Result<object> GetPartResult(string part)
        => part.StartsWith(MathematicExpressionParser.TemporaryDelimiter) && part.EndsWith(MathematicExpressionParser.TemporaryDelimiter)
            ? Results[int.Parse(part.Substring(MathematicExpressionParser.TemporaryDelimiter.Length, part.Length - (MathematicExpressionParser.TemporaryDelimiter.Length * 2)), CultureInfo.InvariantCulture)]
            : ParseExpressionDelegate.Invoke(part);
}
