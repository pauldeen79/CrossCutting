namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors;

public class MathematicOperators : IMathematicExpressionProcessor
{
    internal static readonly AggregatorBase[] Aggregators = new AggregatorBase[]
    {
        new PowerAggregator(),
        new MultiplyAggregator(),
        new DivideAggregator(),
        new AddAggregator(),
        new SubtractAggregator(),
    };

    private readonly IExpressionParser _expressionParser;

    public MathematicOperators(IExpressionParser expressionParser)
    {
        _expressionParser = expressionParser;
    }

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
                    .Select(x => state.Remainder.LastIndexOf(x.Character, state.Position - 1))
                    .Where(x => x > -1)
                    .OrderByDescending(x => x)
                    .ToArray(), _expressionParser);

                state.SetNextIndexes(Aggregators
                    .Select(x => state.Remainder.IndexOf(x.Character, state.Position + 1))
                    .Where(x => x > -1)
                    .OrderBy(x => x)
                    .ToArray(), _expressionParser);

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
