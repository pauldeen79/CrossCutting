namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IComparisonConditionGroupEvaluator
{
    Result<bool> Evaluate(ExpressionEvaluatorContext context, ComparisonConditionGroup conditionGroup);
}
