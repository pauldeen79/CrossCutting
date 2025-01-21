namespace CrossCutting.Utilities.Parsers.ExpressionStrings.Operators;

public class SmallerOrEqualThanOperator : OperatorExpressionProcessorBase
{
    protected override string Sign => "<=";

    protected override Result<bool> PerformOperator(object? leftValue, object? rightValue)
        => SmallerOrEqualThan.Evaluate(leftValue, rightValue);
}
