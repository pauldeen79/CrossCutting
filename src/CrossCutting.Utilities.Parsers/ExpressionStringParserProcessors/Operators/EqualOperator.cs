namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors.Operators;

public class EqualOperator : OperatorExpressionProcessorBase
{
    public override int Order => 101;

    protected override string Sign => "==";

    protected override Result<bool> PerformOperator(object? leftValue, object? rightValue)
        => IsValid(leftValue, rightValue, StringComparison.CurrentCultureIgnoreCase);

    internal static Result<bool> IsValid(object? leftValue, object? rightValue, StringComparison stringComparison)
    {
        if (leftValue is null && rightValue is null)
        {
            return Result<bool>.Success(true);
        }

        if (leftValue is null || rightValue is null)
        {
            return Result<bool>.Success(false);
        }

        if (leftValue is string leftString && rightValue is string rightString)
        {
            return Result<bool>.Success(leftString.Equals(rightString, stringComparison));
        }

        return Result<bool>.Success(leftValue.Equals(rightValue));
    }
}
