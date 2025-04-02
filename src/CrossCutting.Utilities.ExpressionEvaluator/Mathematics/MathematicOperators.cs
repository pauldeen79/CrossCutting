namespace CrossCutting.Utilities.ExpressionEvaluator.Mathematics;

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

        return Process(state, false);
    }

    public Result<MathematicExpressionState> Parse(MathematicExpressionState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        return Process(state, true);
    }

    private static Result<MathematicExpressionState> Process(MathematicExpressionState state, bool validateOnly)
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

                state.SetPreviousIndexes([.. Aggregators
                    .Select(x => state.Remainder.LastIndexOf(x.Character, state.Position - 1))
                    .Where(x => x > -1)
                    .OrderByDescending(x => x)], validateOnly);

                state.SetNextIndexes([.. Aggregators
                    .Select(x => state.Remainder.IndexOf(x.Character, state.Position + 1))
                    .Where(x => x > -1)
                    .OrderBy(x => x)], validateOnly);

                var validationState = ValidateState(state);
                if (!validationState.IsSuccessful())
                {
                    return validationState;
                }

                var aggregateResult = state.PerformAggregation(validateOnly);
                if (!aggregateResult.IsSuccessful())
                {
                    return Result.FromExistingResult<MathematicExpressionState>(aggregateResult);
                }
            } while (state.Position > -1);
        }

        return Result.Success(state);
    }

    private static Result<MathematicExpressionState> ValidateState(MathematicExpressionState state)
    {
        if (!state.LeftPartResult.IsSuccessful())
        {
            return Result.FromExistingResult<MathematicExpressionState>(state.LeftPartResult);
        }

        if (!state.LeftPartValidationResult.Status.IsSuccessful())
        {
            return Result.FromExistingResult<MathematicExpressionState>(state.LeftPartValidationResult.ToResult());
        }

        if (!state.RightPartResult.IsSuccessful())
        {
            return Result.FromExistingResult<MathematicExpressionState>(state.RightPartResult);
        }

        if (!state.RightPartValidationResult.Status.IsSuccessful())
        {
            return Result.FromExistingResult<MathematicExpressionState>(state.RightPartValidationResult.ToResult());
        }

        return Result.Continue<MathematicExpressionState>();
    }
}
