namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors.Operators;

public class EqualsOperator : OperatorExpressionProcessorBase
{
    public override int Order => 101;

    protected override string Sign => "==";

    protected override Result<bool> PerformOperator(object? leftValue, object? rightValue)
        => Result<bool>.Success(IsValid(leftValue, rightValue));

    internal static bool IsValid(object? leftValue, object? rightValue)
    {
        if (leftValue is null && rightValue is null)
        {
            return true;
        }

        if (leftValue is null || rightValue is null)
        {
            return false;
        }

        if (leftValue is string leftString && rightValue is string rightString)
        {
            return leftString.Equals(rightString, StringComparison.CurrentCultureIgnoreCase);
        }

        return leftValue.Equals(rightValue);
    }
}
