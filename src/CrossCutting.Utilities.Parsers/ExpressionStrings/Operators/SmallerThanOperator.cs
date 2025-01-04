namespace CrossCutting.Utilities.Parsers.ExpressionStrings.Operators;

public class SmallerThanOperator : OperatorExpressionProcessorBase
{
    public override int Order => 106;

    protected override string Sign => "<";

    protected override Result<bool> PerformOperator(object? leftValue, object? rightValue)
        => SmallerThan.Evaluate(leftValue, rightValue);
}
