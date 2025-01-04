namespace CrossCutting.Utilities.Parsers.ExpressionStrings.Operators;

public class GreaterOrEqualThanOperator : OperatorExpressionProcessorBase
{
    public override int Order => 103;

    protected override string Sign => ">=";

    protected override Result<bool> PerformOperator(object? leftValue, object? rightValue)
        => GreaterOrEqualThan.Evaluate(leftValue, rightValue);
}
