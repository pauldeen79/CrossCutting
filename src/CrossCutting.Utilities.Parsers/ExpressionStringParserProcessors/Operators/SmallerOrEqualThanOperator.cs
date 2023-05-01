namespace CrossCutting.Utilities.Parsers.ExpressionStringParserProcessors.Operators;

public class SmallerOrEqualThanOperator : OperatorExpressionProcessorBase
{
    public override int Order => 105;

    protected override string Sign => "<=";

    protected override Result<bool> PerformOperator(object? leftValue, object? rightValue)
    {
        try
        {
            return Result<bool>.Success(leftValue != null
                && rightValue != null
                && leftValue is IComparable c
                && c.CompareTo(rightValue) <= 0);
        }
        catch (ArgumentException ex)
        {
            return Result<bool>.Invalid(ex.Message);
        }
    }
}
