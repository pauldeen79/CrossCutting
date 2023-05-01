namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors.Operators;

public abstract class OperatorExpressionProcessorBase : IExpressionStringParserProcessor
{
    public abstract int Order { get; }
    protected abstract string Sign { get; }

    public Result<object?> Process(ExpressionStringParserState state)
    {
        if (state.Input.IndexOf(Sign) == -1)
        {
            return Result<object?>.Continue();
        }

        var split = state.Input.Substring(1).SplitDelimited(Sign, '\"', true, true);
        if (split.Length != 2)
        {
            return Result<object?>.Continue();
        }

        var leftResult = state.Parser.Parse($"={split[0]}", state.FormatProvider, state.Context);
        if (!leftResult.IsSuccessful())
        {
            return leftResult;
        }

        var rightResult = state.Parser.Parse($"={split[1]}", state.FormatProvider, state.Context);
        if (!rightResult.IsSuccessful())
        {
            return rightResult;
        }

        return Result<object?>.FromExistingResult(PerformOperator(leftResult.Value, rightResult.Value), value => value);
    }

    protected abstract Result<bool> PerformOperator(object? leftValue, object? rightValue);
}
