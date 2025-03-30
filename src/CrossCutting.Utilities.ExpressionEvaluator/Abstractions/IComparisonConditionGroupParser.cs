namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IComparisonConditionGroupParser
{
    Result<ComparisonConditionGroup> Parse(string expression);
}
