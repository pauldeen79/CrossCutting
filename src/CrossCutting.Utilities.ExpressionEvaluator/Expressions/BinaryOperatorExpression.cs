namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class BinaryOperatorExpression : IExpression<bool>
{
    private readonly IBinaryConditionGroupParser _binaryConditionGroupParser;
    private readonly IBinaryConditionGroupEvaluator _binaryConditionGroupEvaluator;

    public BinaryOperatorExpression(IBinaryConditionGroupParser binaryConditionGroupParser, IBinaryConditionGroupEvaluator binaryConditionGroupEvaluator)
    {
        ArgumentGuard.IsNotNull(binaryConditionGroupParser, nameof(binaryConditionGroupParser));
        ArgumentGuard.IsNotNull(binaryConditionGroupEvaluator, nameof(binaryConditionGroupEvaluator));

        _binaryConditionGroupParser = binaryConditionGroupParser;
        _binaryConditionGroupEvaluator = binaryConditionGroupEvaluator;
    }

    internal static readonly string[] OperatorExpressions = ["&&", "||"];

    public int Order => 40; // important: after ComparisonExpression. if the expression is recognized as a ComparisonExpression, it may contain binary operators as combinations

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
        => EvaluateTyped(context).Transform<object?>(x => x);

    public Result<bool> EvaluateTyped(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var foundAnyComparisonCharacter = context.FindAllOccurencedNotWithinQuotes(OperatorExpressions, StringComparison.Ordinal);
        if (!foundAnyComparisonCharacter)
        {
            return Result.Continue<bool>();
        }

        var conditionsResult = _binaryConditionGroupParser.Parse(context.Expression);
        if (!conditionsResult.IsSuccessful() || conditionsResult.Status == ResultStatus.Continue)
        {
            return Result.FromExistingResult<bool>(conditionsResult);
        }

        return _binaryConditionGroupEvaluator.Evaluate(context, conditionsResult.GetValueOrThrow());
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var foundAnyComparisonCharacter = context.FindAllOccurencedNotWithinQuotes(OperatorExpressions, StringComparison.Ordinal);
        if (!foundAnyComparisonCharacter)
        {
            return new ExpressionParseResultBuilder().WithExpressionType(typeof(BinaryOperatorExpression)).WithStatus(ResultStatus.Continue);
        }

        var conditionsResult = _binaryConditionGroupParser.Parse(context.Expression);
        if (!conditionsResult.IsSuccessful() || conditionsResult.Status == ResultStatus.Continue)
        {
            return new ExpressionParseResultBuilder()
                .WithStatus(conditionsResult.Status)
                .WithErrorMessage(conditionsResult.ErrorMessage)
                .AddValidationErrors(conditionsResult.ValidationErrors);
        }

        var result = new ExpressionParseResultBuilder()
            .WithExpressionType(typeof(BinaryOperatorExpression))
            .WithResultType(typeof(bool))
            .WithSourceExpression(context.Expression);

        var counter = 0;
        foreach (var condition in conditionsResult.GetValueOrThrow().Conditions)
        {
            result.AddPartResult(context.Parse(condition.Expression), $"Conditions[{counter}].Expression");
            counter++;
        }

        return result.SetStatusFromPartResults();
    }
}
