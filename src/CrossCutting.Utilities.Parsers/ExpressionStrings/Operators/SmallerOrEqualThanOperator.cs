namespace CrossCutting.Utilities.Parsers.ExpressionStrings.Operators;

public class SmallerOrEqualThanOperator : OperatorExpressionProcessorBase
{
    public override int Order => 105;

    protected override string Sign => "<=";

    protected override Result<bool> PerformOperator(object? leftValue, object? rightValue)
        => SmallerOrEqualThan.Evaluate(leftValue, rightValue);
}
