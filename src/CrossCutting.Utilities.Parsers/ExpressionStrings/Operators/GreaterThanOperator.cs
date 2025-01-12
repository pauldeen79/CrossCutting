namespace CrossCutting.Utilities.Parsers.ExpressionStrings.Operators;

public class GreaterThanOperator : OperatorExpressionProcessorBase
{
    public override int Order => 104;

    protected override string Sign => ">";

    protected override Result<bool> PerformOperator(object? leftValue, object? rightValue)
        => GreaterThan.Evaluate(leftValue, rightValue);
}
