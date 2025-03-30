namespace CrossCutting.Utilities.ExpressionEvaluator;

public class BinaryConditionGroupEvaluator : IBinaryConditionGroupEvaluator
{
    public Result<bool> Evaluate(ExpressionEvaluatorContext context, BinaryConditionGroup conditionGroup)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        conditionGroup = ArgumentGuard.IsNotNull(conditionGroup, nameof(conditionGroup));

        if (ConditionsAreSimple(conditionGroup.Conditions))
        {
            return EvaluateSimpleConditions(context, conditionGroup.Conditions);
        }

        return EvaluateComplexConditions(context, conditionGroup.Conditions);
    }

    private static bool ConditionsAreSimple(IEnumerable<BinaryCondition> conditions)
        => !conditions.Any(x =>
            (x.Combination ?? Combination.And) == Combination.Or
            || x.StartGroup
            || x.EndGroup);

    private static Result<bool> EvaluateSimpleConditions(ExpressionEvaluatorContext context, IEnumerable<BinaryCondition> conditions)
    {
        foreach (var condition in conditions)
        {
            var itemResult = EvaluateCondition(condition, context);
            if (!itemResult.IsSuccessful())
            {
                return itemResult;
            }

            if (!itemResult.Value)
            {
                return itemResult;
            }
        }

        return Result.Success(true);
    }

    private static Result<bool> EvaluateComplexConditions(ExpressionEvaluatorContext context, IEnumerable<BinaryCondition> conditions)
    {
        var builder = new StringBuilder();
        foreach (var condition in conditions)
        {
            var itemResult = EvaluateCondition(condition, context);
            if (!itemResult.IsSuccessful())
            {
                return itemResult;
            }

            OperatorExpression.AppendCondition(builder, condition.Combination, condition.StartGroup, condition.EndGroup, itemResult.Value);
        }

        return Result.Success(OperatorExpression.EvaluateBooleanExpression(builder.ToString()));
    }

    private static Result<bool> EvaluateCondition(BinaryCondition condition, ExpressionEvaluatorContext context)
    {
        var expressionResult = context.Evaluate(condition.Expression);
        if (!expressionResult.IsSuccessful())
        {
            return Result.FromExistingResult<bool>(expressionResult);
        }

        if (expressionResult.Value is bool b)
        {
            return Result.Success(b);
        }
        else if (expressionResult.Value is string s)
        {
            // design decision: if it's a string, then do a null or empty check
            return Result.Success(!string.IsNullOrEmpty(s));
        }
        else
        {
            // design decision: if it's not a boolean, then do a null check
            return Result.Success(expressionResult.Value is not null);
        }
    }
}
