namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors.Operators;

public class NotEqualOperator : OperatorExpressionProcessorBase
{
    public override int Order => 102;

    protected override string Sign => "!=";

    protected override Result<bool> PerformOperator(object? leftValue, object? rightValue)
        => Result<bool>.FromExistingResult(EqualOperator.IsValid(leftValue, rightValue, StringComparison.CurrentCultureIgnoreCase), value => !value);
}
