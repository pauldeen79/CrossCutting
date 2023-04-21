namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors;

internal class Operators : IMathematicExpressionProcessor
{
    internal static readonly Aggregator[] Aggregators = new Aggregator[]
    {
        new PowerAggregator(),
        new MultiplyAggregator(),
        new DivideAggregator(),
        new AddAggregator(),
        new SubtractAggregator(),
    };

    public Result<MathematicExpressionState> Process(MathematicExpressionState state)
    {
        foreach (var aggregators in Aggregators.GroupBy(x => x.Order))
        {
            do
            {
                // Within an operator group, the values should be evaluated from left to right.
                state.SetPosition(aggregators);

                if (state.Position == -1)
                {
                    continue;
                }

                state.SetPreviousIndexes(Aggregators
                    .Select(x => new AggregatorPosition(x.Character, state.Remainder.LastIndexOf(x.Character, state.Position - 1)))
                    .Where(x => x.Position > -1)
                    .OrderByDescending(x => x.Position)
                    .ToArray());

                state.SetNextIndexes(Aggregators
                    .Select(x => new AggregatorPosition(x.Character, state.Remainder.IndexOf(x.Character, state.Position + 1)))
                    .Where(x => x.Position > -1)
                    .OrderBy(x => x.Position)
                    .ToArray());

                if (!state.LeftPartResult.IsSuccessful())
                {
                    return Result<MathematicExpressionState>.FromExistingResult(state.LeftPartResult);
                }

                if (!state.RightPartResult.IsSuccessful())
                {
                    return Result<MathematicExpressionState>.FromExistingResult(state.RightPartResult);
                }

                var aggregateResult = state.PerformAggregation();
                if (!aggregateResult.IsSuccessful())
                {
                    return Result<MathematicExpressionState>.FromExistingResult(aggregateResult);
                }
            } while (state.Position > -1);
        }

        return Result<MathematicExpressionState>.Success(state);
    }
}
