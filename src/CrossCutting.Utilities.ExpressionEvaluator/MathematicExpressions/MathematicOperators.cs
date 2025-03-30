namespace CrossCutting.Utilities.ExpressionEvaluator.MathematicExpressions;

public class MathematicOperators : IMathematicExpression
{
    internal static readonly AggregatorBase[] Aggregators =
    [
        new PowerAggregator(),
        new MultiplyAggregator(),
        new DivideAggregator(),
        new ModulusAggregator(),
        new AddAggregator(),
        new SubtractAggregator(),
    ];

    public Result<MathematicExpressionState> Evaluate(MathematicExpressionState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

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

                state.SetPreviousIndexes([.. Aggregators
                    .Select(x => state.Remainder.LastIndexOf(x.Character, state.Position - 1))
                    .Where(x => x > -1)
                    .OrderByDescending(x => x)]);

                state.SetNextIndexes([.. Aggregators
                    .Select(x => state.Remainder.IndexOf(x.Character, state.Position + 1))
                    .Where(x => x > -1)
                    .OrderBy(x => x)]);

                if (!state.LeftPartResult.IsSuccessful())
                {
                    return Result.FromExistingResult<MathematicExpressionState>(state.LeftPartResult);
                }

                if (!state.RightPartResult.IsSuccessful())
                {
                    return Result.FromExistingResult<MathematicExpressionState>(state.RightPartResult);
                }

                var aggregateResult = state.PerformAggregation();
                if (!aggregateResult.IsSuccessful())
                {
                    return Result.FromExistingResult<MathematicExpressionState>(aggregateResult);
                }
            } while (state.Position > -1);
        }

        return Result.Success(state);
    }
}
