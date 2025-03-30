namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IBinaryConditionGroupEvaluator
{
    Result<bool> Evaluate(ExpressionEvaluatorContext context, BinaryConditionGroup conditionGroup);
}
