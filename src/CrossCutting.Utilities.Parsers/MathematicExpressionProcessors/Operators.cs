namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors;

internal class Operators : IMathematicExpressionProcessor
{
    internal static readonly Dictionary<char, Tuple<int, Func<object, object, Result<object>>>> Aggregators = new()
    {
        { '^', new( 1, Power) },           // M
        { '*', new( 2, Multiply) },        // V
        { '/', new( 2, Divide) },          // D
        //{ '\u221A', SquareRoot }, // W
        { '+', new (3, Add) },             // O
        { '-', new (3, Subtract) },        // A
    };

    public Result<MathematicExpressionState> Process(MathematicExpressionState state)
    {
        foreach (var aggregators in Aggregators.GroupBy(x => x.Value.Item1))
        {
            var index = -1;
            do
            {
                // Within an operator group, the values should be evaluated from left to right.
                var indexes = aggregators
                    .Select(x => new { x.Key, Value = x.Value.Item2, Index = state.Remainder.IndexOf(x.Key) })
                    .Where(x => x.Index > -1)
                    .OrderBy(x => x.Index)
                    .ToArray();
                index = indexes.Any()
                    ? indexes.First().Index
                    : -1;

                if (index == -1)
                {
                    continue;
                }

                var previousIndexes = Aggregators.Keys.Select(x => new
                {
                    Key = x,
                    Index = index == 0
                        ? -1
                        : state.Remainder.LastIndexOf(x, index - 1)
                }).Where(x => x.Index > -1).OrderByDescending(x => x.Index).ToArray();

                string leftPart;
                if (previousIndexes.Any())
                {
                    leftPart = state.Remainder.Substring(previousIndexes.First().Index + 1, index - previousIndexes.First().Index - 1).Trim();
                }
                else
                {
                    leftPart = state.Remainder.Substring(0, index).Trim();
                }

                var nextIndexes = Aggregators.Keys.Select(x => new
                {
                    Key = x,
                    Index = index == state.Remainder.Length
                        ? -1
                        : state.Remainder.IndexOf(x, index + 1)
                }).Where(x => x.Index > -1).OrderBy(x => x.Index).ToArray();

                string rightPart;
                if (nextIndexes.Any())
                {
                    rightPart = state.Remainder.Substring(index + 1, nextIndexes.First().Index - index - 1).Trim();
                }
                else
                {
                    rightPart = state.Remainder.Substring(index + 1).Trim();
                }

                Result<object> leftPartResult;
                if (leftPart.StartsWith(MathematicExpressionParser.TemporaryDelimiter) && leftPart.EndsWith(MathematicExpressionParser.TemporaryDelimiter))
                {
                    leftPartResult = state.Results[int.Parse(leftPart.Substring(MathematicExpressionParser.TemporaryDelimiter.Length, leftPart.Length - (MathematicExpressionParser.TemporaryDelimiter.Length * 2)), CultureInfo.InvariantCulture)];
                }
                else
                {
                    leftPartResult = state.ParseExpressionDelegate.Invoke(leftPart);
                    if (!leftPartResult.IsSuccessful())
                    {
                        return Result<MathematicExpressionState>.FromExistingResult(leftPartResult);
                    }
                }

                Result<object> rightPartResult;
                if (rightPart.StartsWith(MathematicExpressionParser.TemporaryDelimiter) && rightPart.EndsWith(MathematicExpressionParser.TemporaryDelimiter))
                {
                    rightPartResult = state.Results[int.Parse(rightPart.Substring(MathematicExpressionParser.TemporaryDelimiter.Length, rightPart.Length - (MathematicExpressionParser.TemporaryDelimiter.Length * 2)), CultureInfo.InvariantCulture)];
                }
                else
                {
                    rightPartResult = state.ParseExpressionDelegate.Invoke(rightPart);
                    if (!rightPartResult.IsSuccessful())
                    {
                        return Result<MathematicExpressionState>.FromExistingResult(rightPartResult);
                    }
                }

                var aggregateResult = indexes.First().Value.Invoke(leftPartResult.Value!, rightPartResult.Value!);
                if (!aggregateResult.IsSuccessful())
                {
                    return Result<MathematicExpressionState>.FromExistingResult(aggregateResult);
                }

                state.Remainder = string.Concat
                (
                    state.Remainder.Substring(0, previousIndexes.Any() ? previousIndexes.First().Index + 1 : 0),
                    FormattableString.Invariant($"{MathematicExpressionParser.TemporaryDelimiter}{state.Results.Count}{MathematicExpressionParser.TemporaryDelimiter}"),
                    (
                        nextIndexes.Any()
                            ? state.Remainder.Substring(nextIndexes.First().Index)
                            : string.Empty
                    )
                );
                state.Results.Add(aggregateResult);

            } while (index > -1);
        }

        return Result<MathematicExpressionState>.Success(state);
    }

    private static Result<object> Power(object arg1, object arg2)
        => NumericAggregator.Evaluate(arg1, arg2
            , (x, y) => x ^ y
            , (x, y) => x ^ y
            , (x, y) => x ^ y
            , (x, y) => x ^ y
            , (x, y) => Math.Pow(x, y)
            , (x, y) => Math.Pow(Convert.ToDouble(x), Convert.ToDouble(y))
            , (x, y) => Math.Pow(x, y));

    private static Result<object> Multiply(object arg1, object arg2)
        => NumericAggregator.Evaluate(arg1, arg2
            , (x, y) => x * y
            , (x, y) => x * y
            , (x, y) => x * y
            , (x, y) => x * y
            , (x, y) => x * y
            , (x, y) => x * y
            , (x, y) => x * y);

    private static Result<object> Divide(object arg1, object arg2)
        => NumericAggregator.Evaluate(arg1, arg2
            , (x, y) => x / y
            , (x, y) => x / y
            , (x, y) => x / y
            , (x, y) => x / y
            , (x, y) => x / y
            , (x, y) => x / y
            , (x, y) => x / y);

    private static Result<object> Add(object arg1, object arg2)
        => NumericAggregator.Evaluate(arg1, arg2
            , (x, y) => x + y
            , (x, y) => x + y
            , (x, y) => x + y
            , (x, y) => x + y
            , (x, y) => x + y
            , (x, y) => x + y
            , (x, y) => x + y);

    private static Result<object> Subtract(object arg1, object arg2)
        => NumericAggregator.Evaluate(arg1, arg2
            , (x, y) => x - y
            , (x, y) => x - y
            , (x, y) => x - y
            , (x, y) => x - y
            , (x, y) => x - y
            , (x, y) => x - y
            , (x, y) => x - y);
}
