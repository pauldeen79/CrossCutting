namespace CrossCutting.Utilities.ExpressionEvaluator;

public partial record OperatorContext
{
    public object? LeftExpression => Results.GetValue(Constants.LeftExpression);
    public object? RightExpression => Results.GetValue(Constants.RightExpression);
}
