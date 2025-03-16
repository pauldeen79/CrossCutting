namespace CrossCutting.Utilities.Parsers.Expressions;

public class OperatorExpression : IExpression
{
    private const string Pattern = @"(==|!=|<=|>=|<|>)";
    private static readonly Regex _operatorRegEx = new(Pattern, RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var matches = _operatorRegEx.Matches(context.Expression);

        // Ensure there's exactly one operator
        if (matches.Count != 1)
        {
            return Result.Continue<object?>();
        }

        string[] parts = Regex.Split(context.Expression, matches[0].Value);
        if (parts.Length != 2)
        {
            // More than one operator
            return Result.Continue<object?>();
        }

        var leftOperandResult = context.Evaluator.Evaluate(parts[0].Trim(), new ExpressionEvaluatorSettingsBuilder().WithFormatProvider(context.Settings.FormatProvider), context.Context);
        if (!leftOperandResult.IsSuccessful())
        {
            return Result.Invalid<object?>("Left operand is invalid, see inner results for more details", [leftOperandResult]);
        }

        var operatorSymbol = matches[0].Value;

        var rightOperandResult = context.Evaluator.Evaluate(parts[1].Trim(), new ExpressionEvaluatorSettingsBuilder().WithFormatProvider(context.Settings.FormatProvider), context.Context);
        if (!rightOperandResult.IsSuccessful())
        {
            return Result.Invalid<object?>("Right operand is invalid, see inner results for more details", [rightOperandResult]);
        }

        return operatorSymbol switch
        {
            "==" => Equal.Evaluate(leftOperandResult.Value, rightOperandResult.Value, context.Settings.StringComparison).Transform<object?>(x => x),
            _ => GreaterThan.Evaluate(leftOperandResult.Value, rightOperandResult.Value).Transform<object?>(x => x)
        };
    }

    public Result<Type> Validate(ExpressionEvaluatorContext context)
        => Result.Continue<Type>(); //TODO: Implement this
}
