namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors.Operators;

public class NotEqualsOperator : OperatorExpressionProcessorBase
{
    public override int Order => 102;

    protected override string Sign => "!=";

    protected override bool PerformOperator(object? leftValue, object? rightValue)
        => !EqualsOperator.IsValid(leftValue, rightValue);
}
