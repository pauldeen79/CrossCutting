namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IBinaryConditionGroupParser
{
    Result<BinaryConditionGroup> Parse(string expression);
}
