namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors.Operators;

public abstract class OperatorExpressionProcessorBase : IExpressionStringParserProcessor
{
    public abstract int Order { get; }
    protected abstract string Sign { get; }

    public Result<object?> Process(ExpressionStringParserState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Input.IndexOf(Sign) == -1)
        {
            return Result.Continue<object?>();
        }

        var split = state.Input.Substring(1).SplitDelimited(Sign, '\"', true, true);
        if (split.Length != 2)
        {
            return Result.Continue<object?>();
        }

        var leftResult = state.Parser.Parse($"={split[0]}", state.FormatProvider, state.Context, state.FormattableStringParser);
        if (!leftResult.IsSuccessful())
        {
            return leftResult;
        }

        var rightResult = state.Parser.Parse($"={split[1]}", state.FormatProvider, state.Context, state.FormattableStringParser);
        if (!rightResult.IsSuccessful())
        {
            return rightResult;
        }

        return Result.FromExistingResult<object?>(PerformOperator(leftResult.Value, rightResult.Value));
    }

    public Result Validate(ExpressionStringParserState state)
    {
        state = ArgumentGuard.IsNotNull(state, nameof(state));

        if (state.Input.IndexOf(Sign) == -1)
        {
            return Result.Continue();
        }

        var split = state.Input.Substring(1).SplitDelimited(Sign, '\"', true, true);
        if (split.Length != 2)
        {
            return Result.Continue();
        }

        var leftResult = state.Parser.Validate($"={split[0]}", state.FormatProvider, state.Context, state.FormattableStringParser);
        if (!leftResult.IsSuccessful())
        {
            return leftResult;
        }

        var rightResult = state.Parser.Validate($"={split[1]}", state.FormatProvider, state.Context, state.FormattableStringParser);
        if (!rightResult.IsSuccessful())
        {
            return rightResult;
        }

        return Result.Success();
    }

    protected abstract Result<bool> PerformOperator(object? leftValue, object? rightValue);
}
