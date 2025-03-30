namespace CrossCutting.Utilities.ExpressionEvaluator;

public class ComparisonConditionGroupEvaluator : IComparisonConditionGroupEvaluator
{
    public Result<bool> Evaluate(ExpressionEvaluatorContext context, ComparisonConditionGroup conditionGroup)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        conditionGroup = ArgumentGuard.IsNotNull(conditionGroup, nameof(conditionGroup));

        if (ConditionsAreSimple(conditionGroup))
        {
            return EvaluateSimpleConditions(context, conditionGroup);
        }

        return EvaluateComplexConditions(context, conditionGroup);
    }

    private static bool ConditionsAreSimple(ComparisonConditionGroup conditionGroup)
        => !conditionGroup.Conditions.Any(x =>
            (x.Combination ?? Combination.And) == Combination.Or
            || x.StartGroup
            || x.EndGroup);

    private static Result<bool> EvaluateSimpleConditions(ExpressionEvaluatorContext context, ComparisonConditionGroup conditionGroup)
    {
        foreach (var condition in conditionGroup.Conditions)
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

    private static Result<bool> EvaluateComplexConditions(ExpressionEvaluatorContext context, ComparisonConditionGroup conditionGroup)
    {
        var builder = new StringBuilder();
        foreach (var condition in conditionGroup.Conditions)
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

    private static Result<bool> EvaluateCondition(ComparisonCondition condition, ExpressionEvaluatorContext context)
    {
        var results = new ResultDictionaryBuilder()
            .Add(Constants.LeftExpression, () => context.Evaluate(condition.LeftExpression))
            .Add(Constants.RightExpression, () => context.Evaluate(condition.RightExpression))
            .Build();

        var error = results.GetError();
        if (error is not null)
        {
            return Result.FromExistingResult<bool>(error);
        }

        return condition.Operator.Evaluate(new OperatorContextBuilder().WithResults(results).WithStringComparison(context.Settings.StringComparison));
    }
}
