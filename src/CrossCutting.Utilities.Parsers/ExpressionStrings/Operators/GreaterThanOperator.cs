namespace CrossCutting.Utilities.Parsers.ExpressionStrings.Operators;

public class GreaterThanOperator : OperatorExpressionProcessorBase
{
    protected override string Sign => ">";

    protected override Result<bool> PerformOperator(object? leftValue, object? rightValue)
        => GreaterThan.Evaluate(leftValue, rightValue);
}
