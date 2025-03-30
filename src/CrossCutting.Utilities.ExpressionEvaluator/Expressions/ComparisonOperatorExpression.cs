namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class ComparisonOperatorExpression : IExpression<bool>
{
    private readonly IComparisonConditionGroupParser _comparisonConditionGroupParser;
    private readonly IComparisonConditionGroupEvaluator _comparisonConditionGroupEvaluator;
    private readonly string[] _operatorExpressions;

    public ComparisonOperatorExpression(IComparisonConditionGroupParser comparisonConditionGroupParser, IComparisonConditionGroupEvaluator comparisonConditionGroupEvaluator, IEnumerable<IOperator> operators)
    {
        comparisonConditionGroupParser = ArgumentGuard.IsNotNull(comparisonConditionGroupParser, nameof(comparisonConditionGroupParser));
        comparisonConditionGroupEvaluator = ArgumentGuard.IsNotNull(comparisonConditionGroupEvaluator, nameof(comparisonConditionGroupEvaluator));
        operators = ArgumentGuard.IsNotNull(operators, nameof(operators));

        _comparisonConditionGroupParser = comparisonConditionGroupParser;
        _comparisonConditionGroupEvaluator = comparisonConditionGroupEvaluator;
        _operatorExpressions = operators.Select(x => x.OperatorExpression).ToArray();
    }

    public int Order => 30;

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
        => EvaluateTyped(context).Transform<object?>(x => x);

    public Result<bool> EvaluateTyped(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var foundAnyComparisonCharacter = context.FindAllOccurencedNotWithinQuotes(_operatorExpressions, StringComparison.Ordinal);
        if (!foundAnyComparisonCharacter)
        {
            return Result.Continue<bool>();
        }

        var conditionsResult = _comparisonConditionGroupParser.Parse(context.Expression);
        if (!conditionsResult.IsSuccessful() || conditionsResult.Status == ResultStatus.Continue)
        {
            return Result.FromExistingResult<bool>(conditionsResult);
        }

        return _comparisonConditionGroupEvaluator.Evaluate(context, conditionsResult.GetValueOrThrow());
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var foundAnyComparisonCharacter = context.FindAllOccurencedNotWithinQuotes(_operatorExpressions, StringComparison.Ordinal);
        if (!foundAnyComparisonCharacter)
        {
            return new ExpressionParseResultBuilder().WithExpressionType(typeof(ComparisonOperatorExpression)).WithStatus(ResultStatus.Continue);
        }

        var conditionsResult = _comparisonConditionGroupParser.Parse(context.Expression);
        if (!conditionsResult.IsSuccessful() || conditionsResult.Status == ResultStatus.Continue)
        {
            return new ExpressionParseResultBuilder()
                .WithStatus(conditionsResult.Status)
                .WithErrorMessage(conditionsResult.ErrorMessage)
                .AddValidationErrors(conditionsResult.ValidationErrors);
        }

        var result = new ExpressionParseResultBuilder()
            .WithExpressionType(typeof(ComparisonOperatorExpression))
            .WithResultType(typeof(bool))
            .WithSourceExpression(context.Expression);

        var counter = 0;
        foreach (var condition in conditionsResult.GetValueOrThrow().Conditions)
        {
            result.AddPartResult(context.Parse(condition.LeftExpression), $"Conditions[{counter}].LeftExpression");
            result.AddPartResult(context.Parse(condition.RightExpression), $"Conditions[{counter}].RightExpression");
            counter++;
        }

        return result.DetectStatusFromPartResults();
    }
}
