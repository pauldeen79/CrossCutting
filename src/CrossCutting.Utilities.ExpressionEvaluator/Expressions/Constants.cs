namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

[ExcludeFromCodeCoverage] // Don't know why, but I don't get code coverage on constants. But they're definitely covered in unit tests!
public static class Constants
{
    public const string LeftExpression = nameof(LeftExpression);
    public const string RightExpression = nameof(RightExpression);
    public const string Expression = nameof(Expression);
    public const string Operand = nameof(Operand);
}
